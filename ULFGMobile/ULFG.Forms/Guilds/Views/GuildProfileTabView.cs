using ULFG.Core.Data.Item;
using ULFG.Forms.Guilds.ViewModels;
using ULFG.Forms.PlatformInterfaces;
using Xamarin.Forms;

namespace ULFG.Forms.Guilds.Views
{
    /// <summary>
    /// <see cref="TabbedPage"/> que representa la página principal del detalle de un gremio
    /// </summary>
    /// <remarks>Incluye pestañas con <see cref="GuildInfoView"/> y <see cref="MemberListView"/></remarks>
    public class GuildProfileTabView :TabbedPage
    {
        readonly User user = (User)Application.Current.Properties["user"];
        public GuildProfileTabView(Guild guild)
        {
            BindingContext = new GuildProfileTabViewModel(Navigation, guild);

            this.SetBinding(TabbedPage.TitleProperty, "Title");

            ToolbarItem invite = new ToolbarItem()
            {
                Text = "Invitar",
                Order = ToolbarItemOrder.Primary,
                Icon = DependencyService.Get<IResourceManager>().GetResourcesPath("invite.png")
            };
            invite.SetBinding(ToolbarItem.CommandProperty, "Invite");

            ToolbarItem admin = new ToolbarItem()
            {
                Text = "Administrar",
                Order = ToolbarItemOrder.Secondary
            };
            admin.SetBinding(ToolbarItem.CommandProperty, "Edit");

            ToolbarItem leave = new ToolbarItem()
            {
                Text = "Abandonar",
                Order = ToolbarItemOrder.Secondary
            };
            leave.SetBinding(ToolbarItem.CommandProperty, "Leave");
            ToolbarItem disband = new ToolbarItem()
            {
                Text = "Deshacer gremio",
                Order = ToolbarItemOrder.Secondary
            };
            disband.SetBinding(ToolbarItem.CommandProperty, "Disband");
            ToolbarItem contact = new ToolbarItem()
            {
                Text = "Contactar lider",
                Order = ToolbarItemOrder.Secondary
            };
            contact.SetBinding(ToolbarItem.CommandProperty, "Contact");
            ToolbarItem chat = new ToolbarItem()
            {
                Text = "Ir al chat",
                Order = ToolbarItemOrder.Primary,
                Icon = DependencyService.Get<IResourceManager>().GetResourcesPath("chat.png")
            };
            chat.SetBinding(ToolbarItem.CommandProperty, "Chat");

            ToolbarItems.Add(invite);
            ToolbarItems.Add(chat);            
            if (user.Id.Equals(guild.Leader))
            {
                ToolbarItems.Add(admin);
                ToolbarItems.Add(disband);
            }
            else
            {
                ToolbarItems.Add(contact);
                ToolbarItems.Add(leave);
            }

            Children.Add(new GuildInfoView(guild));
            Children.Add(new MemberListView(guild));
        }
    }
}
