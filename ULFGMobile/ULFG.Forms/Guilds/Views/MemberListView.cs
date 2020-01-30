using ULFG.Core.Data.Item;
using ULFG.Forms.Behaviors.Events.ArgsConverters;
using ULFG.Forms.Behaviors.Events.ViewsEventsBehaviors;
using ULFG.Forms.Guilds.ViewModels;
using ULFG.Forms.PlatformInterfaces;
using ULFG.Forms.Profiles.Views;
using Xamarin.Forms;

namespace ULFG.Forms.Guilds.Views
{
    /// <summary>
    /// <see cref="ContentPage"/> que representa la vista de la página con la lista de miembros de un gremio
    /// </summary>
    public class MemberListView :ContentPage
    {
        public MemberListView(Guild guild)
        {
            Padding = new Thickness(0, 15, 0, 0);

            BindingContext = new MemberListViewModel(Navigation, guild);

            Title = "Miembros";
            Icon = DependencyService.Get<IResourceManager>().GetResourcesPath("group.png");
            ListView MemberList = new ListView(ListViewCachingStrategy.RecycleElement)
            {
                ItemTemplate = new DataTemplate((typeof(UserCell))),
                SeparatorVisibility = SeparatorVisibility.None,
                RowHeight = 80
            };

            MemberList.SetBinding(ListView.ItemsSourceProperty, "Items");
            var tapped = new ListViewEventBehavior()
            {
                ListEventName = "ItemTapped",
                ListConverter = new ItemTappedEventArgsConverter()
            };
            tapped.SetBinding(ListViewEventBehavior.CommandProperty, "ItemTapped");
            MemberList.Behaviors.Add(tapped);

            Content = MemberList;
        }
    }
}
