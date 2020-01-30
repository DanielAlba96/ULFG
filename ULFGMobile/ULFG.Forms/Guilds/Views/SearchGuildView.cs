using ULFG.Forms.Guilds.ViewModels;
using ULFG.Forms.PlatformInterfaces;
using Xamarin.Forms;

namespace ULFG.Forms.Guilds.Views
{
    /// <summary>
    /// <see cref="ContentPage"/> que representa la vista de la página con la lista de todos los gremios
    /// </summary>
    /// <remarks>Incluye un filtro por nombre y descripción</remarks>
    public class SearchGuildView : ContentPage
    {

        public SearchGuildView()
        {
            Padding = new Thickness(0, 20, 0, 0);

            BindingContext = new SearchGuildViewModel(Navigation);

            Title = "Busqueda";
            Icon = DependencyService.Get<IResourceManager>().GetResourcesPath("search.png");

            SearchBar SearchBar = new SearchBar()
            {
                Placeholder = "Buscar por nombre y descripcion",
                HeightRequest = 35
            };
            SearchBar.TextChanged += (sender, e) =>
            {
                ((SearchGuildViewModel)BindingContext).SearchTextChanged(sender, e);
            };
            SearchBar.SetBinding(SearchBar.TextProperty, "SearchText");
            SearchBar.SetBinding(SearchBar.SearchCommandProperty, "Search");

            ListView List = new ListView(ListViewCachingStrategy.RecycleElement)
            {
                RowHeight = 100,
                ItemTemplate = new DataTemplate(typeof(GuildCell))
            };
            List.SetBinding(ListView.ItemsSourceProperty, "Items");
            List.ItemTapped += async (sender, e) =>
            {
                await ((SearchGuildViewModel)BindingContext).ItemTapped(e);
            };
           
            StackLayout stack = new StackLayout
            {
                Children ={
                    SearchBar,
                    List
                }
            };
            Content = stack;
        }
    }
}