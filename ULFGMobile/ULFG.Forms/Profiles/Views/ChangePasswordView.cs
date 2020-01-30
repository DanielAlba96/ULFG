using ULFG.Forms.Behaviors.Validators;
using ULFG.Forms.Profiles.ViewModels;
using Xamarin.Forms;

namespace ULFG.Forms.Profiles.Views
{
    /// <summary>
    /// <see cref="ContentPage"/> que representa la vista de la página de cambio de contraseña
    /// </summary>
    public class ChangePasswordView : ContentPage
    {
        public ChangePasswordView()
        {
            BindingContext = new ChangePasswordViewModel(Navigation);
            Title = "Cambiar contraseña";

            Label lblOld = new Label()
            {
                Text = "Contraseña actual",
                FontSize = 16,
                FontAttributes = FontAttributes.Bold
            };

            Entry oldEntry = new Entry() { IsPassword = true, FontSize = 16 };
            oldEntry.SetBinding(Entry.TextProperty, "OldPass");
            oldEntry.Behaviors.Add(new EntryLengthValidatorBehavior() { MaxLength = 16 });

            Label lblNew = new Label()
            {
                Text = "Nueva contraseña",
                FontSize = 16,
                FontAttributes = FontAttributes.Bold
            };

            Entry newEntry = new Entry() { IsPassword = true, FontSize = 16 };
            newEntry.SetBinding(Entry.TextProperty, "NewPass");
            newEntry.Behaviors.Add(new EntryLengthValidatorBehavior() { MaxLength = 16 });

            Label lblNewRepeat = new Label()
            {
                Text = "Repite nueva contraseña",
                FontSize = 16,
                FontAttributes = FontAttributes.Bold };

            Entry newRepeatEntry = new Entry()
            {
                IsPassword = true,
                FontSize = 16
            };
            newRepeatEntry.SetBinding(Entry.TextProperty, "RepeatPass");
            newRepeatEntry.Behaviors.Add(new EntryLengthValidatorBehavior() { MaxLength = 16 });

            Button btnSave = new Button()
            {
                Text = "Cambiar contraseña",
                FontSize = 16, FontAttributes = 
                FontAttributes.Bold,
                HorizontalOptions = LayoutOptions.Center
            };
            btnSave.SetBinding(Button.CommandProperty, "Save");

            StackLayout content = new StackLayout()
            {
                Children =
                {
                    lblOld,
                    oldEntry,
                    lblNew,
                    newEntry,
                    lblNewRepeat,
                    newRepeatEntry,
                    btnSave
                }
            };
            Content = new ScrollView() { Content = content};
        }
    }
}