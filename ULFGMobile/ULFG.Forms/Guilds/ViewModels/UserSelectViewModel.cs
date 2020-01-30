using Acr.UserDialogs;
using Plugin.Connectivity;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using ULFG.Core.Data.Item;
using ULFG.Core.Data.ItemManager.Impl;
using ULFG.Forms.Profiles.Views;
using ULFG.Forms.Shared;
using Xamarin.Forms;

namespace ULFG.Forms.Guilds.ViewModels
{
    /// <summary>
    /// <see cref="ViewModelBase"/> que representa el modelo de la página de selección de usuarios a invitar
    /// </summary>
    public class UserSelectViewModel : ViewModelBase
    {
        /// <summary>
        /// Gremio actual
        /// </summary>
        readonly Guild guild;

        ObservableCollection<User> _items;
        ICommand _invite;

        /// <summary>
        /// Inicializa el modelo
        /// </summary>
        /// <param name="nav">Referencia a la navegación actual de la aplicación</param>
        /// <param name="guild">Gremio actual</param>
        public UserSelectViewModel(INavigation nav, Guild guild) : base(nav)
        {
            this.guild = guild;
            _items = Task.Run(async () => { return await GuildMemberManager.DefaultManager.GetNoMembersByGuildIdAsync(guild.Id, CrossConnectivity.Current.IsConnected); }).Result;
        }

        /// <summary>
        /// Lista de usuarios que no pertenecen al gremio
        /// </summary>
        public ObservableCollection<User> Items { get => _items; set => _items = value; }

        /// <summary>
        /// Comando para el evento ItemTapped de un elemento de la lista
        /// </summary>
        public ICommand Tapped
        {
            get { return _invite = _invite ?? new Command<User>(async (u) => await ItemTapped(u)); }
        }

        /// <summary>
        /// Acción del comando <seealso cref="Tapped"/>
        /// </summary>
        /// <param name="user">El usuario seleccionado</param>
        async Task ItemTapped(User user)
        {
            String action = await UserDialogs.Instance.ActionSheetAsync("Usuario: " + user.Nickname, "No hacer nada", null, null, "Ver Perfil", "Invitar");
            if (action != null)
                if (action.Equals("Invitar"))
                {
                    var progress = UserDialogs.Instance.Progress(new ProgressDialogConfig() { Title = "Espera" });
                    var member = new GuildMember
                    {
                        GuildId = guild.Id,
                        MemberId = user.Id
                    };
                    await GuildMemberManager.DefaultManager.SaveGuildMemberAsync(member);
                    _items.Remove(user);
                    var config = new AlertConfig() { Title = "Usuario invitado", Message = "Se ha invitado al usuario " + user.Nickname + " al gremio " + guild.Name };
                    progress.Hide();
                    await UserDialogs.Instance.AlertAsync(config);
                    MessagingCenter.Send<object, string>(this, ULFG.Forms.App.GuildMemberChangedKey, "New " + guild.Id + " " + user.Id);
                    RaisePropertyChanged("Items");
                }
                else if (action.Equals("Ver Perfil"))
                    await Navigation.PushAsync(new ExtProfileView(user));
        }
    }
}
