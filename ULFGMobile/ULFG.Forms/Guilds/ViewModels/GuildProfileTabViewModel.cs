using Acr.UserDialogs;
using Plugin.Connectivity;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using ULFG.Core.Data.Item;
using ULFG.Core.Data.ItemManager.Impl;
using ULFG.Core.Logic;
using ULFG.Forms.Guilds.Views;
using ULFG.Forms.PrivateChat.Views;
using ULFG.Forms.Shared;
using Xamarin.Forms;

namespace ULFG.Forms.Guilds.ViewModels
{
    /// <summary>
    /// <see cref="ViewModelBase"/> que representa el modelo de la página del detalle de un gremio
    /// </summary>
    public class GuildProfileTabViewModel : ViewModelBase
    {
        /// <summary>
        /// Usuario actual
        /// </summary>
        readonly User actualUser = (User)Application.Current.Properties["user"];

        /// <summary>
        /// Acceso a la lógica de gremios
        /// </summary>
        readonly GuildOperations manager = new GuildOperations();

        /// <summary>
        /// Gremio actual
        /// </summary>
        Guild guild;

        string _title;

        DelegateCommand _invite;
        DelegateCommand _kick;
        DelegateCommand _disband;
        DelegateCommand _edit;
        DelegateCommand _contact;
        DelegateCommand _chat;

        /// <summary>
        /// Inicializa el modelo y se subscribe a los canales necesarios de <see cref="MessagingCenter"/>
        /// </summary>
        /// <param name="nav">Referencia a la navegación actual de la aplicación</param>
        /// <param name="guild">Gremio actual</param>
        public GuildProfileTabViewModel(INavigation nav, Guild guild) : base(nav)
        {
            this.guild = guild;
            MessagingCenter.Subscribe<object, string>(this, ULFG.Forms.App.GuildChangedKey, async (sender, e) => await HandleGuildChanged(e));
            _title = guild.Name;
        }

        /// <summary>
        /// Nombre del gremio. Se usa como título de la pantallla
        /// </summary>
        public string Title { get => _title; set => _title = value; }

        /// <summary>
        /// Comando para invitar a un usuario
        /// </summary>
        public ICommand Invite
        {
            get { return _invite = _invite ?? new DelegateCommand(async () => await Navigation.PushAsync(new UserSelectView(guild))); }
        }

        /// <summary>
        /// Comando para editar un gremio
        /// </summary>
        public ICommand Edit
        {
            get { return _edit = _edit ?? new DelegateCommand(async () => await Navigation.PushAsync(new EditGuildView(guild))); }
        }

        /// <summary>
        /// Comando para abandonar un gremio
        /// </summary>
        public ICommand Leave
        {
            get { return _kick = _kick ?? new DelegateCommand(async () => await LeaveGuild()); }
        }

        /// <summary>
        /// Comando para deshacer un gremio
        /// </summary>
        public ICommand Disband
        {
            get { return _disband = _disband ?? new DelegateCommand(async () => await DisbandCommand()); }
        }

        /// <summary>
        /// Comando para enviar un mensaje directo al lider del gremio
        /// </summary>
        public ICommand Contact
        {
            get { return _contact = _contact ?? new DelegateCommand(async () => await ContactCommand()); }
        }

        /// <summary>
        /// Comando para ir al chat grupal del gremio
        /// </summary>
        public ICommand Chat
        {
            get { return _chat = _chat ?? new DelegateCommand(async () => await Navigation.PushAsync(new GuildChatView(guild))); }
        }

        /// <summary>
        /// Acción del comando <seealso cref="Leave"/>
        /// </summary>
        async Task LeaveGuild()
        {
            await manager.LeaveGuild(actualUser.Id, guild.Id);
            MessagingCenter.Send<object,string>(this, ULFG.Forms.App.GuildChangedKey, "Delete " + guild.Id + " $");
            MessagingCenter.Send<object, string>(this, ULFG.Forms.App.GuildChangedKey, "Edit " + guild.Id);
            await Navigation.PopAsync();
        }

        /// <summary>
        /// Acción del comando <seealso cref="Contact"/>
        /// </summary>
        async Task ContactCommand()
        {
            User leader = await UserManager.DefaultManager.GetUserByIdAsync(guild.Leader, CrossConnectivity.Current.IsConnected);
            await Navigation.PushAsync(new SendMessageView(leader));
        }

        /// <summary>
        /// Acción del comando <seealso cref="Disband"/>
        /// </summary>
        async Task DisbandCommand()
        {
            var config = new ConfirmConfig()
            {
                Title = "Confirmar",
                Message = "¿Estas seguro de que deseas deshacer el gremio ? Esto borrara todos los mensajes y expulsará a " +
                      "todos los miembros. Esta acción no se puede deshacer",
                OkText = "Si, borrar el gremio",
                CancelText = "No, he cambiado de idea"
            };
            var answer = await UserDialogs.Instance.ConfirmAsync(config);
            if (answer)
            {
                var progress = UserDialogs.Instance.Progress(new ProgressDialogConfig() { Title = "Deshaciendo gremio" });
                Task.Run(() => GuildManager.DefaultManager.DeleteGuildAsync(guild)).Wait();
                MessagingCenter.Send<object, string>(this, ULFG.Forms.App.GuildChangedKey, "Delete " + guild.Id);
                progress.Hide();
                await Navigation.PopAsync();
            }
        }

        /// <summary>
        /// Gestiona los cambios en el gremio actual escuchando por un canal de <see cref="MessagingCenter"/>
        /// </summary>
        /// <param name="msg">Contenido del mensaje recibido del canal</param>
        async Task HandleGuildChanged(string msg)
        {
            var type = msg.Split(' ').ElementAt(0);
            var id = msg.Split(' ').ElementAt(1);
            if (type.Equals("Edit"))
            {
                guild = await GuildManager.DefaultManager.GetGuildByIdAsync(id);
                _title = guild.Name;
                RaisePropertyChanged("Title");
            }
        }
    }
}
