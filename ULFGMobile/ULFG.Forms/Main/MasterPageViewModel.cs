using System.IO;
using ULFG.Core.Data.Item;
using ULFG.Forms.Shared;
using Xamarin.Forms;

namespace ULFG.Forms
{
    /// <summary>
    /// <see cref="ViewModelBase"/> que representa el modelo del menú de navegación
    /// </summary>
    public class MasterPageViewModel : ViewModelBase
    {
        string _user;
        ImageSource _usericon;

        /// <summary>
        /// Usuario en sesión
        /// </summary>
        User actual;

        /// <summary>
        /// Inicializa el modelo y se subscribe a los canales necesarios de <see cref="MessagingCenter"/>
        /// </summary>
        /// <param name="nav">Referencia a la navegación actual de la aplicación</param>
        public MasterPageViewModel(INavigation nav) : base(nav)
        {
            Initialize();

            MessagingCenter.Subscribe<object, string>(this, ULFG.Forms.App.UserChangedKey, (sender, e) =>
             {
                 Initialize();
                 RaisePropertyChanged("User");
                 RaisePropertyChanged("UserIcon");
             });
        }

        void Initialize()
        {
            actual = (User)Application.Current.Properties["user"];
            _user = actual.Nickname + "@" + actual.Username;
            _usericon = ImageSource.FromStream(()=> new MemoryStream(actual.Image));
        }

        /// <summary>
        /// Imagen de perfil de usuario
        /// </summary>
        public ImageSource UserIcon
        {
            get { return _usericon; }
            set { _usericon = value; }
        }

        /// <summary>
        /// Usuario en sesión
        /// </summary>
        public string User
        {
            get { return _user; }
            set { _user = value; }
        }
    }
}
