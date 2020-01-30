using Plugin.Connectivity;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using ULFG.Core.Data.Item;
using ULFG.Core.Data.ItemManager.Impl;
using ULFG.Forms.Profiles.Views;
using ULFG.Forms.Shared;
using Xamarin.Forms;

namespace ULFG.Forms.PrivateChat.ViewModels
{
    /// <summary>
    /// <see cref="ViewModelBase"/> que representa el modelo de la página con la lista de chats activos
    /// </summary>
    public class ChatListViewModel : ViewModelBase
    {
        /// <summary>
        /// Usuario actual
        /// </summary>
        readonly User user = (User)Application.Current.Properties["user"];

        ObservableCollection<Chat> _items;
        bool _isRefreshing;

        DelegateCommand _new;
        DelegateCommand _refresh;

        /// <summary>
        /// Inicializa el modelo y se subscribe a los canales necesarios de <see cref="MessagingCenter"/>
        /// </summary>
        /// <param name="nav">Referencia a la navegación actual de la aplicación</param>
        public ChatListViewModel(INavigation nav) : base(nav)
        {
            var task = Task.Run(() => { return ChatManager.DefaultManager.GetChatsByMemberIdAsync(user.Id, CrossConnectivity.Current.IsConnected); });
            _isRefreshing = false;
            MessagingCenter.Subscribe<object, string>(this, ULFG.Forms.App.NewMessageKey, (sender, e) => HandleNewMessage(e));
            MessagingCenter.Subscribe<object, string>(this, ULFG.Forms.App.ChatDeletedKey, (sender, e) => HandleDeleteChat(e));
            _items = task.Result;
        }

        /// <summary>
        /// Comando para crear un nuevo mensaje de chat
        /// </summary>
        public ICommand New
        {
            get { return _new = _new ?? new DelegateCommand(async () => await Navigation.PushAsync(new UserListView(true))); }
        }

        /// <summary>
        /// Comando para refrescar la lista de chats
        /// </summary>
        public ICommand Refresh
        {
            get { return _refresh = _refresh ?? new DelegateCommand(async () => await RefreshCommand()); }
        }

        /// <summary>
        /// Lista de chats
        /// </summary>
        public ObservableCollection<Chat> Items { get => _items; set => _items = value; }

        /// <summary>
        /// Indica si la lista de chats se está refrescando
        /// </summary>
        public bool IsRefreshing { get => _isRefreshing; set => _isRefreshing = value; }

        async Task RefreshCommand()
        {
            _items = await ChatManager.DefaultManager.GetChatsByMemberIdAsync(user.Id, CrossConnectivity.Current.IsConnected);
            _isRefreshing = false;
            RaisePropertyChanged("Items");
            RaisePropertyChanged("IsRefreshing");
        }

        /// <summary>
        /// Gestiona los nuevos mensajes recibidos escuchando a través de un canal de <see cref="MessagingCenter"/>
        /// </summary>
        /// <param name="data">Contenido del mensaje recibido del canal</param>
        void HandleNewMessage(string data)
        {
            Xamarin.Forms.Device.BeginInvokeOnMainThread(async () =>
            {
                if (data.Split(' ').Length > 2)
                {
                    var msgId = data.Split(' ').ElementAt(2);
                    var msg = await MessageManager.DefaultManager.GetMessageByIdAsync(msgId, CrossConnectivity.Current.IsConnected);
                    var id = msg.Chat_Id;
                    var exist = _items.Any(x => x.Id == id);
                    if (!exist)
                    {
                        var chat = await ChatManager.DefaultManager.GetChatByIdAsync(id, CrossConnectivity.Current.IsConnected);
                        _items.Add(chat);
                        RaisePropertyChanged("Items");
                    }
                }
                else
                    await RefreshCommand();
            });
        }

        /// <summary>
        /// Gestiona el borrado de los chats escuchando a través de un canal de <see cref="MessagingCenter"/>
        /// </summary>
        /// <param name="msg">Contenido del mensaje recibido del canal</param>
        void HandleDeleteChat(string msg)
        {
            Xamarin.Forms.Device.BeginInvokeOnMainThread(() =>
            {
                var aBorrar = _items.FirstOrDefault(x => x.Id == msg);
                if (aBorrar != null)
                {
                    _items.Remove(aBorrar);
                    RaisePropertyChanged("Items");
                }
            });
        }
    }
}
