using System.Linq;
using ULFG.Forms.Behaviors.Validators;
using Xamarin.Forms;

namespace ULFG.Forms.Shared
{
    /// <summary>
    /// <see cref="ContentPage"/> que representa la vista de la página de chat, tanto individual como grupal
    /// </summary>
    public class ChatView : ContentPage
    {
        /// <summary>
        /// Lista de mensajes del chat
        /// </summary>
        protected ListView messages;

        public ChatView()
        {
            messages = new ListView()
            {
                HasUnevenRows = true,
                ItemTemplate = new DataTemplate(typeof(MessageCell)),
                SeparatorVisibility = SeparatorVisibility.None
            };
            messages.SetBinding(ListView.ItemsSourceProperty, "Items");

            Editor msgEntry = new Editor
            {
                HeightRequest = 120
            };
            msgEntry.SetBinding(Editor.TextProperty, "Msg");
            msgEntry.Behaviors.Add(new EntryLengthValidatorBehaviorMultiLine() { MaxLength = 240 });

            Button btnSend = new Button
            {
                Text = "Enviar"
            };
            btnSend.SetBinding(Button.CommandProperty, "Send");

            StackLayout stack = new StackLayout
            {
                Children =
                {
                    messages,
                    msgEntry,
                    btnSend
                }
            };
            Content = stack;
        }

        /// <summary>
        /// Escucha la llegada de nuevos mensajes por <see cref="MessagingCenter"/> y hace scroll
        /// a la lista cuando llega uno nuevo
        /// </summary>
        protected override void OnAppearing()
        {
            base.OnAppearing();
            var lastMessage = messages.ItemsSource.Cast<object>().LastOrDefault();
            messages.ScrollTo(lastMessage, ScrollToPosition.End, false);

            MessagingCenter.Subscribe<object, string>(this, ULFG.Forms.App.NewMessageScrollKey, (sender, e) =>
            {
                    lastMessage = messages.ItemsSource.Cast<object>().LastOrDefault();
                    messages.ScrollTo(lastMessage, ScrollToPosition.End, false);
            });
        }

        /// <summary>
        /// Deja de escuchar nuevos mensajes por <see cref="MessagingCenter"/>
        /// </summary>
        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            MessagingCenter.Unsubscribe<object, string>(this, ULFG.Forms.App.NewMessageScrollKey);
        }
    }
}
