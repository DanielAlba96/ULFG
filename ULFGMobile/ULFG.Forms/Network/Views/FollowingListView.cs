using ULFG.Core.Data.Item;
using Xamarin.Forms;
using ULFG.Forms.Profiles.Views;
using ULFG.Forms.Network.ViewModels;
using System;
using ULFG.Forms.PlatformInterfaces;
using System.Threading.Tasks;

namespace ULFG.Forms.Network.Views
{
    /// <summary>
    /// <see cref="ContentPage"/> que representa la vista de la página con la lista de seguidores
    /// </summary>
    public class FollowingListView : ContentPage
    {       
        readonly Func<Task> refresh;

        /// <summary>
        /// Función para actualizar la página
        /// </summary>
        public Func<Task> Refresh
        {
            get { return refresh; }
        }

        public FollowingListView()
        {
            Title = "Seguidores";
            Icon = DependencyService.Get<IResourceManager>().GetResourcesPath("group.png");

            BindingContext = new FollowingListViewModel(Navigation);

            refresh = ((FollowingListViewModel)BindingContext).Refresh;

            Application.Current.Properties["userNavigation"] = Navigation;
            ListView list = new ListView(ListViewCachingStrategy.RecycleElement)
            {
                ItemTemplate = new DataTemplate(typeof(UserCell)),
                RowHeight = 80
            };
            list.ItemTapped += async (sender, e) =>
            {
                await Navigation.PushAsync(new ExtProfileView((User)e.Item));
            };
            list.SetBinding(ListView.ItemsSourceProperty, "Items");

            Content = list;
        }
    }
}