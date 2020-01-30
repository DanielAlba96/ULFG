using ULFG.Core.Data.Item;
using ULFG.Core.Logic;
using Xamarin.Forms;
using Acr.UserDialogs;
using System.IO;
using System.Windows.Input;
using System.Threading.Tasks;
using ULFG.Forms.PrivateChat.Views;
using Plugin.Connectivity;
using ULFG.Core.Data.ItemManager.Impl;
using ULFG.Forms.Shared;

namespace ULFG.Forms.Profiles.ViewModels
{
    /// <summary>
    /// <see cref="ViewModelBase"/> que representa el modelo de la página del perfil de un usuario
    /// </summary>
    public class ExtProfileViewModel : ViewModelBase
    {
        /// <summary>
        /// Acceso a la lógica de interacciones
        /// </summary>
        readonly SocialOperations Manager = new SocialOperations();
        /// <summary>
        /// El usuario actual
        /// </summary>
        readonly User actual = (User)Application.Current.Properties["user"];

        /// <summary>
        /// El usuario cuyo perfil se esta viendo
        /// </summary>
        readonly User ext;
        readonly IProgressDialog progress;

        string _userInfo;
        string _userBio;
        string _followText;
        string _blockText;
        bool _followEnabled;
        ImageSource _userIcon;
        int _blockWidth;
        int _followWidth;

        Block extBlock;

        DelegateCommand _follow;
        DelegateCommand _block;
        DelegateCommand _sendMsg;

        /// <summary>
        /// Inicializa el modelo
        /// </summary>
        /// <param name="nav">Referencia a la navegación actual de la aplicación</param>
        /// <param name="user">El usuario al que pertenece el perfil</param>
        public ExtProfileViewModel(INavigation nav, User user) : base(nav)
        {
            this.ext = user;
            var task = Task.Run(() => DetermineText());
            _userIcon = ImageSource.FromStream(() => new MemoryStream(ext.Image));
            _userBio = ext.Bio;
            _userInfo = ext.Nickname + "@" + ext.Username;
            progress = UserDialogs.Instance.Progress(new ProgressDialogConfig() { Title = "Espera", AutoShow = false });
            task.Wait();
        }

        /// <summary>
        /// Texto del botón de bloquear
        /// </summary>
        public string BlockText { get => _blockText; set => _blockText = value; }

        /// <summary>
        /// Tamaño del botón de bloquear
        /// </summary>
        public int BlockWidth { get => _blockWidth; set => _blockWidth = value; }

        /// <summary>
        /// Texto del botón de seguir
        /// </summary>
        public string FollowText { get => _followText; set => _followText = value; }

        /// <summary>
        /// Tamaño del botón de seguir
        /// </summary>
        public int FollowWidth { get => _followWidth; set => _followWidth = value; }

        /// <summary>
        /// Biografía del usuario
        /// </summary>
        public string UserBio { get => _userBio; set => _userBio = value; }

        /// <summary>
        /// Nombre de usuario y apodo del dueño del perfil
        /// </summary>
        public string UserInfo { get => _userInfo; set => _userInfo = value; }

        /// <summary>
        /// Indica si el botón de seguir está habilitado
        /// </summary>
        public bool FollowEnabled { get => _followEnabled; set => _followEnabled = value; }

        /// <summary>
        /// Imagen de perfil del usuario
        /// </summary>
        public ImageSource UserIcon { get => _userIcon; set => _userIcon = value; }

        /// <summary>
        /// Comando para seguir a un usuario
        /// </summary>
        public ICommand Follow
        {
            get { return _follow = _follow ?? new DelegateCommand(async () => await FollowCommand()); }
        }

        /// <summary>
        /// Comando para bloquear a un usuario
        /// </summary>
        public ICommand Block
        {
            get { return _block = _block ?? new DelegateCommand(async () => await BlockCommand()); }
        }

        /// <summary>
        /// Comando para enviar un mensaje
        /// </summary>
        public ICommand SendMsg
        {
            get { return _sendMsg = _sendMsg ?? new DelegateCommand(async () => await SendCommand()); }
        }

        /// <summary>
        /// Acción del comando <seealso cref="SendCommand"/>
        /// </summary>
        async Task SendCommand()
        {
            if (extBlock == null)
                await Navigation.PushAsync(new SendMessageView(ext));
            else
            {
                var config = new AlertConfig() { Title = "Bloqueo", Message = "No puedes mandar un mensaje a un usuario que te esta bloqueando" };
                await UserDialogs.Instance.AlertAsync(config);
            }
        }

