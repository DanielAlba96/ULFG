using ULFG.Core.Data.Item;
using Xamarin.Forms;
using ULFG.Forms.Guilds.ViewModels;
using FFImageLoading.Forms;
using ULFG.Forms.Behaviors.Validators;
using ULFG.Forms.Behaviors.Events.ViewsEventsBehaviors;
using ULFG.Forms.Behaviors.Events.ArgsConverters;

namespace ULFG.Forms.Guilds.Views
{
    /// <summary>
    /// <see cref="ContentPage"/> que representa la vista de la página de edición de un gremio
    /// </summary>
    public class EditGuildView : ContentPage
    {
        public EditGuildView(Guild guild)
        {
            BindingContext = new EditGuildViewModel(Navigation, guild);

            Title = "Editar gremio";

            CachedImage img = new CachedImage() { Aspect = Aspect.AspectFit, HeightRequest = 200, BitmapOptimizations = true };
            img.SetBinding(CachedImage.SourceProperty, "Source");

            Button btnImage = new Button()
            {
                Text = "Cambiar imagen",
                WidthRequest = 130,
                HorizontalOptions = LayoutOptions.Center,
            };
            btnImage.SetBinding(Button.CommandProperty, "ChangeImage");
            
            Label lblTitle = new Label()
            {
                Text = "Titulo:"
            };

            Entry entryTitle = new Entry();
            entryTitle.SetBinding(Entry.IsEnabledProperty, "NameEnabled");
            entryTitle.SetBinding(Entry.TextProperty, "Name");
            entryTitle.Behaviors.Add(new EntryLengthValidatorBehavior() { MaxLength = 24 });

            Button btnTitle = new Button() { Text = "Editar", HorizontalOptions = LayoutOptions.Center, WidthRequest = 100 };
            btnTitle.SetBinding(Button.TextProperty, "BtnName");
            btnTitle.SetBinding(Button.CommandProperty, "ChangeName");
            
            Label lblDesc = new Label()
            {
                Text = "Descripción (para no miembros):"
            };

            Editor editorDesc = new Editor(){ HeightRequest = 120 };
            editorDesc.SetBinding(Editor.TextProperty, "Desc");
            editorDesc.SetBinding(Editor.IsEnabledProperty, "DescEnabled");
            editorDesc.Behaviors.Add(new EntryLengthValidatorBehaviorMultiLine() { MaxLength = 80 });
            editorDesc.SetBinding(Editor.BackgroundColorProperty, "DescBackground");

            Button btnDesc = new Button() { HorizontalOptions = LayoutOptions.Center, WidthRequest = 100 };
            btnDesc.SetBinding(Button.TextProperty, "BtnDesc");
            btnDesc.SetBinding(Button.CommandProperty, "ChangeDesc");

            Label lblMsg = new Label() { Text = "Mensaje del gremio (para miembros):" };

            Editor editorMsg = new Editor() { HeightRequest = 120 };
            editorMsg.SetBinding(Editor.TextProperty, "Msg");
            editorMsg.SetBinding(Editor.IsEnabledProperty, "MsgEnabled");
            editorMsg.Behaviors.Add(new EntryLengthValidatorBehaviorMultiLine() { MaxLength = 240 });
            editorMsg.SetBinding(Editor.BackgroundColorProperty, "MsgBackground");

            Button btnMsg = new Button() { Text = "Editar" };
            btnMsg.SetBinding(Button.TextProperty, "BtnMsg");
            btnMsg.SetBinding(Button.CommandProperty, "ChangeMsg");

            Label lblVisibility = new Label() { Text = "Cualquiera puede unirse"};

            Switch visibility = new Switch();
            visibility.SetBinding(Switch.IsToggledProperty, "Visibility");
            var behaviour = (new SwitchEventBehavior()
            {
                SwitchEventName = "Toggled",
                SwitchConverter = new ToggledArgsConverter(),

            });
            behaviour.SetBinding(SwitchEventBehavior.CommandProperty, "ChangeVisibility");
            visibility.Behaviors.Add(behaviour);

            StackLayout content = new StackLayout()
            {
                Children =
                {
                   img,
                   btnImage,                  
                   lblTitle,
                   entryTitle,
                   btnTitle,
                   lblMsg,
                   editorMsg,
                   btnMsg,
                   lblDesc,
                   editorDesc,
                   btnDesc,
                   lblVisibility,
                   visibility
                }
            };
            Content = new ScrollView() { Content = content, Margin = 10 };
        }
    }
}
