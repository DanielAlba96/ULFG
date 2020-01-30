using Plugin.Connectivity;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using ULFG.Core.Data.Item;
using ULFG.Core.Data.ItemManager.Impl;
using ULFG.Forms.Publications.Views;
using ULFG.Forms.Shared;
using Xamarin.Forms;

namespace ULFG.Forms.Publications.ViewModels
{
    /// <summary>
    /// <see cref="ViewModelBase"/> que representa el modelo del portal de publicaciones
    /// </summary>
    public class PortalViewModel : ViewModelBase
    {
        /// <summary>
        /// Usuario actual
        /// </summary>
        readonly User user = (User)Application.Current.Properties["user"];

        ObservableCollection<Publication> _items;
        bool _isRefreshing;

        DataTemplate _template;
        DelegateCommand _newPublication;
        DelegateCommand _refresh;

        /// <summary>
        /// Inicializa el modelo y se subscribe a los canales necesarios de <see cref="MessagingCenter"/>
        /// </summary>
        /// <param name="nav">Referencia a la navegación actual de la aplicación</param>
        public PortalViewModel(INavigation nav) : base(nav)
        {
            Application.Current.Properties["userNavigation"] = nav;
            var task = Task.Run(async () => { return await PublicationManager.DefaultManager.GetPublicationsByUserAsync(user.Id, CrossConnectivity.Current.IsConnected); });

            _template = new DataTemplate(typeof(PublicationCell));

            MessagingCenter.Subscribe<object, string>(this, ULFG.Forms.App.UserChangedKey, (sender, e) =>
            {
                _template = new DataTemplate(typeof(PublicationCell));
                RaisePropertyChanged("Template");
            });

            MessagingCenter.Subscribe<object, string>(this, ULFG.Forms.App.PublicationChangedKey, (sender, e) => HandlePubliChanged(e));

            MessagingCenter.Subscribe<object, string>(this, ULFG.Forms.App.FollowsChanged, (sender, e) => HandleFollowChange(e));

            _items = task.Result;
        }
        /// <summary>
        /// Lista de publicaciones
        /// </summary>
        public ObservableCollection<Publication> Items
        {
            set { _items = value; }
            get { return _items; }
        }

        /// <summary>
        /// Plantilla de los elementos de la lista de publicaciones
        /// </summary>
        public DataTemplate Template
        {
            get { return _template; }
            set { _template = value; }
        }

        /// <summary>
        /// Indica si la lista se está refrescando
        /// </summary>
        public bool IsRefreshing
        {
            get { return _isRefreshing; }
            set { _isRefreshing = value; }
        }

        /// <summary>
        /// Comando para crear una nueva publicación
        /// </summary>
        public ICommand NewPublication
        {
            get { return _newPublication = _newPublication ?? new DelegateCommand(async () => await NewPublicationCommand()); }
        }

        /// <summary>
        /// Comando para refrescar la lista
        /// </summary>
        public ICommand Refresh
        {
            get { return _refresh = _refresh ?? new DelegateCommand(async () => await RefreshCommand()); }
        }

        /// <summary>
        /// Acción del comando para crear nueva publicación
        /// </summary>
        async Task NewPublicationCommand()
        {
            await Navigation.PushAsync(new NewPublicationView());
        }

        /// <summary>
        /// Acción del comando para refrescar la lista
        /// </summary>
        async Task RefreshCommand()
        {
            _items = await PublicationManager.DefaultManager.GetPublicationsByUserAsync(user.Id, CrossConnectivity.Current.IsConnected);
            _isRefreshing = false;
            RaisePropertyChanged("Items");
            RaisePropertyChanged("IsRefreshing");
        }

        /// <summary>
        /// Gestiona la recepción de notificaciones de cambio se seguimiento escuchando por un canal de <see cref="MessagingCenter"/>
        /// </summary>
        /// <param name="e">Contenido del mensaje recibido del canal</param>
        void HandleFollowChange(string e)
        {
            Xamarin.Forms.Device.BeginInvokeOnMainThread(async () =>
            {
                if (e.Contains("Delete"))
                {
                    var id = e.Split(' ').ElementAt(1);
                    var aBorrar = new ObservableCollection<Publication>(_items.Where(x => x.AutorId == id));
                    _items = new ObservableCollection<Publication>(_items.Except(aBorrar));
                }
                else
                    _items = await PublicationManager.DefaultManager.GetPublicationsByUserAsync(user.Id);
                RaisePropertyChanged("Items");
            });
        }

        /// <summary>
        /// Gestiona los cambios en las publicaciones escuchando por un canal de <see cref="MessagingCenter"/>
        /// </summary>
        /// <param name="e">Contenido del mensaje recibido del canal</param>
        void HandlePubliChanged(string e)
        {
            Xamarin.Forms.Device.BeginInvokeOnMainThread(async () =>
            {

                var type = e.Split(' ').ElementAt(0);
                var id = e.Split(' ').ElementAt(1);
                if (type.Contains("Deleted"))
                {
                    var index = _items.IndexOf(_items.FirstOrDefault(x => x.Id == id));
                    _items.RemoveAt(index);
                    RaisePropertyChanged("Items");
                }
                else
                {
                    var publi = await PublicationManager.DefaultManager.GetPublicationByIdAsync(id);
                    _items.Insert(0, publi);
                    RaisePropertyChanged("Items");
                }
            });
        }
    }
}