        /// <summary>
        /// Acción del comando <seealso cref="FollowCommand"/>
        /// </summary>
        async Task FollowCommand()
        {
            progress.Show();
            if (_followText.Equals("Seguir"))
            {
                if (await Manager.FollowUser(actual.Id, ext.Id))
                {
                    _followText = "Dejar de seguir";
                    _followWidth = 150;
                    RaisePropertyChanged("FollowText");
                    RaisePropertyChanged("FollowWidth");
                    MessagingCenter.Send<object, string>(this, ULFG.Forms.App.FollowsChanged, "Add " + ext.Id);
                }
                else
                {
                    var config = new AlertConfig()
                    {
                        Title = "No se puede seguir al usuario",
                        Message = "El usuario seleccionado te esta bloqueando y no puedes seguirle"
                    };
                    await UserDialogs.Instance.AlertAsync(config);
                }
            }
            else
            {
                await Manager.UnfollowUser(actual.Id, ext.Id);
                _followText = "Seguir";
                _followWidth = 80;
                RaisePropertyChanged("FollowText");
                RaisePropertyChanged("FollowWidth");
                MessagingCenter.Send<object, string>(this, ULFG.Forms.App.FollowsChanged, "Delete " + ext.Id);
            }
            progress.Hide();
        }

        /// <summary>
        /// Acción del comando <seealso cref="BlockCommand"/>
        /// </summary>
        async Task BlockCommand()
        {
            progress.Show();
            if (_blockText.Equals("Bloquear"))
            {
                await Manager.BlockUser(actual.Id, ext.Id);
                _blockText = "Desbloquear";
                _blockWidth = 120;
                _followText = "Seguir";
                _followWidth = 80;
                _followEnabled = false;
                RaisePropertyChanged("BlockText");
                RaisePropertyChanged("BlockWidth");
                RaisePropertyChanged("FollowText");
                RaisePropertyChanged("FollowWidth");
                RaisePropertyChanged("FollowEnabled");
                MessagingCenter.Send<object, string>(this, ULFG.Forms.App.FollowsChanged, "Delete " + ext.Id);
                MessagingCenter.Send<object, string>(this, ULFG.Forms.App.BlockChangedKey, "Add " + ext.Id);
            }
            else
            {
                await Manager.UnblockUser(actual.Id, ext.Id);
                _blockText = "Bloquear";
                _blockWidth = 100;
                if (extBlock == null)
                {
                    _followEnabled = true;
                    RaisePropertyChanged("FollowEnabled");
                }
                RaisePropertyChanged("BlockText");
                RaisePropertyChanged("BlockWidth");
                MessagingCenter.Send<object, string>(this, ULFG.Forms.App.BlockChangedKey, "Delete " + ext.Id);
            }
            progress.Hide();
        }
        /// <summary>
        /// Determina el texto de cada botón en función del estado de los seguimientos entre el usuario
        /// logeado y el dueño del perfil
        /// </summary>
        void DetermineText()
        {
            var taskFollow = Task.Run(() => { return FollowManager.DefaultManager.GetFollowsByBothSidesAsync(actual.Id, ext.Id, CrossConnectivity.Current.IsConnected); });
            var taskBlock = Task.Run(() => { return BlockManager.DefaultManager.GetBlocksByBothSidesAsync(actual.Id, ext.Id, CrossConnectivity.Current.IsConnected); });
            var taskExtBlock = Task.Run(() => { return BlockManager.DefaultManager.GetBlocksByBothSidesAsync(ext.Id, actual.Id, CrossConnectivity.Current.IsConnected); });

            var follow = taskFollow.Result;
            var block = taskBlock.Result;
            extBlock = taskExtBlock.Result;

            if (extBlock != null)
            {
                _followText = "Seguir";
                _followWidth = 80;
                _followEnabled = false;
            }
            else
            {
                _followEnabled = true;
                if (follow != null)
                {
                    _followText = "Dejar de seguir";
                    _followWidth = 150;
                }
                else
                {
                    _followText = "Seguir";
                    _followWidth = 80;
                }
            }

            if (block != null)
            {
                _blockText = "Desbloquear";
                _blockWidth = 120;
                _followEnabled = false;
            }
            else
            {
                _blockText = "Bloquear";
                _blockWidth = 100;
            }
        }
    }
}
