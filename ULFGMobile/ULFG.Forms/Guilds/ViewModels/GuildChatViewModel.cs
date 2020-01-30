using Acr.UserDialogs;
using Plugin.Connectivity;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using ULFG.Core.Data.Item;
using ULFG.Core.Data.ItemManager.Impl;
using ULFG.Forms.Shared;
using Xamarin.Forms;

namespace ULFG.Forms.Guilds.ViewModels
{
    /// <summary>
    /// <see cref="ViewModelBase"/> que representa el modelo de la página del chat de gremio
    /// </summary>
    public class GuildChatViewModel : ViewModelBase
    {
        /// <summary>
        /// Usuario actual
        /// </summary>
        readonly User user = (User)Application.Current.Properties["user"];

        /// <summary>
        /// Gremio actual
        /// </summary>
        readonly Guild guild;

        ObservableCollection<Message> _items;
        string _msg;

        DelegateCommand _send;

        /// <summary>
        /// Inicializa el modelo y se subscribe a los canales necesarios de <see cref="MessagingCenter"/>
        /// </summary>
        /// <param name="nav">Referencia a la navegación actual de la aplicación</param>
        /// <param name="guild">Gremio actual</param>
        public GuildChatViewModel(INavigation nav, Guild guild) : base(nav)
        {
            var task = Task.Run(() => MessageManager.DefaultManager.GetMessagesByGuildIdAsync(guild.Id, CrossConnectivity.Current.IsConnected));
            this.guild = guild;
            MessagingCenter.Subscribe<object, string>(this, ULFG.Forms.App.NewGuildMessageKey, async (sender, e) => await HandleNewMessage(e));
            MessagingCenter.Subscribe<object, string>(this, ULFG.Forms.App.GuildKickedKey, (sender, e) => HandleGuildKick(e));
            _msg = string.Empty;
            _items = task.Result;
        }

        /// <summary>
        /// Lista de mensajes del chat de gremio
        /// </summary>
        public ObservableCollection<Message> Items { get => _items; set => _items = value; }

        /// <summary>
        /// Texto del campo de mensaje a enviar
        /// </summary>
        public string Msg { get => _msg; set => _msg = value; }

        /// <summary>
        /// Comando para enviar un mensaje al chat
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
                var config = new AlertConfig() { Title = "Error", Message = "Un mensaje de chat no puede estar vacio" };
                await UserDialogs.Instance.AlertAsync(config);
                return;
            }

            Message m = new Message
            {
                Text = _msg,
                Guild_Id = guild.Id,
                Creator_Id = user.Id,
                CreationDate = DateTime.Now
            };
            await MessageManager.DefaultManager.SaveMsgAsync(m);
            _msg = "";
            _items.Add(m);
            RaisePropertyChanged("Msg");
            RaisePropertyChanged("Items");
            MessagingCenter.Send<object, string>(this, ULFG.Forms.App.NewMessageScrollKey, "");
        }

        /// <summary>
        /// Gestiona los mensajes nuevos escuchando por un canal de <see cref="MessagingCenter"/>
        /// </summary>
        /// <param name="msg">Contenido del mensaje recibido del canal</param>
        async Task HandleNewMessage(string msg)
        {
                if (msg.Split(' ').Length > 2)
                {
                    var id = msg.Split(' ').ElementAt(2);
                    var newMsg = await MessageManager.DefaultManager.GetMessageByIdAsync(id, CrossConnectivity.Current.IsConnected);
                    _items.Add(newMsg);
                }
                else
                    _items = await MessageManager.DefaultManager.GetMessagesByGuildIdAsync(guild.Id, CrossConnectivity.Current.IsConnected);

                RaisePropertyChanged("Items");                
                MessagingCenter.Send<object, string>(this, ULFG.Forms.App.NewMessageScrollKey, "");
        
        }

        /// <summary>
        /// Gestiona el caso en el que el usuario actual sea expulsado del gremio mientras está en esta pantalla escuchando por un canal 
        /// de <see cref="MessagingCenter"/>
        /// </summary>
        /// <param name="msg">Contenido del mensaje recibido del canal</param>
        void HandleGuildKick(string msg)
        {
            Xamarin.Forms.Device.BeginInvokeOnMainThread(async () =>
            {
                if (guild.Id.Equals(msg.Split(' ').ElementAt(1)))
                {
                    var config = new AlertConfig() { Title = "Expulsion", Message = " Has sido expulsado del gremio " + guild.Name };
                    await UserDialogs.Instance.AlertAsync(config);
                    await Navigation.PopToRootAsync();
                }
            });
        }
    }
}
