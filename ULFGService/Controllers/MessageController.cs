using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.OData;
using Microsoft.Azure.Mobile.Server;
using ULFGService.DataObjects;
using ULFGService.Helpers;

namespace ULFGService.Controllers
{
    /// <summary>
    /// Controller de la tabla Guilds
    /// </summary>
    public class MessageController : TableController<Message>
    {
        /// <summary>
        /// Inicializa el controller
        /// </summary>
        /// <param name="controllerContext">Contexto de inicialización</param>
        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
            ULFGContext context = new ULFGContext();
            DomainManager = new EntityDomainManager<Message>(context, Request,true);
        }

        /// <summary>
        /// Operacion GET sobre la tabla
        /// </summary>
        /// <remarks>GET tables/Message</remarks>
        /// <returns>Una lista con los elementos resultado de la consulta</returns>
        public IQueryable<Message> GetAllMessage()
        {
            return Query(); 
        }

        /// <summary>
        /// Busca un elemento por id
        /// </summary>
        /// <remarks>GET tables/Message/48D68C86-6EA6-4C25-AA33-223FC9A27959</remarks>
        /// <param name="id">el id del elemento</param>
        /// <returns>Un elemento</returns>
        public SingleResult<Message> GetMessage(string id)
        {
            return Lookup(id);
        }

        /// <summary>
        /// Operación update sobre la tabla
        /// </summary>
        /// <remarks>PATCH tables/Message/48D68C86-6EA6-4C25-AA33-223FC9A27959</remarks>
        /// <param name="id">id del elemento a actualizar</param>
        /// <param name="patch">Elemento a actualizar</param>
        /// <returns>El elemento actualizado</returns>
        public Task<Message> PatchMessage(string id, Delta<Message> patch)
        {
             return UpdateAsync(id, patch);
        }

        /// <summary>
        /// Inserta un elemento en la tabla
        /// </summary>
        /// <remarks>POST tables/Message/</remarks>
        /// <param name="item">El elemento a insertar</param>
        /// <returns>El resultado de la operación</returns>
        public async Task<IHttpActionResult> PostMessage(Message item)
        {
            Message current = await InsertAsync(item);
            if (current != null)
            {
                NotificationManager not = new NotificationManager();
                ULFGContext db = new ULFGContext();
                if (item.Guild_id != null)
                {
                    var guild = db.GuildItems.Find(item.Guild_id);
                    var users = db.GuildMembers.Where(x => x.Guild_id == item.Guild_id).Select(x=> x.Member_id).ToArray();
#pragma warning disable 4014
                    Task.Run (()=> not.SendNotification(this.Configuration, "Nuevo mensaje grupal en el gremio " + guild.Name, "Message Guild " + item.Guild_id, users));
                }
                else
                {
                    var chat = db.ChatItems.Find(item.Chat_id);
                    string receiver;
                    string sender;
                    if (chat.Member1_id.Equals(item.Creator_id))
                    {
                        receiver = chat.Member2_id;
                        sender = chat.Member1_id;
                    }
                    else
                    {
                        receiver = chat.Member1_id;
                        sender = chat.Member2_id;
                    }
                    var sendername = db.UserItems.Find(sender).Username;
                    Task.Run(()=> not.SendNotification(this.Configuration, "Nuevo mensaje directo de @" + sendername, "Message Chat " + current.Id, receiver));
#pragma warning restore 4014
                }
            }
            return CreatedAtRoute("Tables", new { id = current.Id }, current);
        }

        /// <summary>
        /// Borra un elemento de la tabla
        /// </summary>
        /// <remarks>DELETE tables/Message/48D68C86-6EA6-4C25-AA33-223FC9A27959</remarks>
        /// <param name="id">El id del elemento a borrar</param>
        public Task DeleteMessage(string id)
        {
             return DeleteAsync(id);
        }
    }
}
