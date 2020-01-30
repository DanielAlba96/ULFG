using FFImageLoading.Forms;
using System.IO;
using ULFG.Core.Data.Item;
using ULFG.Forms.PrivateChat.Views;
using ULFG.Forms.Shared;
using Xamarin.Forms;

namespace ULFG.Forms.Profiles.Views
{
    /// <summary>
    /// <see cref="UserViewCellBase"/> de un usuario
    /// </summary>
    public class UserCell : UserViewCellBase
    {
        readonly User actualUser = (User)Application.Current.Properties["user"];
        /// <summary>
        /// Label con la biografía del usuario que se modifica cuando cambia el binding context
        /// </summary>
        readonly Label lblBio;

        /// <summary>
        /// Crea la estructura de componentes de la Cell
        /// </summary>
        public UserCell() : base()
        {     
            lblBio = new Label
            {
                FontSize = 15,
                TextColor = Color.FromHex("212121"),
                HorizontalOptions = LayoutOptions.StartAndExpand,
                LineBreakMode = LineBreakMode.WordWrap
            };

            Label space = new Label();

             userIcon = new CachedImage
             {
                BitmapOptimizations = true
             };

            var sendMsg = new MenuItem { Text = "Enviar mensaje" };
            sendMsg.SetBinding(MenuItem.CommandParameterProperty, new Binding("."));
            sendMsg.Clicked += async (sender, e) => {
                var mi = ((MenuItem)sender);
                await nav.PushAsync(new SendMessageView((User)mi.CommandParameter));
            };
            ContextActions.Add(sendMsg);

            SharedRelative view = new SharedRelative(userIcon, lblNick, lblSeparator, lblName);

            view.Children.Add(lblBio,
                Constraint.RelativeToView(lblNick, (parent, sibling) =>
                {
                    return sibling.X;
                }),
                Constraint.RelativeToView(lblNick, (parent, sibling) =>
                {
                    return sibling.Y + sibling.Height;
                }),
                Constraint.RelativeToParent((parent)=>
                {
                    return parent.Width - 80;
                }),
                Constraint.Constant(80));
            view.Children.Add(space,
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

            if (!(BindingContext is User user))
                return;
            lblName.Text = user.Username + ((user.Id.Equals(actualUser.Id)) ? " (Tú)" : "");
            userIcon.Source = ImageSource.FromStream(() => new MemoryStream(user.Image));
            lblBio.Text = user.Bio;
            lblNick.Text = user.Nickname;
        }
    }
}
