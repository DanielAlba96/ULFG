using ULFG.Core.Data.Item;
using Xamarin.Forms;
using ULFG.Forms.Profiles.ViewModels;
using FFImageLoading.Forms;
using ULFG.Forms.PlatformInterfaces;

namespace ULFG.Forms.Profiles.Views
{
    /// <summary>
    /// <see cref="ContentPage"/> que representa la vista de la página del perfil de un usuario
    /// </summary>
    public class ExtProfileView : ContentPage
    {
        public ExtProfileView(User user)
        {
            BindingContext = new ExtProfileViewModel(Navigation, user);

            this.SetBinding(ContentPage.TitleProperty, "UserInfo");

            ToolbarItem msg = new ToolbarItem()
            {
                Text = "Enviar mensaje",
                Icon = DependencyService.Get<IResourceManager>().GetResourcesPath("newMessage.png")
            };

            msg.SetBinding(ToolbarItem.CommandProperty, "SendMsg");

            ToolbarItems.Add(msg);

            CachedImage img = new CachedImage()
            {
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                WidthRequest = 150,
                HeightRequest = 150,
                VerticalOptions = LayoutOptions.End,
            };
            img.SetBinding(CachedImage.SourceProperty, "UserIcon");

            Label lblUser = new Label()
            {
                FontSize = 18,
                HorizontalTextAlignment = TextAlignment.Center,
                FontAttributes = FontAttributes.Bold,
                TextColor = Color.FromHex("231F20")
            };
            lblUser.SetBinding(Label.TextProperty, "UserInfo");

            Label lblText = new Label() { Text = "Biografía" };

            Label lblBio = new Label() { TextColor = Color.FromHex("231F20") };
            lblBio.SetBinding(Label.TextProperty, "UserBio");

            Button btnFollow = new Button();
            btnFollow.SetBinding(Button.TextProperty, "FollowText");
            btnFollow.SetBinding(Button.CommandProperty, "Follow");
            btnFollow.SetBinding(Button.IsEnabledProperty, "FollowEnabled");
            btnFollow.SetBinding(Button.WidthRequestProperty, "FollowWidth");

            Button btnBlock = new Button();
            btnBlock.SetBinding(Button.TextProperty, "BlockText");
            btnBlock.SetBinding(Button.CommandProperty, "Block");
            btnBlock.SetBinding(Button.WidthRequestProperty, "BlockWidth");

            Frame content = new Frame() { Content = lblBio, Padding = 5 };

            StackLayout buttons = new StackLayout()
            {
                Children =
                {
                    btnFollow,
                    btnBlock
                },
                Orientation = StackOrientation.Horizontal,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                Spacing = 10
            };

            Grid grid = new Grid()
            {
                ColumnDefinitions =
                {
                    new ColumnDefinition{Width = GridLength.Star}
                },
                RowDefinitions =
                {
                    new RowDefinition{Height = 100},
                    new RowDefinition{Height = 30},
                    new RowDefinition{Height = 30},
                    new RowDefinition{Height = 150},
                    new RowDefinition{Height = 40}
                },
                RowSpacing = 10
            };

            grid.Children.Add(img, 0, 0);
            grid.Children.Add(lblUser, 0, 1);
            grid.Children.Add(lblText, 0, 2);
            grid.Children.Add(content, 0, 3);
            grid.Children.Add(buttons, 0, 4);

            Content = new ScrollView() { Content = grid};
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
        }
    }
}
