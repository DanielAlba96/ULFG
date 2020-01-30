using Acr.UserDialogs;
using Plugin.Connectivity;
using System.Windows.Input;
using ULFG.Core.Logic;
using ULFG.Forms.PlatformInterfaces;
using Xamarin.Forms;
using System.Linq;
using System.Threading.Tasks;
using ULFG.Forms.Shared;
using ULFG.Core.Data.Item;

namespace ULFG.Forms.Login.ViewModels
{
    /// <summary>
    /// <see cref="ViewModelBase"/> que representa el modelo de la página de registro
    /// </summary>
    public class RegisterViewModel : ViewModelBase
    {
        /// <summary>
        /// Usuario actual
        /// </summary>
        readonly UserOperations Manager = new UserOperations();

        string _username;
        string _nickname;
        string _email;
        string _pass;
        string _repeatPass;

        private DelegateCommand _register;

        /// <summary>
        /// Inicializa el modelo
        /// </summary>
        /// <param name="nav">Referencia a la navegación de la aplicación</param>
        public RegisterViewModel(INavigation nav) : base(nav)
        {
            //No hace falta inicializar nada
        }

        /// <summary>
        /// Nombre de usuario
        /// </summary>
        public string Username { get => _username; set => _username = value; }

        /// <summary>
        /// Apodo
        /// </summary>
        public string Nickname { get => _nickname; set => _nickname = value; }

        /// <summary>
        /// Apodo
        /// </summary>
        public string Email { get => _email; set => _email = value; }

        /// <summary>
        /// Contraseña
        /// </summary>
        public string Pass { get => _pass; set => _pass = value; }
        /// <summary>
        /// Contraseña repetida
        /// </summary>
        public string RepeatPass { get => _repeatPass; set => _repeatPass = value; }

        /// <summary>
        /// Comando para registrar un usuario
        /// </summary>
        public ICommand Register
        {
            get { return _register = _register ?? new DelegateCommand(async () => await RegisterCommand()); }
        }

        /// <summary>
        /// Accion del comando <seealso cref="Register"/>
        /// </summary>
        /// <returns></returns>
        async Task RegisterCommand()
        {
            var progressConfig = new ProgressDialogConfig() { Title = "Realizando el registro" };
            var progress = UserDialogs.Instance.Progress(progressConfig);
            if (!CrossConnectivity.Current.IsConnected)
            {
                var config = new AlertConfig() { Title = "Sin conexion", Message = "No puedes registrarte en la aplicacion sin conexion a internet" };
                await UserDialogs.Instance.AlertAsync(config);
                progress.Dispose();
                await Navigation.PopAsync();
                return;
            }

            // Comprueba si todos los campos tienene valor
            if (string.IsNullOrEmpty(_username) ||string.IsNullOrEmpty(_nickname) || string.IsNullOrEmpty(_pass)
                || string.IsNullOrEmpty(_repeatPass) || string.IsNullOrEmpty(_email))
            {
                var config = new AlertConfig() { Title = "Error", Message = "Debes rellenar todos los campos" };
                await UserDialogs.Instance.AlertAsync(config);
                progress.Dispose();
                return;
            }
            // Se comprueba que ambos campos de contraseña coinciden
            if (!_pass.Equals(_repeatPass))
            {
                var config = new AlertConfig() { Title = "Error", Message = "Las constraseñas no coinciden" };
                await UserDialogs.Instance.AlertAsync(config);
                progress.Dispose();
                return;
            }
            // Valida el formato de todos los campos
            if (await ValidateFields())
            {
                var img = DependencyService.Get<IResourceManager>().GetBasicProfileImageAsByteArray();
                var user = await Manager.RegisterUser(_username, _nickname, _email, _pass, img);

                if (user != null)
                {
                    Application.Current.Properties["user"] = user;
                    progress.Title = "Preparando página inicial";
                    Page next = new MasterPageView();
                    progress.Dispose();
                    Application.Current.MainPage = next;                   
                }
                else
                {
                    var config = new AlertConfig() { Title = "Error", Message = "Ya hay un usuario registrado con ese nombre de usuario" };
                    await UserDialogs.Instance.AlertAsync(config);
                    progress.Dispose();
                }
            }
            else
                progress.Dispose();
        }
        /// <summary>
        /// Valida el formato de todos los campos
        /// </summary>
        /// <returns>False si hubo algún problema, True si todo esta correcto</returns>
        async Task<bool> ValidateFields()
        {

            if (_username.Length < 5)
            {
                var config = new AlertConfig() { Title = "Nombre de usuario incorrecto", Message = "El nombre de usuario debe tener al menos 5 caracteres" };
                await UserDialogs.Instance.AlertAsync(config);
                return false;
            }

            if (_nickname.Length < 3)
            {
                var config = new AlertConfig() { Title = "Apodo incorrecto", Message = "El apodo debe tener al menos 3 caracteres" };
                await UserDialogs.Instance.AlertAsync(config);
                return false;
            }

            if (!IsValidEmail(_email))
            {
                var config = new AlertConfig() { Title = "Email incorrecto", Message = "El formato del email debe ser del tipo nombre@dominio" };
                await UserDialogs.Instance.AlertAsync(config);
                return false;
            }

            if (!_pass.Any(char.IsNumber) || !_pass.Any(char.IsLower)  
                || !_pass.Any(char.IsUpper) || _pass.Length < 8)
            {
                var config = new AlertConfig()
                {
                    Title = "Contraseña incorrecta",
                    Message = "La contraseña debe tener al menos 8 caracteres y contener " +
                    "una letra minuscula, una letra mayuscula y un numero"
                };
                await UserDialogs.Instance.AlertAsync(config);
                return false;
            }
            return true;
        }

        /// <summary>
        /// Valida el formato del email
        /// </summary>
        /// <param name="email">El email</param>
        /// <returns>True si es correcto, False en caso contrario</returns>
        bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
    }
}
