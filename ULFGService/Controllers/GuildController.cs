using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.OData;
using Microsoft.Azure.Mobile.Server;
using ULFGService.DataObjects;

namespace ULFGService.Controllers
{
    /// <summary>
    /// Controller de la tabla Guilds
    /// </summary>
    public class GuildController : TableController<Guild>
    {
        /// <summary>
        /// Inicializa el controller
        /// </summary>
        /// <param name="controllerContext">Contexto de inicialización</param>
        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
            ULFGContext context = new ULFGContext();
            DomainManager = new EntityDomainManager<Guild>(context, Request,true);
        }

        /// <summary>
        /// Operacion GET sobre la tabla
        /// </summary>
        /// <remarks>GET tables/Guild</remarks>
        /// <returns>Una lista con los elementos resultado de la consulta</returns>
        public IQueryable<Guild> GetAllGuild()
        {
            return Query(); 
        }

        /// <summary>
        /// Busca un elemento por id
        /// </summary>
        /// <remarks>GET tables/Guild/48D68C86-6EA6-4C25-AA33-223FC9A27959</remarks>
        /// <param name="id">el id del elemento</param>
        /// <returns>Un elemento</returns>
        public SingleResult<Guild> GetGuild(string id)
        {
            return Lookup(id);
        }

        /// <summary>
        /// Operación update sobre la tabla
        /// </summary>
        /// <remarks>PATCH tables/Guild/48D68C86-6EA6-4C25-AA33-223FC9A27959</remarks>
        /// <param name="id">id del elemento a actualizar</param>
        /// <param name="patch">Elemento a actualizar</param>
        /// <returns>El elemento actualizado</returns>
        public Task<Guild> PatchGuild(string id, Delta<Guild> patch)
        {
            var item = patch.GetEntity();
            return UpdateAsync(id, patch);
        }

        /// <summary>
        /// Inserta un elemento en la tabla
        /// </summary>
        /// <remarks>POST tables/Guild</remarks>
        /// <param name="item">El elemento a insertar</param>
        /// <returns>El resultado de la operación</returns>
        public async Task<IHttpActionResult> PostGuild(Guild item)
        {
            Guild current = await InsertAsync(item);
            return CreatedAtRoute("Tables", new { id = current.Id }, current);
        }

        /// <summary>
        /// Borra un elemento de la tabla
        /// </summary>
        /// <remarks>DELETE tables/Guild/48D68C86-6EA6-4C25-AA33-223FC9A27959</remarks>
        /// <param name="id">El id del elemento a borrar</param>
        public Task DeleteGuild(string id)
        {
             return DeleteAsync(id);
        }
    }
}
