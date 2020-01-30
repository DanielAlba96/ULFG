using System.Threading.Tasks;
using Xamarin.Forms;

namespace ULFG.Forms.Guilds.Views
{
    /// <summary>
    /// <see cref="TabbedPage"/> que representa la página principal de los gremios
    /// </summary>
    /// <remarks>Incluye pestañas con <see cref="GuildListView"/> y <see cref="SearchGuildView"/></remarks>
    public class GuildTabView : TabbedPage
    {
        bool updated;
        /// <summary>
        /// Indica si la página ha sido actualizada 
        /// </summary>
        public bool Updated
        {
            get { return updated; }
            set { updated = value; }
        }

        public GuildTabView()
        {
            Title = "Gremios";

            Children.Add(new GuildListView());
            Children.Add(new SearchGuildView());
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (!updated)
            {
                Task.Run(() =>
               {
                    ((GuildListView)Children[0]).Refresh();
                   updated = true;
               }).Wait();
            }
        }
    }
}
