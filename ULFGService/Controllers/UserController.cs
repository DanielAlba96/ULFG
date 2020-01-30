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
    public class UserController : TableController<User>
    {
        /// <summary>
        /// Inicializa el controller
        /// </summary>
        /// <param name="controllerContext">Contexto de inicialización</param>
        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
            ULFGContext context = new ULFGContext();
            DomainManager = new EntityDomainManager<User>(context, Request,true);
        }

        /// <summary>
        /// Operacion GET sobre la tabla
        /// </summary>
        /// <remarks>GET tables/User</remarks>
        /// <returns>Una lista con los elementos resultado de la consulta</returns>
        public IQueryable<User> GetAllUser()
        {
            return Query(); 
        }

        /// <summary>
        /// Busca un elemento por id
        /// </summary>
        /// <remarks>GET tables/User/48D68C86-6EA6-4C25-AA33-223FC9A27959</remarks>
        /// <param name="id">el id del elemento</param>
        /// <returns>Un elemento</returns>
        public SingleResult<User> GetUser(string id)
        {
            return Lookup(id);
        }

        /// <summary>
        /// Operación update sobre la tabla
        /// </summary>
        /// <remarks>PATCH tables/User/48D68C86-6EA6-4C25-AA33-223FC9A27959</remarks>
        /// <param name="id">id del elemento a actualizar</param>
        /// <param name="patch">Elemento a actualizar</param>
        /// <returns>El elemento actualizado</returns>
        public Task<User> PatchUser(string id, Delta<User> patch)
        {
             return UpdateAsync(id, patch);
        }

        /// <summary>
        /// Inserta un elemento en la tabla
        /// </summary>
        /// <remarks>POST tables/User</remarks>
        /// <param name="item">El elemento a insertar</param>
        /// <returns>El resultado de la operación</returns>
        public async Task<IHttpActionResult> PostUser(User item)
        {
            User current = await InsertAsync(item);
            return CreatedAtRoute("Tables", new { id = current.Id }, current);
        }

        /// <summary>
        /// Borra un elemento de la tabla
        /// </summary>
        /// <remarks>DELETE tables/User/48D68C86-6EA6-4C25-AA33-223FC9A27959</remarks>
        /// <param name="id">El id del elemento a borrar</param>
        public Task DeleteUser(string id)
        {
             return DeleteAsync(id);
        }
    }
}
