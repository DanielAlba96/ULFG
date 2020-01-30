using FFImageLoading.Forms;
using ULFG.Forms.Behaviors.Validators;
using ULFG.Forms.Publications.ViewModels;
using Xamarin.Forms;

namespace ULFG.Forms.Publications.Views
{
    /// <summary>
    /// <see cref="ContentPage"/> que representa la vista de la pantalla de crear publicación
    /// </summary>
    public class NewPublicationView : ContentPage
    {
        public NewPublicationView()
        {
            BindingContext = new NewPublicationViewModel(Navigation);

            Title = "Crear nueva publicacion";

            Label lblMessage = new Label
            {
                Text = "Mensaje",
                FontSize = 16,
                FontAttributes = FontAttributes.Bold,
                HorizontalOptions = LayoutOptions.Center
            };

            Editor msgEntry = new Editor() { HeightRequest = 80};
            msgEntry.SetBinding(Editor.TextProperty, "Message");
            msgEntry.Behaviors.Add(new EntryLengthValidatorBehaviorMultiLine() { MaxLength = 110 });

            CachedImage img = new CachedImage() { Aspect = Aspect.AspectFit, DownsampleHeight = 400 };
            img.SetBinding(CachedImage.SourceProperty, "Source");

            var tapGestureRecognizer = new TapGestureRecognizer();
            tapGestureRecognizer.SetBinding(TapGestureRecognizer.CommandProperty, "ImageTapped");
            img.GestureRecognizers.Add(tapGestureRecognizer);

            Button btnAttachment = new Button { Text = "Adjuntar archivo", FontSize = 14, WidthRequest = 150, HorizontalOptions = LayoutOptions.Center };
            btnAttachment.SetBinding(Button.CommandProperty, "CreateAttach");

            Button btnAccept = new Button() { Text = "Publicar", FontSize = 14, WidthRequest = 150, HorizontalOptions = LayoutOptions.Center };
            btnAccept.SetBinding(Button.CommandProperty, "SavePublication");

            StackLayout content = new StackLayout
            {
                Children =
                {
                   lblMessage,
                   msgEntry,
                   img,
                   btnAttachment,
                   btnAccept
               }
            };
            Content = new ScrollView() { Content = content, Margin = 20 };
        }
    }
}
