using Xamarin.Forms;
using ULFG.Core.Data.Item;
using ULFG.Forms.Guilds.ViewModels;
using System;
using ULFG.Forms.PlatformInterfaces;
using System.Threading.Tasks;

namespace ULFG.Forms.Guilds.Views
{
    /// <summary>
    /// <see cref="ContentPage"/> que representa la vista de la página con la lista de gremios a los que se pertenece
    /// </summary>
    public class GuildListView : ContentPage
    {
        readonly Func<Task> refresh;

        /// <summary>
        /// Función para actualizar la página
        /// </summary>
        public Func<Task> Refresh
        {
            get { return refresh; }
        }

        public GuildListView()
        {
            Padding = new Thickness(0, 5, 0, 0);

            BindingContext = new GuildListViewModel(Navigation);

            refresh = ((GuildListViewModel)BindingContext).RefreshCommand;

            Title = "Mis Gremios";
            Icon = DependencyService.Get<IResourceManager>().GetResourcesPath("guild.png");

            ToolbarItem NewGuild = new ToolbarItem()
            {
                Text = "Crear Gremio",
                Order = ToolbarItemOrder.Primary,
                Icon = DependencyService.Get<IResourceManager>().GetResourcesPath("newGuild.png")
            };
            NewGuild.SetBinding(ToolbarItem.CommandProperty, "New");

            ToolbarItem refreshItem = new ToolbarItem()
            {
                Text = "Actualizar",
                Order = ToolbarItemOrder.Primary,
                Command = new Command(()=> refresh()),
                Icon = DependencyService.Get<IResourceManager>().GetResourcesPath("sync.png")
            };

            ToolbarItems.Add(refreshItem);
            ToolbarItems.Add(NewGuild);
                                 
            ListView list = new ListView()
            {
                IsPullToRefreshEnabled = true,
                RowHeight = 100,
                RefreshCommand = new Command(()=> refresh())
            };
            list.ItemTapped += async (sender, e) =>
            {
                await Navigation.PushAsync(new GuildProfileTabView((Guild)e.Item));
            };
            list.SetBinding(ListView.ItemTemplateProperty, "Template");
            list.SetBinding(ListView.ItemsSourceProperty, "Items");
            list.SetBinding(ListView.IsRefreshingProperty, "IsRefreshing", BindingMode.TwoWay);

            Content = list;
        }
    }
}