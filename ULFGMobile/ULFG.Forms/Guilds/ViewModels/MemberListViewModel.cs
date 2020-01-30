using Acr.UserDialogs;
using Plugin.Connectivity;
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

namespace ULFG.Forms.Guilds.ViewModels
{
    /// <summary>
    /// <see cref="ViewModelBase"/> que representa el modelo de la página con la lista de miembros de un gremio
    /// </summary>
    public class MemberListViewModel : ViewModelBase
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
        readonly Guild guild;

        ObservableCollection<User> _items;
        ICommand _tapped;

        /// <summary>
        /// Inicializa el modelo y se subscribe a los canales necesarios de <see cref="MessagingCenter"/>
        /// </summary>
        /// <param name="nav">Referencia a la navegación actual de la aplicación</param>
        /// <param name="guild">Gremio actual</param>
        public MemberListViewModel(INavigation nav, Guild guild) : base(nav)
        {
            var task = Task.Run(() => { return GuildMemberManager.DefaultManager.GetMembersByGuildIdAsync(guild.Id, CrossConnectivity.Current.IsConnected); });
            this.guild = guild;
            MessagingCenter.Subscribe<object, string>(this, ULFG.Forms.App.GuildMemberChangedKey, async (sender, e) => await HandleGuildMemberChanged(e));
            _items = task.Result;
        }

        /// <summary>
        /// Lista de miembros
        /// </summary>
        public ObservableCollection<User> Items { get => _items; set => _items = value; }

        /// <summary>
        /// Comando para el evento ItemTapped de un elemento de la lista
        /// </summary>
        public ICommand ItemTapped
        {
            get { return _tapped = _tapped ?? new Command<User>(async (u) => await ItemTappedCommand(u)); }
        }

        /// <summary>
        /// Acción del comando <seealso cref="ItemTapped"/>
        /// </summary>
        /// <param name="e">Usuario que corresponde al elemento seleccionado</param>
        async Task ItemTappedCommand(User e)
        {
            var user = (User)e;
            if (user.Id.Equals(actualUser.Id))
                return;
            if (guild.Leader.Equals(actualUser.Id))
            {
                string action = await UserDialogs.Instance.ActionSheetAsync(user.Nickname + " @ " + user.Username, "No hacer nada", null, null, "Ver perfil", "Expulsar");
                switch (action)
                {
                    case "Ver perfil":
                        await Navigation.PushAsync(new ExtProfileView(user));
                        break;
                    case "Expulsar":
                        var progress = UserDialogs.Instance.Progress(new ProgressDialogConfig() { Title = "Realizando expulsión" });
                        await manager.LeaveGuild(user.Id, guild.Id);
                        var aBorrar = _items.FirstOrDefault(x => x.Id == user.Id);
                        _items.Remove(aBorrar);
                        RaisePropertyChanged("Items");
                        MessagingCenter.Send<object, string>(this, ULFG.Forms.App.GuildChangedKey, "Edit " + guild.Id);
                        progress.Hide();
                        break;
                }
            }
            else
                await Navigation.PushAsync(new ExtProfileView(user));
        }

        /// <summary>
        /// Gestiona los cambios en los miembros escuchando por un canal de <see cref="MessagingCenter"/>
        /// </summary>
        /// <param name="msg">Contenido del mensaje recibido del canal</param>
        async Task HandleGuildMemberChanged(string msg)
        {
            var type = msg.Split(' ').ElementAt(0);
            var id = msg.Split(' ').ElementAt(2);
            if (type.Equals("New"))
            {
                var user = await UserManager.DefaultManager.GetUserByIdAsync(id);
                _items.Add(user);
                RaisePropertyChanged("Items");
            }
            else if (type.Equals("Delete"))
            {
                var user = _items.FirstOrDefault(x => x.Id == id);
                _items.Remove(user);
                RaisePropertyChanged("Items");
            }
            MessagingCenter.Send<object, string>(this, ULFG.Forms.App.GuildChangedKey, "Edit " + guild.Id);
        }
    }
}
