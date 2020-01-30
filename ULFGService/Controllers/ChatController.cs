using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.OData;
using Microsoft.Azure.Mobile.Server;
using Microsoft.Azure.NotificationHubs;
using ULFGService.DataObjects;
using ULFGService.Helpers;

namespace ULFGService.Controllers
{
    /// <summary>
    /// Controller de la tabla Chats
    /// </summary>
    public class ChatController : TableController<Chat>
    {
        /// <summary>
        /// Inicializa el controller
        /// </summary>
        /// <param name="controllerContext">Contexto de inicialización</param>
        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
            ULFGContext context = new ULFGContext();
            DomainManager = new EntityDomainManager<Chat>(context, Request,true);
        }

        /// <summary>
        /// Operacion GET sobre la tabla
        /// </summary>
        /// <remarks>GET tables/Chat</remarks>
        /// <returns>Una lista con los elementos resultado de la consulta</returns>
        public IQueryable<Chat> GetAllChat()
        {
            return Query(); 
        }

        /// <summary>
        /// Busca un elemento por id
        /// </summary>
        /// <remarks>GET tables/Chat/48D68C86-6EA6-4C25-AA33-223FC9A27959</remarks>
        /// <param name="id">el id del elemento</param>
        /// <returns>Un elemento</returns>
        public SingleResult<Chat> GetChat(string id)
        {
            return Lookup(id);
        }
 
        /// <summary>
        /// Operación update sobre la tabla
        /// </summary>
        /// <remarks>PATCH tables/Chat/48D68C86-6EA6-4C25-AA33-223FC9A27959</remarks>
        /// <param name="id">id del elemento a actualizar</param>
        /// <param name="patch">Elemento a actualizar</param>
        /// <returns>El elemento actualizado</returns>
        public Task<Chat> PatchChat(string id, Delta<Chat> patch)
        {
            var chat = patch.GetEntity();
            NotificationHubClient hub = NotificationHubClient.CreateClientFromConnectionString(
               NotificationManager.NotificationHubConnection, NotificationManager.NotificationHubName);
            CollectionQueryResult<RegistrationDescription> registrations;
            if (chat.Member1_deleted)
            {
                registrations = hub.GetRegistrationsByTagAsync(chat.Member1_id, 10).Result;
                foreach(var reg in registrations)
                {
                    reg.Tags.Remove(chat.Id);
                    hub.UpdateRegistrationAsync(reg).Wait();
                }
            }
            else if (chat.Member2_deleted)
            {
                registrations = hub.GetRegistrationsByTagAsync(chat.Member2_id, 10).Result;
                foreach (var reg in registrations)
                {
                    reg.Tags.Remove(chat.Id);
                    hub.UpdateRegistrationAsync(reg).Wait();
                }
            }

             return UpdateAsync(id, patch);
        }

        /// <summary>
        /// Inserta un elemento en la tabla
        /// </summary>
        /// <remarks>POST tables/Chat</remarks>
        /// <param name="item">El elemento a insertar</param>
        /// <returns>El resultado de la operación</returns>
        public async Task<IHttpActionResult> PostChat(Chat item)
        {
            Chat current = await InsertAsync(item);
            return CreatedAtRoute("Tables", new { id = current.Id }, current);
        }

        /// <summary>
        /// Borra un elemento de la tabla
        /// </summary>
        /// <remarks>DELETE tables/Chat/48D68C86-6EA6-4C25-AA33-223FC9A27959</remarks>
        /// <param name="id">El id del elemento a borrar</param>
        public Task DeleteChat(string id)
        {
             return DeleteAsync(id);
        }
    }
}
