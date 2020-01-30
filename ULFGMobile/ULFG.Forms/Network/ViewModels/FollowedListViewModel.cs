using Plugin.Connectivity;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using ULFG.Core.Data.Item;
using ULFG.Core.Data.ItemManager.Impl;
using ULFG.Forms.Shared;
using Xamarin.Forms;

namespace ULFG.Forms.Network.ViewModels
{
    /// <summary>
    /// <see cref="ViewModelBase"/> que representa el modelo de la página con la lista de seguimientos activos
    /// </summary>
    public class FollowedListViewModel : ViewModelBase
    {
        /// <summary>
        /// Usuario actual
        /// </summary>
        readonly User actualUser = (User)Application.Current.Properties["user"];

        ObservableCollection<User> _items;

        /// <summary>
        /// Inicializa el modelo y se subscribe a los canales necesarios de <see cref="MessagingCenter"/>
        /// </summary>
        /// <param name="nav">Referencia a la navegación actual de la aplicación</param>
        public FollowedListViewModel(INavigation nav) : base(nav)
        {
            _items = Task.Run(() => { return FollowManager.DefaultManager.GetUsersFollowedBy(actualUser.Id, CrossConnectivity.Current.IsConnected); }).Result;
            MessagingCenter.Subscribe<object, string>(this, ULFG.Forms.App.FollowsChanged, (sender, e) => HandleFollowChange(e));
        }

        /// <summary>
        /// Refresca la lista de seguimientos
        /// </summary>
        /// <returns></returns>
        public async Task Refresh()
        {
            _items = await FollowManager.DefaultManager.GetUsersFollowedBy(actualUser.Id, CrossConnectivity.Current.IsConnected);
            RaisePropertyChanged("Items");
        }

        /// <summary>
        /// Lista de seguimientos
        /// </summary>
        public ObservableCollection<User> Items { get => _items; set => _items = value; }

        /// <summary>
        /// Gestiona cambios en los seguidores escuchando a través de un canal de <see cref="MessagingCenter"/>
        /// </summary>
        /// <param name="e">Contenido del mensaje recibido del canal</param>
        void HandleFollowChange(string e)
        {
            var id = e.Split(' ').ElementAt(1);
            if (e.Contains("Delete"))
            {

                var item = _items.FirstOrDefault(x => x.Id == id);
                _items.Remove(item);
                RaisePropertyChanged("Items");
            }
            else
            {
                var user = Task.Run(() => { return UserManager.DefaultManager.GetUserByIdAsync(id); }).Result;
                _items.Add(user);
                RaisePropertyChanged("Items");
            }
        }
    }
}
