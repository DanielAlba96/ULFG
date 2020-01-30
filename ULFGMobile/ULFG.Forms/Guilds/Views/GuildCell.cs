using FFImageLoading.Forms;
using System.IO;
using System.Threading.Tasks;
using ULFG.Core.Data.Item;
using ULFG.Core.Data.ItemManager.Impl;
using Xamarin.Forms;

namespace ULFG.Forms.Guilds.Views
{
    /// <summary>
    /// <see cref="ViewCell"/> que representa un gremio
    /// </summary>
    public class GuildCell : ViewCell
    {
        readonly Label lblMembers;
        readonly CachedImage guildIcon;

        public GuildCell()
        {
            Label lblTitle = new Label
            {
                FontSize = 16,
                FontAttributes = Xamarin.Forms.FontAttributes.Bold,
                TextColor = Color.FromHex("A45F00"),
                HorizontalOptions = LayoutOptions.StartAndExpand
            };

            Label lblDesc = new Label
            {
                FontSize = 14,
                TextColor = Color.FromHex("212121"),
                HorizontalOptions = LayoutOptions.StartAndExpand,
                LineBreakMode = LineBreakMode.WordWrap
            };

            Label phantom = new Label();

            lblMembers = new Label
            {
                FontAttributes = FontAttributes.Italic,
                TextColor = Color.FromHex("424242")
            };

            guildIcon = new CachedImage();

            lblTitle.SetBinding(Label.TextProperty, "Name");
            lblDesc.SetBinding(Label.TextProperty, "Description");

            RelativeLayout view = new RelativeLayout();

            view.Children.Add(guildIcon,
                Constraint.Constant(10),
                Constraint.Constant(10),
                Constraint.Constant(70),
                Constraint.Constant(70));

            view.Children.Add(lblTitle,
                Constraint.RelativeToView(guildIcon, (parent, sibling) =>
                {
                    return sibling.X + sibling.Width + 10;
                }),
                Constraint.RelativeToView(guildIcon, (parent, sibling) =>
                {
                    return sibling.Y - 10;
                }));

            view.Children.Add(lblDesc,
                Constraint.RelativeToView(lblTitle, (parent, sibling) =>
                {
                    return sibling.X;
                }),
                Constraint.RelativeToView(lblTitle, (parent, sibling) =>
                {
                    return sibling.Y + sibling.Height;
                }),
                Constraint.RelativeToParent((parent)=>
                {
                    return parent.Width - 95;
                }),
                Constraint.Constant(37));

            view.Children.Add(lblMembers,
                Constraint.RelativeToView(lblDesc, (parent, sibling) =>
                {
                    return parent.Width - 90;
                }),
                Constraint.RelativeToView(lblDesc, (parent, sibling) =>
                {
                    return sibling.Y + sibling.Height + 10;
                }));
            view.Children.Add(phantom,
                Constraint.RelativeToView(guildIcon, (parent, sibling) =>
                {
                    return sibling.X;
                }),
                Constraint.RelativeToView(guildIcon, (parent, sibling) =>
                {
                    return sibling.Y + sibling.Height;
                }),
               Constraint.RelativeToView(guildIcon, (parent, sibling) =>
               {
                   return sibling.Width;
               }),
               Constraint.Constant(10));

            View = view;
        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();

            if (!(BindingContext is Guild guild))
                return;

            var numMembers = Task.Run(async () => { return await GuildMemberManager.DefaultManager.GetNumberOfMembers(guild.Id); }).Result;
            lblMembers.Text = numMembers + ((numMembers > 1) ? " miembros" : " miembro");
            guildIcon.Source = ImageSource.FromStream(() => new MemoryStream(guild.Image));
        }
    }
}
