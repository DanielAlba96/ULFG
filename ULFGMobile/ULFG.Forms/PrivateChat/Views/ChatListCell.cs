using Plugin.Connectivity;
using System;
using System.IO;
using System.Threading.Tasks;
using ULFG.Core.Data.Item;
using ULFG.Core.Data.ItemManager.Impl;
using ULFG.Core.Logic;
using ULFG.Forms.Profiles.Views;
using ULFG.Forms.Shared;
using Xamarin.Forms;

namespace ULFG.Forms.PrivateChat.Views
{
    /// <summary>
    /// <see cref="UserViewCellBase"/> que representa un elemento de la lista de chats
    /// </summary>
    public class ChatListCell : UserViewCellBase
    {
        /// <summary>
        /// Usuario actual
        /// </summary>
        readonly User actualUser = (User)Application.Current.Properties["user"];

        /// <summary>
        /// Acceso a la lógica de mensajería
        /// </summary>
        readonly MessageOperations op = new MessageOperations();

        /// <summary>
        /// Labels con datos del último mensaje del chat que se modifican cuando cambia el binding context
        /// </summary>
        readonly Label lblLast, lblLastDate;

        /// <summary>
        /// Crea la estructura de componentes de la Cell
        /// </summary>
        public ChatListCell() : base()
        {           
            lblLast = new Label
            {
                FontSize = 15,
                TextColor = Color.FromHex("424242"),
                HorizontalOptions = LayoutOptions.StartAndExpand,
                LineBreakMode = LineBreakMode.WordWrap
            };

            lblLastDate = new Label
            {
                TextColor = Color.FromHex("616161"),
                FontSize = 14
            };

            Label phantom = new Label();

            var profile = new MenuItem { Text = "Ver Perfil" };
            profile.SetBinding(MenuItem.CommandParameterProperty, new Binding("."));
            profile.Clicked += async (sender, e) =>
            {
                var mi = ((MenuItem)sender);
                await nav.PushAsync(new ExtProfileView((User)mi.CommandParameter));
            };

            var leave = new MenuItem { Text = "Abandonar" };
            leave.SetBinding(MenuItem.CommandParameterProperty, new Binding("."));
            leave.Clicked += async (sender, e) =>
            {
                var mi = ((MenuItem)sender);
                await op.DeleteChat(((Chat)mi.CommandParameter).Id, actualUser.Id);
                MessagingCenter.Send<object, string>(this, ULFG.Forms.App.ChatDeletedKey, ((Chat)mi.CommandParameter).Id);
            };

            ContextActions.Add(profile);
            ContextActions.Add(leave);

            SharedRelative view = new SharedRelative(userIcon, lblNick, lblSeparator, lblName);

            view.Children.Add(lblLastDate,
                Constraint.RelativeToView(lblNick, (parent, sibling) =>
                {
                    return sibling.X;
                }),
                Constraint.RelativeToView(lblName, (parent, sibling) =>
                {
                    return sibling.Y + sibling.Height;
                }));

            view.Children.Add(lblLast,
                Constraint.RelativeToView(lblLastDate, (parent, sibling) =>
                {
                    return sibling.X;
                }),
                Constraint.RelativeToView(lblLastDate, (parent, sibling) =>
                {
                    return sibling.Y + sibling.Height;
                }),
                Constraint.RelativeToParent((parent) =>
                {
                    return parent.Width - 80;
                }));
            view.Children.Add(phantom,
                Constraint.RelativeToView(userIcon, (parent, sibling) =>
                {
                    return sibling.X;
                }),
                Constraint.RelativeToView(userIcon, (parent, sibling) =>
                {
                    return sibling.Y + sibling.Height;
                }),
               Constraint.RelativeToView(userIcon, (parent, sibling) =>
               {
                   return sibling.Width;
               }),
               Constraint.Constant(10));

            View = view;
        }

        /// <summary>
        /// Actualiza los componentes de la Cell cuando cambia el binding context
        /// </summary>
        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();
            if (!(BindingContext is Chat chat))
                return;
            string otherId;
            if (chat.Member1_id.Equals(actualUser.Id))
                otherId = chat.Member2_id;
            else
                otherId = chat.Member1_id;

            User other = Task.Run(() => { return UserManager.DefaultManager.GetUserByIdAsync(otherId, CrossConnectivity.Current.IsConnected); }).Result;
            lblName.Text = other.Username;
            lblNick.Text = other.Nickname;
            userIcon.Source = ImageSource.FromStream(() => new MemoryStream(other.Image));
            var last = Task.Run(() => { return MessageManager.DefaultManager.GetLastMessageOfChatAsync(actualUser.Id, chat.Id, CrossConnectivity.Current.IsConnected); }).Result;
            if (last != null)
            {
                lblLast.Text = last.Text;
                var date = last.CreationDate;
                if (date.Day.Equals(DateTime.Now.Day))

                    lblLastDate.Text = String.Format("{0:HH:mm}", date);
                else

                    lblLastDate.Text = String.Format("{0:dd-MM-yyyy}", date);
            }
        }
    }
}