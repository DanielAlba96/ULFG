using System;
using System.Threading.Tasks;
using ULFG.Core.Data.Item;
using ULFG.Core.Data.ItemManager.Impl;
using Xamarin.Forms;

namespace ULFG.Forms.Shared
{
    /// <summary>
    /// <see cref="ViewCell"/> de los mensajes de chat, tanto individuales como grupales
    /// </summary>
    public class MessageCell : ViewCell
    {
        public MessageCell()
        {

        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();

            User user = (User)Application.Current.Properties["user"];

            if (!(BindingContext is Message msg))
                return;

            Label texto = new Label();
            texto.SetBinding(Label.TextProperty, "Text");
            Label lblCreator = new Label()
            {
                FontAttributes = FontAttributes.Bold
            };
            var creator = Task.Run(async () => { return await UserManager.DefaultManager.GetUserByIdAsync(msg.Creator_Id); }).Result;
            var date = msg.CreationDate;
            var dateString = String.Format("{0:dd-MM-yyyy}", date);
            lblCreator.Text = creator.Nickname;
            Label lblDate = new Label()
            {
                FontSize = 10,
                HorizontalOptions = LayoutOptions.EndAndExpand
            };

            if (date.Day.Equals(DateTime.Now.Day)) //Si el mensaje es de hoy se muestra la hora

                lblDate.Text = String.Format("{0:HH:mm}", date);
            else //Si es de otro dia se muestra la fecha
                lblDate.Text = String.Format("{0:dd-MM-yyyy}", date);

            StackLayout stack1 = new StackLayout()
            {
                Children =
                {
                    lblCreator,
                    lblDate
                },
                Orientation = StackOrientation.Horizontal
            };

            StackLayout stack2 = new StackLayout()
            {
                Children = {
                stack1,
                texto
                }
            };

            ContentView content = new ContentView
            {
                BackgroundColor = Color.Aquamarine,
                Padding = new Thickness(15),
                Content = stack2,
                Margin = 20,
            };

            Label phantom = new Label
            {
                WidthRequest = 100,
                Opacity = 0
            };

            var actualId = user.Id;

            StackLayout stack;

            //La combinación de colores del mensaje cambiará en función de si es propio o de otro usuario
            if (actualId.Equals(msg.Creator_Id)) 
            {
                content.HorizontalOptions = LayoutOptions.EndAndExpand;

                stack = new StackLayout
                {
                    Children = {
                phantom,
                content
                },
                    Orientation = StackOrientation.Horizontal
                };
                lblCreator.TextColor = Color.FromHex("424242");
                lblDate.TextColor = Color.FromHex("757575");
                texto.TextColor = Color.FromHex("424242");
                content.BackgroundColor = Color.FromHex("FFBD59");
            }
            else
            {
                stack = new StackLayout
                {
                    Children = {
                content,
                phantom
                },
                    Orientation = StackOrientation.Horizontal
                };
                lblCreator.TextColor = Color.FromHex("DB8D28");
                lblDate.TextColor = Color.FromHex("FFBD59");
                texto.TextColor = Color.FromHex("DB8D28");
                content.BackgroundColor = Color.FromHex("424242");
            }
            View = stack;
        }
    }
}
