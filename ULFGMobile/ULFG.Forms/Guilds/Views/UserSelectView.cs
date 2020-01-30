using ULFG.Core.Data.Item;
using Xamarin.Forms;
using ULFG.Forms.Profiles.Views;
using ULFG.Forms.Guilds.ViewModels;
using ULFG.Forms.Behaviors.Events.ArgsConverters;
using ULFG.Forms.Behaviors.Events.ViewsEventsBehaviors;

namespace ULFG.Forms.Guilds.Views
{
    /// <summary>
    /// <see cref="ContentPage"/> que representa la vista de la página de selección de usuarios a invitar
    /// </summary>
    public class UserSelectView : ContentPage
	{
        public UserSelectView(Guild guild)
        {
            Padding = new Thickness(0, 10, 0, 0);

            BindingContext = new UserSelectViewModel(Navigation, guild);

            Title = "Selecciona usuario a invitar";

            Application.Current.Properties["userNavigation"] = Navigation;
            ListView list = new ListView(ListViewCachingStrategy.RecycleElement)
            {
                ItemTemplate = new DataTemplate(typeof(UserCell)),
                RowHeight = 80,
                SeparatorVisibility = SeparatorVisibility.None
            };
            list.SetBinding(ListView.ItemsSourceProperty, "Items");
            var tap = new ListViewEventBehavior()
            {
                ListEventName = "ItemTapped",
                ListConverter = new ItemTappedEventArgsConverter()
            };
            tap.SetBinding(ListViewEventBehavior.CommandProperty, "Tapped");
            list.Behaviors.Add(tap);

            Content = list;
		}
	}
}