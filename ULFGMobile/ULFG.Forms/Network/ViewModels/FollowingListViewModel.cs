using Plugin.Connectivity;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using ULFG.Core.Data.Item;
using ULFG.Core.Data.ItemManager.Impl;
using ULFG.Forms.Shared;
using Xamarin.Forms;

namespace ULFG.Forms.Network.ViewModels
{
    /// <summary>
    /// <see cref="ViewModelBase"/> que representa el modelo de la página con la lista de seguidores
    /// </summary>
    public class FollowingListViewModel : ViewModelBase
    {
        /// <summary>
        /// Usuario actual
        /// </summary>
        readonly User actualUser = (User)Application.Current.Properties["user"];

        ObservableCollection<User> _items;

        /// <summary>
        /// Inicializa el modelo
        /// </summary>
        /// <param name="nav">Referencia a la navegación actual de la aplicación</param>
        public FollowingListViewModel(INavigation nav) : base(nav)
        {
            var task = Task.Run(() => { return FollowManager.DefaultManager.GetUsersFollowing(actualUser.Id, CrossConnectivity.Current.IsConnected); });
            _items = task.Result;
        }

        /// <summary>
        /// Refresca la lista de seguidores
        /// </summary>
        public async Task Refresh()
        {
            _items = await FollowManager.DefaultManager.GetUsersFollowing(actualUser.Id, CrossConnectivity.Current.IsConnected);
            RaisePropertyChanged("Items");
        }

        /// <summary>
        /// Lista de seguidores
        /// </summary>
        public ObservableCollection<User> Items { get => _items; set => _items = value; }
    }
}
