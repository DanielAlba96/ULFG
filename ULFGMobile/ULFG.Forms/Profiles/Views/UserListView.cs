using ULFG.Core.Data.Item;
using ULFG.Forms.PlatformInterfaces;
using ULFG.Forms.PrivateChat.Views;
using ULFG.Forms.Profiles.ViewModels;
using Xamarin.Forms;

namespace ULFG.Forms.Profiles.Views
{
    /// <summary>
    /// <see cref="ContentPage"/> que representa la vista de la página de la lista de usuarios
    /// </summary>
    public class UserListView : ContentPage
    {
        public UserListView(bool onlyMsg = false)
        {
            Padding = new Thickness(0, 15, 0, 0);
            BindingContext = new UserListViewModel(Navigation);
            Icon = DependencyService.Get<IResourceManager>().GetResourcesPath("search.png");

            SearchBar search = new SearchBar() { Placeholder = "Buscar por apodo y nombre de usuario", HeightRequest = 35};
            search.SetBinding(SearchBar.TextProperty, "SearchText");
            search.SetBinding(SearchBar.SearchCommandProperty, "Search");
            search.TextChanged += (sender, e) =>
            {
                 ((UserListViewModel)BindingContext).SearchTextChanged(sender, e);
            };
            Application.Current.Properties["userNavigation"] = Navigation;
            ListView list = new ListView(ListViewCachingStrategy.RecycleElement)
            {
                RowHeight = 80,
                ItemTemplate = new DataTemplate((typeof(UserCell)))
            };
            if (!onlyMsg)
            {
                Title = "Usuarios";
                list.ItemTapped += async (sender, e) =>
                {
                    await Navigation.PushAsync(new ExtProfileView((User)e.Item));
                };
            }
            else
            {
                Title = "Selecciona el destinatario";               
                list.ItemTapped += async (sender, e) =>
                {
                    await Navigation.PushAsync(new SendMessageView((User)e.Item));
                };
            }
            list.SetBinding(ListView.ItemsSourceProperty, "Items");

            StackLayout content = new StackLayout()
            {
                Children = { search, list }
            };

            Content = content;
        }
    }
}
