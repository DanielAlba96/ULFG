using ULFG.Forms.Publications.Views;
using ULFG.Forms.PrivateChat.Views;
using Xamarin.Forms;
using ULFG.Forms.Guilds.Views;
using ULFG.Forms.Profiles.Views;

namespace ULFG.Forms
{
    /// <summary>
    /// <see cref="TabbedPage"/> que represeta la página principal de la aplicación
    /// </summary>
    /// <remarks>Incluye pestañas con <see cref="PortalView"/>, <see cref="ChatListView"/> y <see cref="UserListView"/></remarks>
    public class MainPage:TabbedPage
    {
        public MainPage()
        {
            Title = "ULFG";
            Children.Add(new PortalView());
            Children.Add(new ChatListView());
            Children.Add(new UserListView());
        }        
    }
}
