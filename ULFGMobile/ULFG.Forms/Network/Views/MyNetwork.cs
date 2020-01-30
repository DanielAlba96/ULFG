using System.Threading.Tasks;
using Xamarin.Forms;

namespace ULFG.Forms.Network.Views
{
    /// <summary>
    /// <see cref="TabbedPage"/> que representa la página principal de la red de contactos
    /// </summary>
    /// <remarks>Incluye pestañas con <see cref="FollowedListView"/>, <see cref="FollowingListView"/> y <see cref="BlockListView"/></remarks>
    public class MyNetwork:TabbedPage
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

        public MyNetwork()
        {
            Title = "Mi red";
            Children.Add(new FollowedListView());
            Children.Add(new FollowingListView());
            Children.Add(new BlockListView());
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (!updated)
            {
                Task.Run(()=>
                {
                    ((FollowedListView)Children[0]).Refresh();
                    ((FollowingListView)Children[1]).Refresh();
                    ((BlockListView)Children[2]).Refresh();
                    updated = true;
                }).Wait();
            }
        }
    }
}
