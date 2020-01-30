using Xamarin.Forms;
using ULFG.Core.Data.Item;
using ULFG.Forms.PrivateChat.ViewModels;
using ULFG.Forms.PlatformInterfaces;
using ULFG.Forms.Behaviors.Validators;

namespace ULFG.Forms.PrivateChat.Views
{
    /// <summary>
    /// <see cref="ContentPage"/> que representa la vista de la página de enviar mensaje
    /// </summary>
    public class SendMessageView : ContentPage
    {
        public SendMessageView(User receiver)
        {
            BindingContext = new SendMessageViewModel(Navigation, receiver);

            Title = "Enviar mensaje a " + receiver.Username;

            ToolbarItem send = new ToolbarItem()
            {
                Text = "Enviar mensaje",
                Order = ToolbarItemOrder.Primary,
                Icon = DependencyService.Get<IResourceManager>().GetResourcesPath("send.png")
            };
            send.SetBinding(ToolbarItem.CommandProperty, "Send");
            ToolbarItems.Add(send);

            Label lblTo = new Label
            {
                Text = "Escribe aqui tu mensaje"
            };

            Editor msgEntry = new Editor() { HeightRequest = 120, BackgroundColor = Color.White };
            msgEntry.SetBinding(Editor.TextProperty, "Message");
            msgEntry.Behaviors.Add(new EntryLengthValidatorBehaviorMultiLine() { MaxLength = 240 });

            StackLayout stack = new StackLayout
            {
                Children ={
                    lblTo,
                    msgEntry
                }
            };

            Content = stack;
        }
    }
}