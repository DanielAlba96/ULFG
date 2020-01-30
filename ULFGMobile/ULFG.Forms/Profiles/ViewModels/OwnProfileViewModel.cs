using Acr.UserDialogs;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Input;
using ULFG.Core.Data.Item;
using ULFG.Core.Data.ItemManager.Impl;
using ULFG.Forms.Profiles.Views;
using ULFG.Forms.Shared;
using Xamarin.Forms;

namespace ULFG.Forms.Profiles.ViewModels
{
    /// <summary>
    /// <see cref="ViewModelBase"/> que representa el modelo de la página de edición del perfil propio
    /// </summary>
    public class OwnProfileViewModel : ViewModelBase
    {
        /// <summary>
        /// Usuario actual
        /// </summary>
        readonly User user = (User)Application.Current.Properties["user"];
        readonly IProgressDialog progress;


        ImageSource _userIcon;
        string _username;
        string _nickname;
        string _bio;
        string _email;

        bool _isNicknameActive;
        bool _isBioActive;
        bool _isEmailActive;

        string _btnNickText;
        string _btnBioText;
        string _btnEmailText;

        Color _bioBackground;

        DelegateCommand _changeIcon;
        DelegateCommand _changeNick;
        DelegateCommand _changeBio;
        DelegateCommand _changeEmail;
        DelegateCommand _changePass;

