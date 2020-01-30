using Acr.UserDialogs;
using Plugin.Connectivity;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using ULFG.Core.Data.Item;
using ULFG.Core.Data.ItemManager.Impl;
using ULFG.Forms.Guilds.Views;
using ULFG.Forms.PrivateChat.Views;
using ULFG.Forms.Shared;
using Xamarin.Forms;

namespace ULFG.Forms.Guilds.ViewModels
{
    /// <summary>
    /// <see cref="ViewModelBase"/> que representa el modelo de la página con la lista de todos los gremios
    /// </summary>
    public class SearchGuildViewModel : ViewModelBase
    {
        /// <summary>
        /// Usuario actual
        /// </summary>
        readonly User user = (User)Application.Current.Properties["user"];

        string _searchText;
        ObservableCollection<Guild> _items;

        DelegateCommand _search;

        /// <summary>
        /// Inicializa el modelo y se subscribe a los canales necesarios de <see cref="MessagingCenter"/>
        /// </summary>
        /// <param name="nav">Referencia a la navegación actual de la aplicación</param>
        public SearchGuildViewModel(INavigation nav) : base(nav)
        {
            var task = Task.Run(async () => { return await GuildManager.DefaultManager.GetGuildsAsync(CrossConnectivity.Current.IsConnected); });
            MessagingCenter.Subscribe<object, string>(this, ULFG.Forms.App.GuildChangedKey, async (sender, e) => await HandleGuildChanged(e));
            MessagingCenter.Subscribe<object, string>(this, ULFG.Forms.App.GuildMemberChangedKey, async (sender, e) => await HandleGuildChanged("Edit " + e.Split(' ').ElementAt(1)));
            _items = task.Result;
        }

        /// <summary>
        /// Comando para filtrar la lista de gremios
        /// </summary>
        public ICommand Search
        {
            get { return _search = _search ?? new DelegateCommand(SearchCommand); }
        }

        /// <summary>
        /// Texto del campo de búsqueda
        /// </summary>
        public string SearchText { get => _searchText; set => _searchText = value; }
        /// <summary>
        /// Lista de gremios
        /// </summary>
        public ObservableCollection<Guild> Items { get => _items; set => _items = value; }

        /// <summary>
        /// Acción del comando <seealso cref="Search"/>
        /// </summary>
        void SearchCommand()
        {
            var config = new ProgressDialogConfig() { Title = "Buscando gremios" };
            var progress = UserDialogs.Instance.Progress(config);
            _items = Task.Run(async () => { return await GuildManager.DefaultManager.GetGuildByWordAsync(_searchText, CrossConnectivity.Current.IsConnected); }).Result;
            RaisePropertyChanged("Items");
            progress.Dispose();
        }

        /// <summary>
        /// Evento para cuando se modifica el texto del campo de búsqueda
        /// </summary>
        /// <param name="sender">El componente que envía el mensaje</param>
        /// <param name="textChangedEventArgs">Información sobre el evento</param>
        public void SearchTextChanged(object sender, TextChangedEventArgs textChangedEventArgs)
        {
            if (_searchText.Equals(string.Empty))
            {
                _items = Task.Run(async () => { return await GuildManager.DefaultManager.GetGuildsAsync(CrossConnectivity.Current.IsConnected); }).Result;
                RaisePropertyChanged("Items");
            }
        }

        /// <summary>
        /// Evento para cuando se toca un elemento de la lista
        /// </summary>
        /// <param name="e">El elemento tocado</param>
        public async Task ItemTapped(ItemTappedEventArgs e)
        {
            Guild gu = (Guild)e.Item;
            var isMember = await GuildManager.DefaultManager.IsMemberAsync(gu.Id, user.Id);
            string res;
            if (isMember)
                await Navigation.PushAsync(new GuildProfileTabView(gu));
            else
            {
                if (gu.IsPublic)
                    res = await UserDialogs.Instance.ActionSheetAsync("Gremio " + gu.Name, "Atras", null, null, "Unirse", "Contactar lider");
                else
                    res = await UserDialogs.Instance.ActionSheetAsync("Gremio " + gu.Name, "Atras", null, null, "Contactar lider");
                switch (res)
                {
                    case "Unirse":
                        var member = new GuildMember
                        {
                            GuildId = gu.Id,
                            MemberId = user.Id
                        };
                        await GuildMemberManager.DefaultManager.SaveGuildMemberAsync(member);
                        Device.BeginInvokeOnMainThread(() =>
                        {
                            Navigation.PushAsync(new GuildProfileTabView(gu));
                        });
                        break;
                    case "Contactar lider":
                        User leader = await UserManager.DefaultManager.GetUserByIdAsync(gu.Leader);
                        await Navigation.PushAsync(new SendMessageView(leader));
                        break;
                }
            }
        }

        /// <summary>
        /// Grstiona los cambios en los gremios escuchando por un canal dev <see cref="MessagingCenter"/>
        /// </summary>
        /// <param name="msg">Contenido del mensaje recibido del canal</param>
        async Task HandleGuildChanged(string msg)
        {
            var type = msg.Split(' ').ElementAt(0);
            var id = msg.Split(' ').ElementAt(1);
            if (type.Equals("Delete"))
            {
                if (msg.Contains("$"))
                    return;
                var aBorrar = _items.FirstOrDefault(x => x.Id == id);
                if (aBorrar != null)
                {
                    _items.Remove(aBorrar);
                    RaisePropertyChanged("Items");
                }

            }
            else if (type.Equals("Edit"))
            {               
                if (_items.Any(x => x.Id == id))
                {
                    var index = _items.IndexOf(_items.FirstOrDefault(x => x.Id == id));
                    var guild = await GuildManager.DefaultManager.GetGuildByIdAsync(id);
                    _items[index] = guild;
                    RaisePropertyChanged("Items");
                }
            }
            else if (type.Equals("New"))
            {
                _searchText = string.Empty;
                RaisePropertyChanged("SearchText");
            }
        }
    }
}
