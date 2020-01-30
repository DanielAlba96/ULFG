using ULFG.Core.Data.Item;
using ULFG.Forms.Guilds.ViewModels;
using ULFG.Forms.Shared;

namespace ULFG.Forms.Guilds.Views
{
    /// <summary>
    /// <see cref="ChatView"/> que representa la vista de la página del chat de gremio
    /// </summary>
    public class GuildChatView : ChatView
    {
        public GuildChatView(Guild guild): base()
        {           
            BindingContext = new GuildChatViewModel(Navigation, guild);
            Title = "Chat del gremio " + guild.Name;
        }
    }
}