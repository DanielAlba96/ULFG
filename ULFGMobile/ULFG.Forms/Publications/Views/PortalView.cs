using Xamarin.Forms;
using ULFG.Forms.Publications.ViewModels;
using ULFG.Forms.PlatformInterfaces;

namespace ULFG.Forms.Publications.Views
{
    /// <summary>
    /// <see cref="ContentPage"/> que representa la vista del portal con las publicaciones
    /// </summary>
    public class PortalView : ContentPage
    {
        public PortalView()
        {
            Padding = new Thickness(0, 15, 0, 0);

            BindingContext = new PortalViewModel(Navigation);
            Title = "Portal";
            Icon = DependencyService.Get<IResourceManager>().GetResourcesPath("portal.png");
            ToolbarItem newP = new ToolbarItem()
            {
                Text = "Publicar",
                Icon = DependencyService.Get<IResourceManager>().GetResourcesPath("newPub.png"),
                Order = ToolbarItemOrder.Primary
            };
            newP.SetBinding(ToolbarItem.CommandProperty, "NewPublication");

            ToolbarItem refresh = new ToolbarItem()
            {
                Text = "Actualizar",
                Icon = DependencyService.Get<IResourceManager>().GetResourcesPath("sync.png"),
                Order = ToolbarItemOrder.Primary
            };
            refresh.SetBinding(ToolbarItem.CommandProperty, "Refresh");

            ToolbarItems.Add(refresh);
            ToolbarItems.Add(newP);
                        
            ListView list = new ListView(ListViewCachingStrategy.RecycleElement)
            {
                IsPullToRefreshEnabled = true,
                HasUnevenRows = true,
                SeparatorColor = Color.Black
            };
            list.SetBinding(ListView.ItemsSourceProperty, "Items");
            list.SetBinding(ListView.RefreshCommandProperty, "Refresh");
            list.SetBinding(ListView.IsRefreshingProperty, "IsRefreshing");
            list.SetBinding(ListView.ItemTemplateProperty, "Template");

            Content = list;
        }
    }
}