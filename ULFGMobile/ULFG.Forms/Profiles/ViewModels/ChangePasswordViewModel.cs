using Acr.UserDialogs;
using System.Linq;
using System.Threading.Tasks;
using ULFG.Core.Data.Item;
using ULFG.Core.Logic;
using ULFG.Forms.Shared;
using Xamarin.Forms;

namespace ULFG.Forms.Profiles.ViewModels
{
    /// <summary>
    /// <see cref="ViewModelBase"/> que representa el modelo de la página de cambio de contraseña
    /// </summary>
    public class ChangePasswordViewModel : ViewModelBase
    {
        /// <summary>
        /// Acceso a la lógica de usuarios
        /// </summary>
        readonly UserOperations Manager = new UserOperations();
        /// <summary>
        /// Representa el usuario actual
        /// </summary>
        readonly User user = (User)Application.Current.Properties["user"];

        string _oldPass;
        string _newPass;
        string _repeatPass;

        DelegateCommand _save;

        /// <summary>
        /// Inicializa el modelo
        /// </summary>
        /// <param name="nav">Referencia a la navegación actual de la aplicación</param>
        public ChangePasswordViewModel(INavigation nav) : base(nav)
        {
            //No se necesita inicializar nada aquí
        }

        /// <summary>
        /// Contraseña antigua
        /// </summary>
        public string OldPass { get => _oldPass; set => _oldPass = value; }
        
        /// <summary>
        /// Contraseña nueva
        /// </summary>
        public string NewPass { get => _newPass; set => _newPass = value; }
        
        /// <summary>
        /// Repetir contraseña
        /// </summary>
        public string RepeatPass { get => _repeatPass; set => _repeatPass = value; }

        /// <summary>
        /// Comando para guardar los cambios
        /// </summary>
        public System.Windows.Input.ICommand Save
        {
            get { return _save = _save ?? new DelegateCommand(async () => await SaveCommand()); }
        }

        /// <summary>
        /// Acción del comando <seealso cref="Save"/>
        /// </summary>
        async Task SaveCommand()
        {
            var progress = UserDialogs.Instance.Progress(new ProgressDialogConfig() { Title = "Cambiando contraseña" });
            if (!_newPass.Equals(_repeatPass))
            {
                var config = new AlertConfig()
                {
                    Title = "Error",
                    Message = "Las contraseñas no coinciden"
                };
                progress.Dispose();
                await UserDialogs.Instance.AlertAsync(config);
                return;
            }
            if (!_newPass.Any(char.IsNumber) || !_newPass.Any(char.IsLower)
              || !_newPass.Any(char.IsUpper) || _newPass.Length < 8)
            {
                var config = new AlertConfig()
                {
                    Title = "Error",
                    Message = "La nueva contraseña no cumple los requisitos de seguridad. Se necesita una " +
                    "longuitud de entre 8 y 16 caracteres. Además debe contener al menos una letra minuscula, una mayuscula y un numero"
                };
                progress.Dispose();
                await UserDialogs.Instance.AlertAsync(config);
                return;
            }

            if (await Manager.ChangePassword(user.Id, _oldPass, _newPass))
            {
                progress.Dispose();
                await Navigation.PopAsync();
            }
            else
            {
                progress.Dispose();
                var config = new AlertConfig()
                {
                    Title = "Error",
                    Message = "La contraseña actual es incorrecta"
                };
                await UserDialogs.Instance.AlertAsync(config);
            }
        }
    }
}
