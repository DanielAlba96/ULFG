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
    /// Controller de la tabla Guilds
    /// </summary>
    public class GuildMemberController : TableController<GuildMember>
    {
        /// <summary>
        /// Inicializa el controller
        /// </summary>
        /// <param name="controllerContext">Contexto de inicialización</param>
        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
            ULFGContext context = new ULFGContext();
            DomainManager = new EntityDomainManager<GuildMember>(context, Request, true);
        }

        /// <summary>
        /// Operacion GET sobre la tabla
        /// </summary>
        /// <remarks>GET tables/GuildMember</remarks>
        /// <returns>Una lista con los elementos resultado de la consulta</returns>
        public IQueryable<GuildMember> GetAllGuildMember()
        {
            return Query();
        }

        /// <summary>
        /// Busca un elemento por id
        /// </summary>
        /// <remarks>GET tables/GuildMember/48D68C86-6EA6-4C25-AA33-223FC9A27959</remarks>
        /// <param name="id">el id del elemento</param>
        /// <returns>Un elemento</returns>
        public SingleResult<GuildMember> GetGuildMember(string id)
        {
            return Lookup(id);
        }

        /// <summary>
        /// Operación update sobre la tabla
        /// </summary>
        /// <remarks>PATCH tables/GuildMember/48D68C86-6EA6-4C25-AA33-223FC9A27959</remarks>
        /// <param name="id">id del elemento a actualizar</param>
        /// <param name="patch">Elemento a actualizar</param>
        /// <returns>El elemento actualizado</returns>
        public Task<GuildMember> PatchGuildMember(string id, Delta<GuildMember> patch)
        {
            return UpdateAsync(id, patch);
        }

        /// <summary>
        /// Inserta un elemento en la tabla
        /// </summary>
        /// <remarks>POST tables/GuildMember</remarks>
        /// <param name="item">El elemento a insertar</param>
        /// <returns>El resultado de la operación</returns>
        public async Task<IHttpActionResult> PostGuildMember(GuildMember item)
        {
            GuildMember current = await InsertAsync(item);
            return CreatedAtRoute("Tables", new { id = current.Id }, current);
        }

        /// <summary>
        /// Borra un elemento de la tabla
        /// </summary>
        /// <remarks>DELETE tables/GuildMember/48D68C86-6EA6-4C25-AA33-223FC9A27959</remarks>
        /// <param name="id">El id del elemento a borrar</param>
        public Task DeleteGuildMember(string id)
        {
            ULFGContext db = new ULFGContext();
            var guildMember = db.GuildMembers.FirstOrDefault(m => m.Id == id);
            var guild = db.GuildItems.FirstOrDefault(x => x.Id == guildMember.Guild_id);
            if (guildMember != null)
            {
                NotificationHubClient hub = NotificationHubClient.CreateClientFromConnectionString(
                    NotificationManager.NotificationHubConnection, NotificationManager.NotificationHubName);
        
                if (!guild.Leader_id.Equals(guildMember.Member_id))
                {
                    NotificationManager not = new NotificationManager();
                    Task.Run(() => not.SendNotification(this.Configuration, "Has sido expulsado del gremio " + guild.Name, "GuildKick " + guild.Id, guildMember.Member_id));
                }
            }
            return DeleteAsync(id);
        }
    }
}
