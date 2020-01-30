using Acr.UserDialogs;
using Plugin.Connectivity;
using System.Threading.Tasks;
using System.Windows.Input;
using ULFG.Core.Data.Item;
using ULFG.Core.Logic;
using ULFG.Forms.PlatformInterfaces;
using ULFG.Forms.Shared;
using Xamarin.Forms;

namespace ULFG.Forms.Guilds.ViewModels
{
    /// <summary>
    /// <see cref="ViewModelBase"/> que representa el modelo de la página de creación de un gremio
    /// </summary>
    public class NewGuildViewModel : ViewModelBase
    {
        /// <summary>
        /// Acceso a la lógica de gremios
        /// </summary>
        readonly GuildOperations manager = new GuildOperations();
        /// <summary>
        /// Usuario actual
        /// </summary>
        readonly User User = (User)Application.Current.Properties["user"];

        string _name;
        string _desc;
        bool _visibility;

        DelegateCommand _create;

        /// <summary>
        /// Inicializa el modelo
        /// </summary>
        /// <param name="nav">Referencia a la navegación actual de la aplicación</param>
        public NewGuildViewModel(INavigation nav) : base(nav)
        {
            _name = string.Empty;
            _desc = string.Empty;
        }

        /// <summary>
        /// Nombre del gremio
        /// </summary>
        public string Name { get => _name; set => _name = value; }

        /// <summary>
        /// Descripción del gremio
        /// </summary>
        public string Desc { get => _desc; set => _desc = value; }

        /// <summary>
        /// Indica si el gremio es público o privado
        /// </summary>
        public bool Visibility { get => _visibility; set => _visibility = value; }

        /// <summary>
        /// Comando para crear un gremio
        /// </summary>
        public ICommand Create
        {
            get { return _create = _create ?? new DelegateCommand(async()=> await CreateCommand()); }
        }

        /// <summary>
        /// Acción del comando <seealso cref="Create"/>
        /// </summary>
        async Task CreateCommand()
        {
            if (!string.IsNullOrEmpty(_name))
            {
                if (_name.Length < 4)
                {
                    var config = new AlertConfig() { Title = "Error", Message = "El nombre debe tener como minimo 4 caracteres" };
                    await UserDialogs.Instance.AlertAsync(config);
                    return;
                }
                var progress = UserDialogs.Instance.Progress(new ProgressDialogConfig() { Title = "Creando gremio" });
                if (!CrossConnectivity.Current.IsConnected)
                {
                    var config = new AlertConfig() { Title = "Sin conexion", Message = "No puedes crear un gremio sin conexión a internet" };
                    await UserDialogs.Instance.AlertAsync(config);
                    progress.Hide();
                    await Navigation.PopAsync();
                    return;
                }
                var bytes = DependencyService.Get<IResourceManager>().GetBasicGuildImageAsByteArray();
                var guild = await manager.CreateGuild(_name, _desc, User.Id, _visibility, bytes);
                MessagingCenter.Send<object, string>(this, ULFG.Forms.App.GuildChangedKey, "New " + guild.Id);
                progress.Hide();
                await Navigation.PopAsync();
            }
            else
            {
                var config = new AlertConfig() { Title = "Error", Message = "El campo nombre es obligatorio" };
                await UserDialogs.Instance.AlertAsync(config);
            }
        }
    }
}
