using Microsoft.Azure.NotificationHubs;
using Microsoft.Azure.NotificationHubs.Messaging;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using ULFGService.Helpers;
using System.Linq;

namespace ULFGService.Controllers
{
    /// <summary>
    /// Gestiona los registros de los dispositivos cliente en el servicio de notificaciones
    /// </summary>
    [RoutePrefix("deviceregister")]
    public class DeviceRegistrationController : ApiController
    {
        /// <summary>
        /// Servicio de notificaciones
        /// </summary>
        private NotificationHubClient hub;

        /// <summary>
        /// Inicializa el servicio de notificaciones
        /// </summary>
        public DeviceRegistrationController()
        {
            hub = NotificationHubClient.CreateClientFromConnectionString(NotificationManager.NotificationHubConnection, NotificationManager.NotificationHubName);
        }

        /// <summary>
        /// Representa el registro de un dispositivo cliente
        /// </summary>
        public class DeviceRegistration
        {
            /// <summary>
            /// Plataforma del cliente
            /// </summary>
            public string Platform { get; set; }

            /// <summary>
            /// Identificador del dispositivo
            /// </summary>
            public string Handle { get; set; }

            /// <summary>
            /// ID del usuario logeado en el dispositivo
            /// </summary>
            public string UserTag { get; set; }
        }

        /// <summary>
        /// Crea un id de registro en el servicio de notificaciones
        /// </summary>
        /// <remarks>api/deviceregister</remarks>
        /// <returns>El id de registro en el servicio</returns>
        [HttpPost]
        [Route("")]
        public async Task<string> RegisterDevice()
        {
            return await hub.CreateRegistrationIdAsync();
        }


        /// <summary>
        /// Registra o actualiza un dispositivo cliente en el servicio de notificaciones usando la información del cliente y el id de registro 
        /// </summary>
        /// <remarks>PUT api/deviceregister/5</remarks>
        /// <param name="id">id de registro en el servicio</param>
        /// <param name="deviceUpdate">Información del cliente</param>
        [HttpPut]
        [Route("{id}")]
        public void UpdateRegistration(string id, DeviceRegistration deviceUpdate)
        {
            RegistrationDescription registration = null;

            const string templateBodyGCM =
                         "{" +
                             "\"notification\" : {" +
                                    "\"body\" : \"$(messageParam)\"," +
                                    "\"title\" : \"ULFG: La red social para jugadores\"," +
                                    "\"icon\" : \"icon\" }," +
                             "\"data\" : {" +
                                    "\"content\" : \"$(dataParam)\" }" +
                        "}";

            const string templateBodyWNS =
                      "<toast displayTimestamp=\"2017 - 04 - 15T19: 45:00Z\">" +

                      "<visual>" +
                      "<binding template=\"ToastGeneric\">" +
                      "<text id=\"1\">ULFG: La red social para jugadores</text>" +
                      "<text id=\"2\">$(messageParam)</text>" +
                      "</binding>" +
                      "</visual>" +
                      "</toast>";

            switch (deviceUpdate.Platform)
            {
                case "wns":
                    registration = new WindowsTemplateRegistrationDescription(deviceUpdate.Handle, templateBodyWNS);
                    break;
                case "gcm":
                    registration = new GcmTemplateRegistrationDescription(deviceUpdate.Handle, templateBodyGCM);
                    break;
                default:
                    throw new HttpResponseException(HttpStatusCode.BadRequest);
            }

            HashSet<string> tags = new HashSet<string>();
            tags.Add(deviceUpdate.UserTag);

            ULFGContext db = new ULFGContext();
            var members = db.GuildMembers.Where(x => x.Member_id == deviceUpdate.UserTag && !x.Deleted);
            foreach (var g in members)
                tags.Add(g.Guild_id);

            registration.RegistrationId = id;
            registration.Tags = tags;

            try
            {
                hub.CreateOrUpdateRegistrationAsync(registration).Wait();
            }
            catch (MessagingException e)
            {
                ReturnGoneIfHubResponseIsGone(e);
            }
        }

        /// <summary>
        /// Elimina el registro de un dispositivo cliente en el servicio e invalida el id
        /// </summary>
        /// <remarks>DELETE api/deviceregister/5</remarks>
        /// <param name="id">el id de registro en el servicio</param>
        [HttpDelete]
        [Route("{id}")]
        public async Task DeregisterDevice(string id)
        {
            await hub.DeleteRegistrationAsync(id);
        }

        private static void ReturnGoneIfHubResponseIsGone(MessagingException e)
        {
            var webex = e.InnerException as WebException;
            if (webex.Status == WebExceptionStatus.ProtocolError)
            {
                var response = (HttpWebResponse)webex.Response;
                if (response.StatusCode == HttpStatusCode.Gone)
                    throw new HttpResponseException(HttpStatusCode.Gone);
            }
        }
    }
}