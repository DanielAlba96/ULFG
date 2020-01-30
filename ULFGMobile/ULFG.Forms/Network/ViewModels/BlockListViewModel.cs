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
    /// <see cref="ViewModelBase"/> que representa el modelo de la página con la lista de bloqueados
    /// </summary>
    public class BlockListViewModel : ViewModelBase
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
        public BlockListViewModel(INavigation nav) : base(nav)
        {
            var task = Task.Run(() => { return BlockManager.DefaultManager.GetBlocksByBlockingUserAsync(actualUser.Id, CrossConnectivity.Current.IsConnected); });
            MessagingCenter.Subscribe<object, string>(this, ULFG.Forms.App.BlockChangedKey, (sender, e) => HandleBlockChange(e));
            _items = task.Result;
        }

        /// <summary>
        /// Refresca la lista de bloqueados
        /// </summary>
        /// <returns></returns>
        public async Task Refresh()
        {
           _items= await BlockManager.DefaultManager.GetBlocksByBlockingUserAsync(actualUser.Id, CrossConnectivity.Current.IsConnected);
            RaisePropertyChanged("Items");
        }

        /// <summary>
        /// Lista de bloqueados
        /// </summary>
        public ObservableCollection<User> Items { get => _items; set => _items = value; }


        /// <summary>
        /// Gestiona cambios en los bloqueos escuchando a través de un canal de <see cref="MessagingCenter"/>
        /// </summary>
        /// <param name="e">Contenido del mensaje recibido del canal</param>
        void HandleBlockChange(string e)
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
