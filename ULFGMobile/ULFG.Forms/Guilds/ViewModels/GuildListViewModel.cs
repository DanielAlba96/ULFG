using Plugin.Connectivity;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using ULFG.Core.Data.Item;
using ULFG.Core.Data.ItemManager.Impl;
using ULFG.Forms.Guilds.Views;
using ULFG.Forms.Shared;
using Xamarin.Forms;

namespace ULFG.Forms.Guilds.ViewModels
{
    /// <summary>
    /// <see cref="ViewModelBase"/> que representa el modelo de la página con la lista de gremios a los que se pertenece
    /// </summary>
    public class GuildListViewModel : ViewModelBase
    {
        /// <summary>
        /// Usuario actual
        /// </summary>
        readonly User user = (User)Application.Current.Properties["user"];

        ObservableCollection<Guild> _items;
        DataTemplate _template;
        bool _isRefreshing;

        DelegateCommand _new;

        /// <summary>
        /// Inicializa el modelo y se subscribe a los canales necesarios de <see cref="MessagingCenter"/>
        /// </summary>
        /// <param name="nav">Referencia a la navegación actual de la aplicación</param>
        public GuildListViewModel(INavigation nav) : base(nav)
        {
            var task = Task.Run(() => GuildManager.DefaultManager.GetGuildsByMemberIdAsync(user.Id, CrossConnectivity.Current.IsConnected));
            MessagingCenter.Subscribe<object, string>(this, ULFG.Forms.App.GuildChangedKey, async (sender, e) => await HandleGuildChanged(e));
            _items = task.Result;
            _template = new DataTemplate(typeof(GuildCell));
        }

        /// <summary>
        /// Lista de gremios a los que se pertenece
        /// </summary>
        public ObservableCollection<Guild> Items { get => _items; set => _items = value; }

        /// <summary>
        /// Indica si la lista de gremios se esta refrescando
        /// </summary>
        public bool IsRefreshing { get => _isRefreshing; set => _isRefreshing = value; }

        /// <summary>
        /// Plantilla de cada elemento de la lista de gremios
        /// </summary>
        public DataTemplate Template { get => _template; set => _template = value; }

        /// <summary>
        /// Comando para crear un nuevo gremio
        /// </summary>
        public ICommand New
        {
            get { return _new = _new ?? new DelegateCommand(async () => await NewCommand()); }
        }

        /// <summary>
        /// Acción del comando <seealso cref="New"/>
        /// </summary>
        /// <returns></returns>
        async Task NewCommand()
        {
            await Navigation.PushAsync(new NewGuildView());
        }

        /// <summary>
        /// Refresca la lista de gremios
        /// </summary>
        public async Task RefreshCommand()
        {
            _items = await GuildManager.DefaultManager.GetGuildsByMemberIdAsync(user.Id, CrossConnectivity.Current.IsConnected);
            _isRefreshing = false;
            RaisePropertyChanged("Items");
            RaisePropertyChanged("IsRefreshing");
        }

        /// <summary>
        /// Gestiona los cambios en la lista de gremios escuchando a través de un canal de <see cref="MessagingCenter"/>
        /// </summary>
        /// <param name="msg">Contenido del mensaje recibido del canal</param>
        async Task HandleGuildChanged(string msg)
        {
            var type = msg.Split(' ').ElementAt(0);
            var id = msg.Split(' ').ElementAt(1);
            if (type.Equals("New"))
            {
                var guild = await GuildManager.DefaultManager.GetGuildByIdAsync(id);
                _items.Add(guild);
                RaisePropertyChanged("Items");
            }
            else if (type.Equals("Delete"))
            {
                var guild = _items.FirstOrDefault(x => x.Id == id);
                _items.Remove(guild);
                RaisePropertyChanged("Items");
            }
            else if (type.Equals("Edit"))
            {
                var index = _items.IndexOf(_items.FirstOrDefault(x => x.Id == id));
                if (index >= 0)
                {
                    var guild = await GuildManager.DefaultManager.GetGuildByIdAsync(id);
                    _items[index] = guild;
                    _template = new DataTemplate(typeof(GuildCell));
                    RaisePropertyChanged("Items");
                    RaisePropertyChanged("Template");
                }
            }
        }
    }
}
