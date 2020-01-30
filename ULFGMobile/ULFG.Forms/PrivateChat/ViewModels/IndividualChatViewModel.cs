using Acr.UserDialogs;
using Plugin.Connectivity;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using ULFG.Core.Data.Item;
using ULFG.Core.Data.ItemManager.Impl;
using ULFG.Core.Logic;
using ULFG.Forms.Profiles.Views;
using ULFG.Forms.Shared;
using Xamarin.Forms;

namespace ULFG.Forms.PrivateChat.ViewModels
{
    /// <summary>
    /// <see cref="ViewModelBase"/> que representa el modelo de la página del chat individual
    /// </summary>
    public class IndividualChatViewModel : ViewModelBase
    {
        /// <summary>
        /// Acceso a la lógica de mensajería
        /// </summary>
        readonly MessageOperations messageOperations = new MessageOperations();

        /// <summary>
        /// Usuario actual
        /// </summary>
        readonly User user = (User)Application.Current.Properties["user"];

        /// <summary>
        /// El otro usuario miembro del chat
        /// </summary>
        readonly User other;

        /// <summary>
        /// El chat actual
        /// </summary>
        readonly Chat chat;

        string _title;
        string _msg = string.Empty;
        bool _messageActive;
        bool _sendActive;
        ObservableCollection<Message> _items;

        DelegateCommand _leave;
        DelegateCommand _profile;
        DelegateCommand _send;

        /// <summary>
        /// Inicializa el modelo y se subscribe a los canales necesarios de <see cref="MessagingCenter"/>
        /// </summary>
        /// <param name="nav">Referencia a la navegación actual de la aplicación</param>
        /// <param name="chat">Chat actual</param>
        public IndividualChatViewModel(INavigation nav, Chat chat) : base(nav)
        {
            _msg = string.Empty;
            var chatTask = Task.Run(() => { return ChatManager.DefaultManager.GetChatByIdAsync(chat.Id, CrossConnectivity.Current.IsConnected); });
            var task = Task.Run(() => { return MessageManager.DefaultManager.GetMessagesByChatIdAsync(chat.Id, CrossConnectivity.Current.IsConnected); });
            string otherId;
            if (chat.Member1_id.Equals(user.Id))
                otherId = chat.Member2_id;
            else
                otherId = chat.Member1_id;
            other = Task.Run(() => { return UserManager.DefaultManager.GetUserByIdAsync(otherId, CrossConnectivity.Current.IsConnected); }).Result;
            _title = "Chat con " + other.Nickname;

            this.chat = chatTask.Result;
            if (this.chat.Member1_deleted || this.chat.Member2_deleted)
            {
                _messageActive = false;
                _msg = "El otro usuario ha abandonado el chat";
                _sendActive = false;
            }
            else
            {
                var block = Task.Run(() => { return BlockManager.DefaultManager.GetBlocksByBothSidesAsync(other.Id, user.Id, CrossConnectivity.Current.IsConnected); }).Result;
                if (block != null)
                {
                    _messageActive = false;
                    _sendActive = false;
                    _msg = "Este usuario te esta bloqueando";
                }
                else
                {
                    _messageActive = true;
                    _sendActive = true;
                }
            }
            MessagingCenter.Subscribe<object, string>(this, ULFG.Forms.App.NewMessageKey, (sender, e) => HandleNewMessage(e));
            _items = task.Result;
        }

        /// <summary>
        /// Titulo de la ventana de chat
        /// </summary>
        public string Title { get => _title; set => _title = value; }

        /// <summary>
        /// Contenido del campo del mensaje
        /// </summary>
        public string Msg { get => _msg; set => _msg = value; }

        /// <summary>
        /// Indica si el campo del mensaje está activo
        /// </summary>
        public bool MessageActive { get => _messageActive; set => _messageActive = value; }

        /// <summary>
        /// Indica si el botón enviar está activo
        /// </summary>
        public bool SendActive { get => _sendActive; set => _sendActive = value; }

        /// <summary>
        /// Lista de mensajes del chat
        /// </summary>
        public ObservableCollection<Message> Items { get => _items; set => _items = value; }

        /// <summary>
        /// Comando para abandonar el chat
        /// </summary>
        public ICommand Leave
        {
            get { return _leave = _leave ?? new DelegateCommand(async () => await LeaveCommand()); }
        }

        /// <summary>
        /// Comando para ver el perfil del otro usuario
        /// </summary>
        public ICommand Profile
        {
            get { return _profile = _profile ?? new DelegateCommand(async () => { await Navigation.PushAsync(new ExtProfileView(other)); }); }
        }

        /// <summary>
        /// Comando para enviar un mensaje
        /// </summary>
        public ICommand Send
        {
            get { return _send = _send ?? new DelegateCommand(async () => await SendCommand()); }
        }

        /// <summary>
        /// Acción del comando <seealso cref="Send"/>
        /// </summary>
        async Task SendCommand()
        {
            if (_msg.Length < 1)
            {
                var config = new AlertConfig() { Title = "Error", Message = "Un mensaje privado no puede estar vacio" };
                await UserDialogs.Instance.AlertAsync(config);
                return;
            }

            var block = Task.Run(() => { return BlockManager.DefaultManager.GetBlocksByBothSidesAsync(other.Id, user.Id, CrossConnectivity.Current.IsConnected); }).Result;
            if (block == null)
            {
                var m = new Message() { Chat_Id = chat.Id, Text = _msg, Creator_Id = user.Id, CreationDate = DateTime.Now };
                await MessageManager.DefaultManager.SaveMsgAsync(m);
                _msg = "";
                _items.Add(m);
                RaisePropertyChanged("Msg");
                RaisePropertyChanged("Items");
                MessagingCenter.Send<object, string>(this, ULFG.Forms.App.NewMessageScrollKey, "");
            }
            else
            {
                var config = new AlertConfig() { Title = "Bloqueado", Message = "Este usuario te ha bloqueado y no puedes mandarle mensajes" };
                await UserDialogs.Instance.AlertAsync(config);
            }
        }

        /// <summary>
        /// Acción del comando <seealso cref="Leave"/>
        /// </summary>
        async Task LeaveCommand()
        {
            Task.Run(() => messageOperations.DeleteChat(chat.Id, user.Id)).Wait();
            MessagingCenter.Send<object, string>(this, ULFG.Forms.App.ChatDeletedKey, chat.Id);
            await Navigation.PopAsync();
        }

        /// <summary>
        /// Gestiona los nuevos mensajes recibidos escuchando a través de un canal de <see cref="MessagingCenter"/>
        /// </summary>
        /// <param name="msg">Contenido del mensaje recibido del canal</param>
        void HandleNewMessage(string msg)
        {
            Xamarin.Forms.Device.BeginInvokeOnMainThread(async () =>
            {
                if (msg.Split(' ').Length > 2)
                {
                    var id = msg.Split(' ').ElementAt(2);
                    _items.Add(await MessageManager.DefaultManager.GetMessageByIdAsync(id, CrossConnectivity.Current.IsConnected));
                }
                else
                    _items = await MessageManager.DefaultManager.GetMessagesByChatIdAsync(chat.Id, CrossConnectivity.Current.IsConnected);
                RaisePropertyChanged("Items");
                MessagingCenter.Send<object, string>(this, ULFG.Forms.App.NewMessageScrollKey, "");
            });
        }
    }
}
