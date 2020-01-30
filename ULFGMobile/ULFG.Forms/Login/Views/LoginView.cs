using FFImageLoading.Forms;
using ULFG.Forms.Behaviors.Validators;
using ULFG.Forms.Login.ViewModels;
using ULFG.Forms.PlatformInterfaces;
using Xamarin.Forms;

namespace ULFG.Forms.Login.Views
{
    /// <summary>
    /// <see cref="ContentPage"/> que representa la vista de la página de login
    /// </summary>
    public class LoginView : ContentPage
    {
        public LoginView()
        {
            BindingContext = new LoginViewModel(Navigation);

            RelativeLayout relativeLayout = new RelativeLayout();

            NavigationPage.SetHasNavigationBar(this, false);

            var path = DependencyService.Get<IResourceManager>().GetResourcesPath("ulfg.png");

            CachedImage icon = new CachedImage()
            {
                Source = path,
                HeightRequest = 130,
                WidthRequest = 130,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions=LayoutOptions.End
            };

            Label blank = new Label { HeightRequest = 5, Opacity = 0 };
            Label blank2 = new Label { HeightRequest = 10, Opacity = 0 };

            Label lblName = new Label
            {
                Text = "Nombre de usuario",
                WidthRequest = 200,
                HorizontalOptions = LayoutOptions.Center
            };

            Label lblPass = new Label
            {
                Text = "Contraseña",
                WidthRequest = 200,
                HorizontalOptions = LayoutOptions.Center
            };

            Entry nameEntry = new Entry
            {
                FontSize = 16,
                WidthRequest=200,
                HorizontalOptions = LayoutOptions.Center
            };
            nameEntry.SetBinding(Entry.TextProperty, "Username");
            nameEntry.Behaviors.Add(new EntryLengthValidatorBehavior() { MaxLength = 16 });

            Entry passEntry = new Entry
            {
                FontSize = 16,
                IsPassword = true,
                WidthRequest = 200,
                HorizontalOptions=LayoutOptions.Center
            };
            passEntry.SetBinding(Entry.TextProperty, "Password");
            passEntry.Behaviors.Add(new EntryLengthValidatorBehavior() { MaxLength = 16 });

            Button btnLogin = new Button
            {
                Text = "Iniciar Sesión",
                FontSize = 14,
                HorizontalOptions = LayoutOptions.Center,
                WidthRequest = 150
            };
            btnLogin.SetBinding(Button.CommandProperty, "Login");

            Button btnRegister = new Button
            {
                Text = "Registrarse",
                FontSize = 14,
                HorizontalOptions=LayoutOptions.Center,
                WidthRequest = 150
            };
            btnRegister.SetBinding(Button.CommandProperty, "Register");

            StackLayout content = new StackLayout()
            {
                Children =
                {
                  icon,
                  blank,
                  lblName,
                  nameEntry,
                  lblPass,
                  passEntry,
                  blank2,
                  btnLogin
                }
            };

            if (Device.RuntimePlatform != Device.UWP)
                content.Children.Add(btnRegister);

            Content = new ScrollView() { Content = content, VerticalOptions = LayoutOptions.Center };
        }
    }
}