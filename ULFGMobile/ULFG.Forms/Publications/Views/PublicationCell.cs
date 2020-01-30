using FFImageLoading.Forms;
using System.IO;
using System.Threading.Tasks;
using ULFG.Core.Data.Item;
using ULFG.Core.Data.ItemManager.Impl;
using ULFG.Forms.Shared;
using Xamarin.Forms;

namespace ULFG.Forms.Publications.Views
{
    /// <summary>
    /// <see cref="UserViewCellBase"/> de una publicación
    /// </summary>
    public class PublicationCell : UserViewCellBase
    {
        /// <summary>
        /// Usuario actual
        /// </summary>
        readonly User actualUser = (User)Application.Current.Properties["user"];

        /// <summary>
        /// Label con el texto de la publicacion que cambia con el binding context
        /// </summary>
        readonly Label lblText;

        /// <summary>
        /// Acción de menu para borrar una publicación que cambia con el binding context
        /// </summary>
        readonly MenuItem delete;

        /// <summary>
        /// Imagen adjunta de la publicación (si tiene) que cambia con el binding context. En caso de no tenerla este campo
        /// será null.
        /// </summary>
        readonly CachedImage cachedImage;

        /// <summary>
        /// Id del usuario creador de la publicación que cambia con el binding context
        /// </summary>
        string userid = "";
        /// <summary>
        /// Array de bytes con imagen adjunta de la publicación (si tiene) que cambia con el binding context
        /// </summary>
        byte[] Attach;

        /// <summary>
        /// Crea la estructura de componentes de la Cell
        /// </summary>
        public PublicationCell() : base()
        {
            lblText = new Label
            {
                FontSize = 15,
                TextColor = Color.FromHex("212121"),
                HorizontalOptions = LayoutOptions.StartAndExpand,
                LineBreakMode = LineBreakMode.WordWrap
            };

            Label phantom1 = new Label
            {
                HeightRequest = 10
            };
            Label phantom2 = new Label
            {
                HeightRequest = 10
            };

            cachedImage = new CachedImage()
            {
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                DownsampleHeight = 400,
                BitmapOptimizations = true
            };

            var imageTap = new TapGestureRecognizer();
            imageTap.Tapped += async (sender, e) =>
            {
                if(Attach != null)
                    await nav.PushAsync(new ImageDetail(Attach));
            };
            cachedImage.GestureRecognizers.Add(imageTap);
            lblText.SetBinding(Label.TextProperty, "Text");

            SharedRelative view = new SharedRelative(userIcon, lblNick, lblSeparator, lblName);
            view.VerticalOptions = LayoutOptions.StartAndExpand;

            view.Children.Add(lblText,
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
                    return parent.Width - 90;
                }),Constraint.Constant(55));

                view.Children.Add(cachedImage,
                    Constraint.RelativeToView(lblText, (parent, sibling) =>
                    {
                        return sibling.X;
                    }),
                    Constraint.RelativeToView(lblText, (parent, sibling) =>
                    {
                        return sibling.Y + sibling.Height + 10;
                    }),
                    Constraint.RelativeToView(userIcon, (parent, sibling) =>
                    {
                        return parent.Width - userIcon.Width - 40;
                    }));

            view.Children.Add(phantom1,
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
               }));

                view.Children.Add(phantom2,
                   Constraint.RelativeToView(cachedImage, (parent, sibling) =>
                   {
                       return sibling.X;
                   }),
                   Constraint.RelativeToView(cachedImage, (parent, sibling) =>
                   {
                       return sibling.Y + sibling.Height;
                   }),
                  Constraint.RelativeToView(userIcon, (parent, sibling) =>
                  {
                      return sibling.Width;
                  }));
                View = view;
            //Crea las acciones de contexto
            delete = new MenuItem() { Text = "Borrar" };
            delete.SetBinding(MenuItem.CommandParameterProperty, new Binding("."));
            delete.Clicked +=  (sender, e) =>
            {
                Publication publi = (Publication)(((MenuItem)sender).CommandParameter);
                Task.Run(() => PublicationManager.DefaultManager.DeletePubAsync(publi));
                MessagingCenter.Send<object, string>(this, ULFG.Forms.App.PublicationChangedKey, "Deleted " + publi.Id);
            };           
        }

        /// <summary>
        /// Actualiza los componentes de la Cell cuando cambia el binding context
        /// </summary>
        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();
            var publi = BindingContext as Publication;
            if (publi != null)
            {
                var user = Task.Run(async () => { return await UserManager.DefaultManager.GetUserByIdAsync(publi.AutorId); }).Result;
                lblText.Text = publi.Text;
                lblName.Text = user.Username;
                lblNick.Text = user.Nickname;
                userid = user.Id;
                userIcon.Source = ImageSource.FromStream(() => new MemoryStream(user.Image));
                if (publi.Attachment != null)
                {
                    cachedImage.Source = ImageSource.FromStream(() => new MemoryStream(publi.Attachment));
                    Attach = publi.Attachment;
                }
                else
                {
                    cachedImage.Source = null;
                    Attach = null;
                }
                if (userid.Equals(actualUser.Id))
                    ContextActions.Add(delete);
                else if (ContextActions.Contains(delete))
                    ContextActions.Remove(delete);
            }
        }
    }
}
