using Acr.UserDialogs;
using System.IO;
using System.Windows.Input;
using ULFG.Core;
using ULFG.Core.Data.Item;
using Xamarin.Forms;
using ULFG.Forms.Shared;
using System.Threading.Tasks;
using ULFG.Core.Data.ItemManager.Impl;

namespace ULFG.Forms.Publications.ViewModels
{
    /// <summary>
    /// <see cref="ViewModelBase"/> que representa el modelo de la página de crear publicaciones
    /// </summary>
    public class NewPublicationViewModel : ViewModelBase
    {
        /// <summary>
        /// Usuario actual
        /// </summary>
        readonly User user = (User)Application.Current.Properties["user"];

        ImageSource _source;
        string _message;
        byte[] attach = null;

        DelegateCommand _imageTapped;
        DelegateCommand _createAttach;
        DelegateCommand _savePublication;

        /// <summary>
        /// Inicializa el modelo
        /// </summary>
        /// <param name="nav">Referencia a la navegación actual de la aplicación</param>
        public NewPublicationViewModel(INavigation nav) : base(nav)
        {
            _message = string.Empty;
        }

        /// <summary>
        /// Representa el contenido de la imagen adjunta
        /// </summary>
        public ImageSource Source
        {
            set { _source = value; }
            get { return _source; }
        }

        /// <summary>
        /// Representa el texto de la publicación
        /// </summary>
        public string Message
        {
            set { _message = value; }
            get { return _message; }
        }

        /// <summary>
        /// Comando para cuando se toca la imagen
        /// </summary>
        public ICommand ImageTapped
        {
            get { return _imageTapped = _imageTapped ?? new DelegateCommand(async () => await ImageTappedCommand()); }
        }

        /// <summary>
        /// Comando para añadir un adjunto
        /// </summary>
        public ICommand CreateAttach
        {
            get { return _createAttach = _createAttach ?? new DelegateCommand(async () => await CreateAttachCommand()); }
        }

        /// <summary>
        /// Comando para crear publicación
        /// </summary>
        public ICommand SavePublication
        {
            get { return _savePublication = _savePublication ?? new DelegateCommand(async () => await SavePublicationCommand()); }
        }

        /// <summary>
        /// Acción del comando <seealso cref="SavePublication"/>
        /// </summary>
        /// <returns></returns>
        async Task SavePublicationCommand()
        {
            var progress = UserDialogs.Instance.Progress(new ProgressDialogConfig() { Title = "Creando publicacion" });
            if (_message.Length < 1)
            {
                progress.Dispose();
                var config = new AlertConfig() { Title = "Error", Message = "El texto de la publicacion no puede estar vacio" };
                await UserDialogs.Instance.AlertAsync(config);
                return;
            }

            Publication p = new Publication() { Text = _message, AutorId = user.Id };
            if (attach != null)
                p.Attachment = attach;

            await PublicationManager.DefaultManager.SavePublicationAsync(p);
            MessagingCenter.Send<object, string>(this, ULFG.Forms.App.PublicationChangedKey, "Add " + p.Id);
            progress.Dispose();
            await Navigation.PopAsync();
        }

        /// <summary>
        /// Acción del comando <seealso cref="ImageTapped"/>/>
        /// </summary>
        /// <returns></returns>
        async Task ImageTappedCommand()
        {
            if (_source != null)
            {
                var confirm = new ConfirmConfig() { Title = "Borrado", Message = "¿Desea borrar el archivo adjunto?", OkText = "Si", CancelText = "No" };
                var answer = await UserDialogs.Instance.ConfirmAsync(confirm);
                if (answer)
                {
                    attach = null;
                    _source = null;
                    RaisePropertyChanged("Source");
                }
            }
        }

        /// <summary>
        /// Acción del comando <seealso cref="CreateAttach"/>
        /// </summary>
        /// <returns></returns>
        async Task CreateAttachCommand()
        {
            var progress = UserDialogs.Instance.Progress(new ProgressDialogConfig() { Title = "Adjuntando imagen" });
            ImageHelper helper = new ImageHelper();
            attach = await helper.LoadImage();
            if (attach != null)
            {
                _source = ImageSource.FromStream(() => { return new MemoryStream(attach); });
                RaisePropertyChanged("Source");
            }
            progress.Dispose();
        }
    }
}


