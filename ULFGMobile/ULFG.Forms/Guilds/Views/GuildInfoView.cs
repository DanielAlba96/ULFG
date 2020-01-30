using Xamarin.Forms;
using ULFG.Core.Data.Item;
using ULFG.Forms.Guilds.ViewModels;
using System.Threading.Tasks;
using FFImageLoading.Forms;
using System;
using ULFG.Forms.PlatformInterfaces;
using ULFG.Core.Data.ItemManager.Impl;

namespace ULFG.Forms.Guilds.Views
{
    /// <summary>
    /// <see cref="ContentPage"/> que representa la vista de la página del detalle de un gremio
    /// </summary>
    public class GuildInfoView : ContentPage
    {
        public GuildInfoView(Guild guild)
        {
            Padding = new Thickness(10, 0, 10, 0);
            BindingContext = new GuildInfoViewModel(Navigation, guild);
            Title = "Info";
            Icon = DependencyService.Get<IResourceManager>().GetResourcesPath("info.png");

            RelativeLayout relative = new RelativeLayout();
            relative.VerticalOptions = LayoutOptions.StartAndExpand;

            Label lblMsg = new Label()
            {
                Text = "Mensaje del lider",
                FontSize = 18,
                HorizontalTextAlignment = TextAlignment.Center,
                FontAttributes = FontAttributes.Bold
            };
            Label lblPhantom = new Label()
            {
                Text = "Mensaje del lider",
                FontSize = 18,
                FontAttributes = FontAttributes.Bold,
                Opacity = 0
            };

            Label lblMsgText = new Label()
            {
                LineBreakMode = LineBreakMode.WordWrap,
                HorizontalTextAlignment = TextAlignment.Start,
                VerticalTextAlignment = TextAlignment.Start,
                TextColor = Color.FromHex("231F20")
            };
            lblMsgText.SetBinding(Label.TextProperty, "Msg");
            Frame frame = new Frame() { Content = lblMsgText };

            Label lblInfo = new Label() { Text = "Datos del gremio", FontSize = 18, FontAttributes = FontAttributes.Bold};
            var dias = (DateTime.Now - guild.CreatedAt).Days;
            Label lblDate = new Label()
            {
                
                Text = "Antiguedad: " + dias +"d",
                FontSize = 14,
                TextColor = Color.FromHex("231F20"),
                LineBreakMode= LineBreakMode.CharacterWrap
            };
            var leader = Task.Run(() => { return UserManager.DefaultManager.GetUserByIdAsync(guild.Leader); }).Result;
            Label lblLeader = new Label()
            { FontSize = 14,
                Text = "Lider: " + leader.Nickname + "@" + leader.Username,
                TextColor = Color.FromHex("231F20")
            };
            Label lblMembers = new Label() { FontSize = 14, TextColor = Color.FromHex("231F20") };
            lblMembers.SetBinding(Label.TextProperty, "Members");

            CachedImage img = new CachedImage();
            img.SetBinding(CachedImage.SourceProperty, "Source");


            relative.Children.Add(img,
             Constraint.Constant(10),
             Constraint.Constant(20),
             Constraint.Constant(120),
             Constraint.Constant(120));

            relative.Children.Add(lblInfo,
                Constraint.RelativeToView(img, (parent, sibling) =>
                {
                    return sibling.X + sibling.Width + 10;
                }), Constraint.RelativeToParent((parent) =>
                {
                    return parent.Y + 10;
                }));

            relative.Children.Add(lblLeader,
                Constraint.RelativeToView(lblInfo, (parent, sibling) =>
                {
                    return sibling.X;
                }), Constraint.RelativeToView(lblInfo, (parent, sibling) =>
                {
                    return sibling.Y + sibling.Height + 10;
                }));

            relative.Children.Add(lblDate,
               Constraint.RelativeToView(lblLeader, (parent, sibling) =>
               {
                   return sibling.X;
               }), Constraint.RelativeToView(lblLeader, (parent, sibling) =>
               {
                   return sibling.Y + sibling.Height + 10;
               }));

            relative.Children.Add(lblMembers,
               Constraint.RelativeToView(lblDate, (parent, sibling) =>
               {
                   return sibling.X;
               }), Constraint.RelativeToView(lblDate, (parent, sibling) =>
               {
                   return sibling.Y + sibling.Height + 10;
               }));

            relative.Children.Add(lblPhantom,
              Constraint.RelativeToParent((parent) => {
                  return ((parent.Width) - 10) / 2;
              }), Constraint.RelativeToView(lblMembers, (parent, sibling) =>
              {
                  return sibling.Y + sibling.Height;
              }));
            
            relative.Children.Add(lblMsg,
                Constraint.RelativeToView(lblPhantom, (parent, sibling) =>
                {
                    return sibling.X - sibling.Width / 2;
                }), Constraint.RelativeToView(lblPhantom, (parent, sibling) =>
                {
                    return sibling.Y + sibling.Height - 10;
                }));

            relative.Children.Add(frame,
                Constraint.RelativeToParent(parent =>
                {
                    return parent.X + 10;
                }), Constraint.RelativeToView(lblMsg, (parent, sibling) =>
                {
                    return sibling.Y + sibling.Height + 10;
                }), Constraint.RelativeToParent((parent) =>
                {
                    return parent.Width - 20;
                }), Constraint.Constant(200));

            Content = new ScrollView() { Content = relative, Margin=10 };
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            var context = (GuildInfoViewModel)BindingContext;
            MessagingCenter.Subscribe<object, string>(context, ULFG.Forms.App.GuildKickedKey, (sender, e) => context.HandleGuildKick(e));
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            var context = (GuildInfoViewModel)BindingContext;
            MessagingCenter.Unsubscribe<object, string>(context, ULFG.Forms.App.GuildKickedKey);
        }
    }
}
