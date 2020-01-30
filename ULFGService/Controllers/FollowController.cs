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
    /// Controller de la tabla Follows
    /// </summary>
    public class FollowController : TableController<Follow>
    {
        /// <summary>
        /// Inicializa el controller
        /// </summary>
        /// <param name="controllerContext">Contexto de inicialización</param>
        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
            ULFGContext context = new ULFGContext();
            DomainManager = new EntityDomainManager<Follow>(context, Request,true);
        }

        /// <summary>
        /// Operacion GET sobre la tabla
        /// </summary>
        /// <remarks>GET tables/Follow</remarks>
        /// <returns>Una lista con los elementos resultado de la consulta</returns>
        public IQueryable<Follow> GetAllFollow()
        {
            return Query(); 
        }

        /// <summary>
        /// Busca un elemento por id
        /// </summary>
        /// <remarks>GET tables/Follow/48D68C86-6EA6-4C25-AA33-223FC9A27959</remarks>
        /// <param name="id">el id del elemento</param>
        /// <returns>Un elemento</returns>
        public SingleResult<Follow> GetFollow(string id)
        {
            return Lookup(id);
        }

        /// <summary>
        /// Operación update sobre la tabla
        /// </summary>
        /// <remarks>PATCH tables/Follow/48D68C86-6EA6-4C25-AA33-223FC9A27959</remarks>
        /// <param name="id">id del elemento a actualizar</param>
        /// <param name="patch">Elemento a actualizar</param>
        /// <returns>El elemento actualizado</returns>
        public Task<Follow> PatchFollow(string id, Delta<Follow> patch)
        {
             return UpdateAsync(id, patch);
        }

        /// <summary>
        /// Inserta un elemento en la tabla
        /// </summary>
        /// <remarks>POST tables/Follow</remarks>
        /// <param name="item">El elemento a insertar</param>
        /// <returns>El resultado de la operación</returns>
        public async Task<IHttpActionResult> PostFollow(Follow item)
        {
            Follow current = await InsertAsync(item);
            if (current != null)
            {
                NotificationManager not = new NotificationManager();
                ULFGContext db = new ULFGContext();
                var following = db.UserItems.Find(item.Following_user);
#pragma warning disable 4014
                Task.Run(()=> not.SendNotification(this.Configuration, "Te ha seguido @" + following.Username, "", item.Followed_user));
#pragma warning restore 4014
            }
            return CreatedAtRoute("Tables", new { id = current.Id }, current);
        }

        /// <summary>
        /// Borra un elemento de la tabla
        /// </summary>
        /// <remarks>DELETE tables/Follow/48D68C86-6EA6-4C25-AA33-223FC9A27959</remarks>
        /// <param name="id">El id del elemento a borrar</param>
        public Task DeleteFollow(string id)
        {
             return DeleteAsync(id);
        }
    }
}
