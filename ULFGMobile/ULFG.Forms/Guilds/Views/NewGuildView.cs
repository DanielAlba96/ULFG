using Xamarin.Forms;
using ULFG.Forms.Guilds.ViewModels;
using ULFG.Forms.Behaviors.Validators;

namespace ULFG.Forms.Guilds.Views
{
    /// <summary>
    /// <see cref="ContentPage"/> que representa la vista de la página de creación de un gremio
    /// </summary>
    public class NewGuildView : ContentPage
	{
        public NewGuildView ()
		{
            BindingContext = new NewGuildViewModel(Navigation);

            Title = "Creacion de gremio";

            Label lblNombre = new Label { Text = "Pon un nombre a tu gremio(*)" };

            Entry nameEntry = new Entry();
            nameEntry.SetBinding(Entry.TextProperty, "Name");
            nameEntry.Behaviors.Add(new EntryLengthValidatorBehavior() { MaxLength = 24});
            Label lblDesc = new Label { Text = "Escribe una breve descripcion de tu gremio" };

            Editor descEntry = new Editor() { HeightRequest = 120};
            descEntry.SetBinding(Editor.TextProperty, "Desc");
            descEntry.Behaviors.Add(new EntryLengthValidatorBehaviorMultiLine() { MaxLength = 240 });
            
            Label Choose = new Label() { Text = "Cualquiera puede unirse" };

            Switch visibility = new Switch();
            visibility.SetBinding(Switch.IsToggledProperty, "Visibility");
            
            Button btnCreate = new Button() { Text = "Crear Gremio" };
            btnCreate.SetBinding(Button.CommandProperty, "Create");

            StackLayout stack = new StackLayout()
            {
                Orientation = StackOrientation.Vertical,
                Children =
                {
                    lblNombre,
                    nameEntry,
                    lblDesc,
                    descEntry,
                    Choose,
                    visibility,
                    btnCreate
                }
            };
            Content = new ScrollView() { Content = stack };
		}
	}
}