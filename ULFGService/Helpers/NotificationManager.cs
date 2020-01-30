using Microsoft.Azure.NotificationHubs;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;

namespace ULFGService.Helpers
{
    /// <summary>
    /// Gestiona el envío de las notificaciones push a los clientes
    /// </summary>
    public class NotificationManager
    {
        /// <summary>
        /// Nombre del servicio de Azure Notification Hub
        /// </summary>
        public static string NotificationHubName = "ULFGNotifications";

        /// <summary>
        /// URL del servicio de notificaciones de Azure Notification Hub
        /// </summary>
        public static string NotificationHubConnection = "Endpoint=sb://ulfgnotificationsnamespace.servicebus.windows.net/;SharedAccessKeyName=DefaultFullSharedAccessSignature;SharedAccessKey=OlRkKJYQUtNXWEFmYRuWChxlgNxJLXxbiwO6kaJdUi4=";

        /// <summary>
        /// Envía una notificacion a un cliente o conjunto de clientes
        /// </summary>
        /// <param name="config">Configuración actual del servidor HTTP</param>
        /// <param name="message">El contenido de la notificación</param>
        /// <param name="data">El contenido adicional que se quiera enviar</param>
        /// <param name="tags">Las etiquetas que sirven para filtran los clientes receptores</param>
        /// <returns>El resultado de la operación de envío</returns>
        public async Task<NotificationOutcome> SendNotification(HttpConfiguration config, string message, string data, params string[] tags)
        {
            NotificationOutcome result = null;
            // Get the Notification Hubs credentials for the mobile app.
            string notificationHubName = NotificationHubName;
            string notificationHubConnection = NotificationHubConnection;

            // Create a new Notification Hub client.
            NotificationHubClient hub = NotificationHubClient
            .CreateClientFromConnectionString(notificationHubConnection, notificationHubName);

            // Send the message so that all template registrations that contain "messageParam"
            // receive the notifications. This includes APNS, GCM, WNS, and MPNS template registrations.
            Dictionary<string, string> templateParams = new Dictionary<string, string>();
            templateParams["messageParam"] = message;
            templateParams["dataParam"] = data;

            string allTags = "";
            for (int i = 0; i < tags.Length; i++)
            {
                if (!i.Equals(tags.Length - 1))
                    allTags = allTags + tags[i] + "&&";
                else
                    allTags = allTags + tags[i];
            }
  
            try
            {
                // Send the push notification and log the results.
                result = await hub.SendTemplateNotificationAsync(templateParams, allTags);

                // Write the success result to the logs.
                config.Services.GetTraceWriter().Info(result.State.ToString());
            }
            catch (System.Exception ex)
            {
                // Write the failure result to the logs.
                config.Services.GetTraceWriter()
                    .Error(ex.Message, null, "Push.SendAsync Error");
            }
            return result;
        }
    }
}