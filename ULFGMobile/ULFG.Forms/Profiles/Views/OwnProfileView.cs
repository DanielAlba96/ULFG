using Xamarin.Forms;
using ULFG.Forms.Profiles.ViewModels;
using FFImageLoading.Forms;
using ULFG.Core.Data.Item;
using ULFG.Forms.Behaviors.Validators;

namespace ULFG.Forms.Profiles.Views
{
    /// <summary>
    /// <see cref="ContentPage"/> que representa la vista de la página de edición del perfil
    /// </summary>
    public class OwnProfileView : ContentPage
    {
        public OwnProfileView()
        {
            User user = (User) Application.Current.Properties["user"];

            BindingContext = new OwnProfileViewModel(Navigation);

            Title = "Mi Perfil (" + user.Username + ")";

            CachedImage img = new CachedImage() { Aspect = Aspect.AspectFit, BitmapOptimizations = true, HeightRequest = 200};
            img.SetBinding(CachedImage.SourceProperty, "UserIcon");

            Button btnImage = new Button() { Text = "Cambiar imagen", HorizontalOptions = LayoutOptions.Center, WidthRequest = 160 };
            btnImage.SetBinding(Button.CommandProperty, "ChangeIcon");

            Label lblUserTitle = new Label()
            {
                Text = "Username:"
            };
            Label lblUser = new Label()
            {
                FontAttributes = FontAttributes.Bold,
                FontSize = 14
            };
            lblUser.SetBinding(Label.TextProperty, "Username");

            Label lblNick = new Label()
            {
                Text = "Apodo:"
            };
            Entry entryNick = new Entry()
            {
                IsEnabled = false
            };
            entryNick.SetBinding(Entry.TextProperty, "Nickname");
            entryNick.SetBinding(Entry.IsEnabledProperty, "IsNicknameActive");
            entryNick.Behaviors.Add(new EntryLengthValidatorBehavior() { MaxLength = 16 });

            Button btnNick = new Button() { HorizontalOptions = LayoutOptions.Center, WidthRequest = 110 };
            btnNick.SetBinding(Button.TextProperty, "BtnNickText");
            btnNick.SetBinding(Button.CommandProperty, "ChangeNick");

            Label lblBio = new Label()
            {
                Text = "Biografia:"
            };

            Button btnPass = new Button { Text = "Cambiar contraseña",  HorizontalOptions = LayoutOptions.Center, WidthRequest = 180 };
            btnPass.SetBinding(Button.CommandProperty, "ChangePass");

            Editor editorBio = new Editor()
            {
                IsEnabled = false,
                HeightRequest = 120
            };
            editorBio.SetBinding(Editor.TextProperty, "Bio");
            editorBio.SetBinding(Editor.IsEnabledProperty, "IsBioActive");
            editorBio.Behaviors.Add(new EntryLengthValidatorBehaviorMultiLine() { MaxLength = 240 });
            editorBio.SetBinding(Editor.BackgroundColorProperty, "BioBackground");

            Button btnBio = new Button() { HorizontalOptions = LayoutOptions.Center, WidthRequest = 110 };
            btnBio.SetBinding(Button.TextProperty, "BtnBioText");
            btnBio.SetBinding(Button.CommandProperty, "ChangeBio");

            Label lblEmail = new Label()
            {
                Text = "Email:"
            };

            Entry entryEmail = new Entry()
            {
                IsEnabled = false             
            };
            entryEmail.SetBinding(Entry.TextProperty, "Email");
            entryEmail.SetBinding(Entry.IsEnabledProperty, "IsEmailActive");
            entryEmail.Behaviors.Add(new EntryLengthValidatorBehavior() { MaxLength = 24 });

            Button btnEmail = new Button() { HorizontalOptions= LayoutOptions.Center, WidthRequest = 110 };
            btnEmail.SetBinding(Button.TextProperty, "BtnEmailText");
            btnEmail.SetBinding(Button.CommandProperty, "ChangeEmail");

            StackLayout stack = new StackLayout()
            {
                Children =
                {
                   img,
                   btnImage,
                   lblNick,
                   entryNick,
                   btnNick,
                   lblEmail,
                   entryEmail,
                   btnEmail,
                   lblBio,
                   editorBio,
                   btnBio,
                   btnPass
               }
            };
            Content = new ScrollView() { Content = stack };
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            BindingContext = new OwnProfileViewModel(Navigation);
        }
    }
}