        /// <summary>
        /// Inicializa el modelo
        /// </summary>
        /// <param name="nav">Referencia a la navegación actual de la aplicación</param>
        public OwnProfileViewModel(INavigation nav) : base(nav)
        {
            _userIcon = ImageSource.FromStream(() => new MemoryStream(user.Image));
            _username = user.Username;
            _nickname = user.Nickname;
            _bio = user.Bio;
            _email = user.Email;
            _btnBioText = "Editar";
            _btnEmailText = "Editar";
            _btnNickText = "Editar";
            _bioBackground = Color.FromHex("#E0E0E0");
            progress = (UserDialogs.Instance.Progress(new ProgressDialogConfig() { Title = "Actualizando datos", AutoShow = false }));
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
        /// Biografía
        /// </summary>
        public string Bio { get => _bio; set => _bio = value; }

        /// <summary>
        /// Email
        /// </summary>
        public string Email { get => _email; set => _email = value; }

        /// <summary>
        /// Indica si el campo de biografía está activo
        /// </summary>
        public bool IsBioActive { get => _isBioActive; set => _isBioActive = value; }

        /// <summary>
        /// Indica si el campo de apodo está activo
        /// </summary>
        public bool IsNicknameActive { get => _isNicknameActive; set => _isNicknameActive = value; }

        /// <summary>
        /// Indica si el campo de email está activo
        /// </summary>
        public bool IsEmailActive { get => _isEmailActive; set => _isEmailActive = value; }

        /// <summary>
        /// Texto del boton de apodo
        /// </summary>
        public string BtnNickText { get => _btnNickText; set => _btnNickText = value; }

        /// <summary>
        /// Texto del boton de biografía
        /// </summary>
        public string BtnBioText { get => _btnBioText; set => _btnBioText = value; }

        /// <summary>
        /// Texto del boton de email
        /// </summary>
        public string BtnEmailText { get => _btnEmailText; set => _btnEmailText = value; }

        /// <summary>
        /// Imagen de perfil
        /// </summary>
        public ImageSource UserIcon { get => _userIcon; set => _userIcon = value; }

        /// <summary>
        /// Color de fondo del campo biografía
        /// </summary>
        public Color BioBackground { get => _bioBackground; set => _bioBackground = value; }

        /// <summary>
        /// Comando para cambiar la imagen de perfil
        /// </summary>
        public ICommand ChangeIcon
        {
            get { return _changeIcon = _changeIcon ?? new DelegateCommand(async () => await ChangeIconCommand()); }
        }

        /// <summary>
        /// Comando para cambiar el apodo
        /// </summary>
        public ICommand ChangeNick
        {
            get { return _changeNick = _changeNick ?? new DelegateCommand(async () => await ChangeNickCommand()); }
        }

        /// <summary>
        /// Comando para cambiar la biografía
        /// </summary>
        public ICommand ChangeBio
        {
            get { return _changeBio = _changeBio ?? new DelegateCommand(async () => await ChangeBioCommand()); }
        }

        /// <summary>
        /// Comando para cambiar el email
        /// </summary>
        public ICommand ChangeEmail
        {
            get { return _changeEmail = _changeEmail ?? new DelegateCommand(async () => await ChangeEmailCommand()); }
        }

        /// <summary>
        /// Comando para cambiar la contraseña
        /// </summary>
        public ICommand ChangePass
        {
            get { return _changePass = _changePass ?? new DelegateCommand(async () => await Navigation.PushAsync(new ChangePasswordView())); }
        }

        /// <summary>
        /// Acción del comando <seealso cref="ChangeIcon"/>
        /// </summary>
        async Task ChangeIconCommand()
        {
            progress.Show();
            ImageHelper helper = new ImageHelper();
            user.Image = await helper.LoadImage();
            await UserManager.DefaultManager.SaveUserAsync(user);
            Application.Current.Properties["user"] = user;
            MessagingCenter.Send<object, string>(this, ULFG.Forms.App.UserChangedKey, "user changed");
            _userIcon = ImageSource.FromStream(() =>
            {
                return new MemoryStream(user.Image);
            });
            RaisePropertyChanged("UserIcon");
            progress.Hide();
        }

        /// <summary>
        /// Acción del comando <seealso cref="ChangeNick"/>
        /// </summary>
        async Task ChangeNickCommand()
        {
            progress.Show();
            if (_btnNickText.Equals("Ok"))
            {
                if (_nickname.Length >= 3)
                {
                    user.Nickname = _nickname;
                    await UserManager.DefaultManager.SaveUserAsync(user);
                    Application.Current.Properties["user"] = user;
                    MessagingCenter.Send<object, string>(this, ULFG.Forms.App.UserChangedKey, "user changed");
                }
                else
                {
                    var config = new AlertConfig() { Title = "Error", Message = "El apodo debe tener al menos 3 caracteres" };
                    await UserDialogs.Instance.AlertAsync(config);
                    _nickname = user.Nickname;
                }
                _btnNickText = "Editar";
                _isNicknameActive = false;
                RaisePropertyChanged("IsNicknameActive");
                RaisePropertyChanged("BtnNickText");
                RaisePropertyChanged("Nickname");
            }
            else
            {
                _isNicknameActive = true;
                _btnNickText = "Ok";
                RaisePropertyChanged("BtnNickText");
                RaisePropertyChanged("IsNicknameActive");
            }
            progress.Hide();
        }

        /// <summary>
        /// Acción del comando <seealso cref="ChangeBio"/>
        /// </summary>
        async Task ChangeBioCommand()
        {
            progress.Show();
            if (_btnBioText.Equals("Ok"))
            {
                user.Bio = _bio;
                await UserManager.DefaultManager.SaveUserAsync(user);
                Application.Current.Properties["user"] = user;
                MessagingCenter.Send<object, string>(this, ULFG.Forms.App.UserChangedKey, "user changed");
                _isBioActive = false;
                _bioBackground = Color.FromHex("#E0E0E0");
                _btnBioText = "Editar";
                RaisePropertyChanged("BtnBioText");
                RaisePropertyChanged("IsBioActive");
                RaisePropertyChanged("Bio");
                RaisePropertyChanged("BioBackground");
            }
            else
            {
                _isBioActive = true;
                _btnBioText = "Ok";
                _bioBackground = Color.White;
                RaisePropertyChanged("IsBioActive");
                RaisePropertyChanged("BtnBioText");
                RaisePropertyChanged("BioBackground");
            }
            progress.Hide();
        }

        /// <summary>
        /// Acción del comando <seealso cref="ChangeEmail"/>
        /// </summary>
        async Task ChangeEmailCommand()
        {
            progress.Show();
            if (_btnEmailText.Equals("Ok"))
            {
                if (IsValidEmail(_email))
                {
                    user.Email = _email;
                    await UserManager.DefaultManager.SaveUserAsync(user);
                    Application.Current.Properties["user"] = user;
                    MessagingCenter.Send<object, string>(this, ULFG.Forms.App.UserChangedKey, "user changed");
                }
                else
                {
                    var config = new AlertConfig() { Title = "Error", Message = "El email debe tener un formato valido (nombre@dominio)" };
                    await UserDialogs.Instance.AlertAsync(config);
                    _email = user.Email;
                }
                _isEmailActive = false;
                _btnEmailText = "Editar";
                RaisePropertyChanged("BtnEmailText");
                RaisePropertyChanged("IsEmailActive");
                RaisePropertyChanged("Email");
            }
            else
            {
                _isEmailActive = true;
                _btnEmailText = "Ok";
                RaisePropertyChanged("BtnEmailText");
                RaisePropertyChanged("IsEmailActive");
            }
            progress.Hide();
        }

        /// <summary>
        /// Comprueba si el formato de un email es válido
        /// </summary>
        /// <param name="email">El email</param>
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
