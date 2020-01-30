using Xamarin.Forms;
using ULFG.Core.Data.Item;
using ULFG.Forms.PrivateChat.ViewModels;
using ULFG.Forms.PlatformInterfaces;

namespace ULFG.Forms.PrivateChat.Views
{
    /// <summary>
    /// <see cref="ContentPage"/> que representa la vista de la página con la lista de chats activos
    /// </summary>
    public class ChatListView : ContentPage
	{
        public ChatListView()
		{
            Padding = new Thickness(0, 15, 0, 0);

            BindingContext = new ChatListViewModel(Navigation);

            Title = "Mensajes";
            Icon = DependencyService.Get<IResourceManager>().GetResourcesPath("inbox.png");

            ToolbarItem newMsg = new ToolbarItem()
            {
                Text = "Nuevo mensaje",
                Icon = DependencyService.Get<IResourceManager>().GetResourcesPath("newMessage.png")
            };
            newMsg.SetBinding(ToolbarItem.CommandProperty, "New");
            ToolbarItems.Add(newMsg);

            ListView list = new ListView()
            {
                IsPullToRefreshEnabled = true,
                RowHeight = 75,
                ItemTemplate = new DataTemplate(typeof(ChatListCell))
            };
            list.SetBinding(ListView.RefreshCommandProperty, "Refresh");
            list.SetBinding(ListView.IsRefreshingProperty, "IsRefreshing");
            list.SetBinding(ListView.ItemsSourceProperty, "Items");
            list.ItemTapped += async (sender,e) =>{ 
                await Navigation.PushAsync(new IndividualChatView((Chat) e.Item));
            };
            
            Content = list;
		}
    }
}