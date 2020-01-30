using Acr.UserDialogs;
using Plugin.Connectivity;
using System.Threading.Tasks;
using System.Windows.Input;
using ULFG.Core.Logic;
using ULFG.Forms.Login.Views;
using ULFG.Forms.PlatformInterfaces;
using ULFG.Forms.Shared;
using Xamarin.Forms;

namespace ULFG.Forms.Login.ViewModels
{
    /// <summary>
    /// <see cref="ViewModelBase"/> que representa el moelo de la página de login
    /// </summary>
    public class LoginViewModel : ViewModelBase
    {
        /// <summary>
        /// Usuario actual
        /// </summary>
        readonly UserOperations Manager = new UserOperations();

        string _username;
        string _password;

        DelegateCommand _login;
        DelegateCommand _register;

        /// <summary>
        /// Inicializa el modelo
        /// </summary>
        /// <param name="nav">Referencia a la navegación de la aplicación</param>
        public LoginViewModel(INavigation nav) : base(nav)
        {
            //No hace falta inicializar nada aquí
        }

        /// <summary>
        /// Nombre de usuario
        /// </summary>
        public string Username { get => _username; set => _username = value; }

        /// <summary>
        /// Apodo
        /// </summary>
        public string Password { get => _password; set => _password = value; }

        /// <summary>
        /// Comando para entrar en sesión
        /// </summary>
        public ICommand Login
        {
            get { return _login = _login ?? new DelegateCommand(async () => await LoginCommand()); }
        }

        /// <summary>
        /// Comando para ir a la pantalla de registro
        /// </summary>
        public ICommand Register
        {
            get { return _register = _register ?? new DelegateCommand(async () => await RegisterCommand()); }
        }

        /// <summary>
        /// Acción del comando <seealso cref="Login"/>
        /// </summary>
        async Task LoginCommand()
        {
            if (!string.IsNullOrEmpty(_username) && !string.IsNullOrEmpty(_password))
            {
                var config = new ProgressDialogConfig() { Title = "Validando credenciales" };
                var progress = UserDialogs.Instance.Progress(config);
                if (!CrossConnectivity.Current.IsConnected)
                {
                    var alertConfig = new AlertConfig() { Title = "Sin conexion", Message = "No puedes realizar un nuevo inicio de sesión sin conexion a internet" };
                    await UserDialogs.Instance.AlertAsync(alertConfig);
                    progress.Dispose();
                    return;
                }
                var token = DependencyService.Get<INotificationManager>().GetNativePNSToken();
                var platform = DependencyService.Get<INotificationManager>().GetNativePNSName();
                var user = await Manager.LoginUser(_username, _password, token, platform);
                if (user == null)
                {
                    var alertConfig = new AlertConfig() { Title = "Error", Message = "Datos incorrectos o usuario inexistente" };
                    await UserDialogs.Instance.AlertAsync(alertConfig);
                    progress.Dispose();
                    return;
                }
                Settings.LoggedUser = user.Id;
                Application.Current.Properties["user"] = user;
                var page = new MasterPageView();
                Application.Current.MainPage = page;
                progress.Dispose();
            }
            else
            {
                var alertConfig = new AlertConfig() { Title = "Error", Message = "Debes rellenar todos los campos" };
                await UserDialogs.Instance.AlertAsync(alertConfig);
            }
        }

        /// <summary>
        /// Acción del comando <seealso cref="Register"/>
        /// </summary>
        async Task RegisterCommand()
        {
            await Navigation.PushAsync(new RegisterView());
        }
    }
}
