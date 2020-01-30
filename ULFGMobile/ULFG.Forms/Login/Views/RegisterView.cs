using ULFG.Forms.Behaviors.Validators;
using ULFG.Forms.Login.ViewModels;
using Xamarin.Forms;

namespace ULFG.Forms.Login.Views
{
    /// <summary>
    /// <see cref="ContentPage"/> que representa la vista de la página de registro
    /// </summary>
    public class RegisterView : ContentPage
    {
        public RegisterView()
        {
            BindingContext = new RegisterViewModel(Navigation);

            Title = "Registro";

            Label lblName = new Label
            {
                Text = "Nombre de usuario(*)"
            };

            Label lblNick = new Label
            {
                Text = "Apodo(*)"
            };
            Label lblEmail = new Label
            {
                Text = "Email(*)"
            };

            Label lblPass = new Label
            {
                Text = "Contraseña(*)"
            };

            Label lblPassRepeat = new Label
            {
                Text = "Repite contraseña(*)"
            };

            Entry nameEntry = new Entry
            {
                FontSize = 16
            };
            nameEntry.SetBinding(Entry.TextProperty, "Username");
            nameEntry.Behaviors.Add(new EntryLengthValidatorBehavior() { MaxLength = 16});

            Entry nickEntry = new Entry
            {
                FontSize = 16
            };
            nickEntry.SetBinding(Entry.TextProperty, "Nickname");
            nickEntry.Behaviors.Add(new EntryLengthValidatorBehavior() { MaxLength = 16 });

            Entry emailEntry = new Entry
            {
                FontSize = 16,
                Placeholder="usuario@ulfg.es"
            };
            emailEntry.SetBinding(Entry.TextProperty, "Email");
            emailEntry.Behaviors.Add(new EntryLengthValidatorBehavior() { MaxLength = 24 });

            Entry passEntry = new Entry
            {
                FontSize = 16,
                IsPassword = true
            };
            passEntry.SetBinding(Entry.TextProperty, "Pass");
            passEntry.Behaviors.Add(new EntryLengthValidatorBehavior() { MaxLength = 16 });

            Entry passEntryRepeat = new Entry
            {
                FontSize = 16,
                IsPassword = true
            };
            passEntryRepeat.SetBinding(Entry.TextProperty, "RepeatPass");
            passEntryRepeat.Behaviors.Add(new EntryLengthValidatorBehavior() { MaxLength = 16 });

            Button btnRegister = new Button
            {
                Text = "Registrar",
                FontSize = 16,
                HorizontalOptions=LayoutOptions.Center,
                Margin = 20,
                WidthRequest = 120
            };
            btnRegister.SetBinding(Button.CommandProperty, "Register");

            StackLayout content = new StackLayout()
            {
                Children =
                {
                   lblName,
                   nameEntry,
                   lblNick,
                   nickEntry,
                   lblEmail,
                   emailEntry,
                   lblPass,
                   passEntry,
                   lblPassRepeat,
                   passEntryRepeat,
                   btnRegister
                }
            };

            Content = new ScrollView() { Content = content, Margin = 20 };
        }
    }
}
