using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.OData;
using Microsoft.Azure.Mobile.Server;
using ULFGService.DataObjects;
using ULFGService.Models;

namespace ULFGService.Controllers
{
    public class ChatMemberController : TableController<ChatMember>
    {
        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
            ULFGContext context = new ULFGContext();
            DomainManager = new EntityDomainManager<ChatMember>(context, Request);
        }

        // GET tables/ChatMember
        public IQueryable<ChatMember> GetAllChatMember()
        {
            return Query(); 
        }

        // GET tables/ChatMember/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public SingleResult<ChatMember> GetChatMember(string id)
        {
            return Lookup(id);
        }

        // PATCH tables/ChatMember/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task<ChatMember> PatchChatMember(string id, Delta<ChatMember> patch)
        {
             return UpdateAsync(id, patch);
        }

        // POST tables/ChatMember
        public async Task<IHttpActionResult> PostChatMember(ChatMember item)
        {
            ChatMember current = await InsertAsync(item);
            return CreatedAtRoute("Tables", new { id = current.Id }, current);
        }

        // DELETE tables/ChatMember/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task DeleteChatMember(string id)
        {
             return DeleteAsync(id);
        }
    }
}
