using ULFG.Core.Data.Item;
using Xamarin.Forms;
using Acr.UserDialogs;
using System.Threading.Tasks;
using System.IO;
using System.Linq;
using ULFG.Core.Data.ItemManager.Impl;
using ULFG.Forms.Shared;

namespace ULFG.Forms.Guilds.ViewModels
{
    /// <summary>
    /// <see cref="ViewModelBase"/> que representa el modelo de la página de detalle de un gremio
    /// </summary>
    public class GuildInfoViewModel : ViewModelBase
    {
        /// <summary>
        /// Gremio actual
        /// </summary>
        Guild guild;

        ImageSource _source;
        string _msg;
        string _members;
        int counter;

        /// <summary>
        /// Inicializa las variables y se subscribe a los canales de <see cref="MessagingCenter"/>
        /// </summary>
        /// <param name="nav"></param>
        /// <param name="guild"></param>
        public GuildInfoViewModel(INavigation nav, Guild guild) : base(nav)
        {
            var task = Task.Run(() => { return GuildMemberManager.DefaultManager.GetNumberOfMembers(guild.Id); });
            this.guild = guild;
            _source = ImageSource.FromStream(() => new MemoryStream(guild.Image));
            _msg = guild.Message;
            counter = task.Result;
            _members = "Numero de miembros: " + counter;
            MessagingCenter.Subscribe<object, string>(this, ULFG.Forms.App.GuildChangedKey, async (sender, e) => await HandleGuildChanged(sender, e));
            MessagingCenter.Subscribe<object, string>(this, ULFG.Forms.App.GuildMemberChangedKey, (sender, e) => HandleGuildMemberChanged(e));
        }

        /// <summary>
        /// Imagen del gremio
        /// </summary>
        public ImageSource Source { get => _source; set => _source = value; }

        /// <summary>
        /// Mensaje fijado del gremio
        /// </summary>
        public string Msg { get => _msg; set => _msg = value; }

        /// <summary>
        /// Número de miembros del gremio
        /// </summary>
        public string Members { get => _members; set => _members = value; }

        /// <summary>
        /// Gestiona las modificaciones en los datos del gremio actual recibidas a través de un canal de <see cref="MessagingCenter"/>
        /// </summary>
        /// <param name="sender">El componenente emisor</param>
        /// <param name="msg">Contenido del mensaje recibido del canal</param>
        async Task HandleGuildChanged(object sender, string msg)
        {
            var type = msg.Split(' ').ElementAt(0);
            var id = msg.Split(' ').ElementAt(1);
            if (sender is GuildInfoViewModel)
                return;
            if (type.Equals("Edit"))
            {
                guild = await GuildManager.DefaultManager.GetGuildByIdAsync(id);
                _source = ImageSource.FromStream(() => new MemoryStream(guild.Image));
                _msg = guild.Message;
                RaisePropertyChanged("Source");
                RaisePropertyChanged("Msg");
            }
        }

        /// <summary>
        /// Gestiona los cambios en el número de miembros recibidos a escuchando a través de un canal de <see cref="MessagingCenter"/>
        /// </summary>
        /// <param name="msg">Contenido del mensaje recibido del canal</param>
        void HandleGuildMemberChanged(string msg)
        {
            var type = msg.Split(' ').ElementAt(0);
            if (type.Equals("New"))
            {
                counter++;
                _members = "Número de miembros: " + counter;
                RaisePropertyChanged("Members");
            }
            else if (type.Equals("Delete"))
            {
                counter--;
                _members = "Número de miembros: " + counter;
                RaisePropertyChanged("Members");
            }
        }

        /// <summary>
        /// Gestiona el caso en el que el usuario actual es expulsado del gremio escuchando a través de un canal de <see cref="MessagingCenter"/>
        /// </summary>
        /// <param name="msg">Contenido del mensaje recibido del canal</param>
        public void HandleGuildKick(string msg)
        {
            if (!Navigation.NavigationStack.Last().Equals(this))
                return;
            Device.BeginInvokeOnMainThread(async () =>
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
