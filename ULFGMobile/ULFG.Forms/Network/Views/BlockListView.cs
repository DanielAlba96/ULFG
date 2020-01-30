using System;
using System.Threading.Tasks;
using ULFG.Core.Data.Item;
using ULFG.Forms.Network.ViewModels;
using ULFG.Forms.PlatformInterfaces;
using ULFG.Forms.Profiles.Views;
using Xamarin.Forms;

namespace ULFG.Forms.Network.Views
{
    /// <summary>
    /// <see cref="ContentPage"/> que representa la vista de la página con la lista de bloqueados
    /// </summary>
    public class BlockListView : ContentPage
	{
        readonly Func<Task> refresh;

        /// <summary>
        /// <see cref="ContentPage"/> que representa la vista de la página con la lista de seguidores
        /// </summary>
        public Func<Task> Refresh
        {
            get { return refresh; }
        }

        public BlockListView()
        {
            Title = "Bloqueados";
            Icon = DependencyService.Get<IResourceManager>().GetResourcesPath("block.png");

            BindingContext = new BlockListViewModel(Navigation);

            refresh = ((BlockListViewModel)BindingContext).Refresh;

            Application.Current.Properties["userNavigation"] = Navigation;
            ListView list = new ListView(ListViewCachingStrategy.RecycleElement)
            {
                ItemTemplate = new DataTemplate(typeof(UserCell)),
                RowHeight = 80
            };
            list.SetBinding(ListView.ItemsSourceProperty, "Items", BindingMode.TwoWay);
            list.ItemTapped += async (sender, e) =>
            {
                await Navigation.PushAsync(new ExtProfileView((User)e.Item));
            };

            Content = list;
        }
    }
}