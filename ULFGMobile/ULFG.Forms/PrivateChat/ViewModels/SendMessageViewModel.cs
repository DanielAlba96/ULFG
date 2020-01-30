using Acr.UserDialogs;
using System.Threading.Tasks;
using System.Windows.Input;
using ULFG.Core.Data.Item;
using ULFG.Core.Logic;
using ULFG.Forms.Shared;
using Xamarin.Forms;

namespace ULFG.Forms.PrivateChat.ViewModels
{
    /// <summary>
    /// <see cref="ViewModelBase"/> que representa el modelo de la página de enviar mensaje
    /// </summary>
    public class SendMessageViewModel : ViewModelBase
    {
        /// <summary>
        /// Acceso a la lógica de mensajería
        /// </summary>
        readonly MessageOperations Manager = new MessageOperations();

        /// <summary>
        /// El usuario actual
        /// </summary>
        readonly User user = (User)Application.Current.Properties["user"];

        /// <summary>
        /// El usuario receptor
        /// </summary>
        readonly User receiver;

        string _message;

        DelegateCommand _send;

        /// <summary>
        /// Inicializa el modelo
        /// </summary>
        /// <param name="nav">Referencia a la navegación actual de la aplicación</param>
        /// <param name="receiver">Usuario receptor del mensaje</param>
        public SendMessageViewModel(INavigation nav, User receiver) : base(nav)
        {
            this.receiver = receiver;
            _message = string.Empty;
        }

        /// <summary>
        /// Contenido del mensaje
        /// </summary>
        public string Message { get => _message; set => _message = value; }

        /// <summary>
        /// Comando de enviar un mensaje
        /// </summary>
        public ICommand Send
        {
            get { return _send = _send ?? new DelegateCommand(async () => await SendCommand()); }
        }

        /// <summary>
        /// Acción del comando <seealso cref="Send"/>
        /// </summary>
        async Task SendCommand()
        {
            var progress = UserDialogs.Instance.Progress(new ProgressDialogConfig() { Title = "Enviando mensaje" });
            if (_message.Length < 1)
            {
                progress.Dispose();
                var config = new AlertConfig() { Title = "Error", Message = "Un mensaje privado no puede estar vacio" };
                await UserDialogs.Instance.AlertAsync(config);
                return;
            }
            var msg = Task.Run(() => { return Manager.SendMessage(user.Id, receiver.Id, _message); }).Result;
            MessagingCenter.Send<object, string>(this, ULFG.Forms.App.NewMessageKey, "Message Chat " + msg);
            progress.Dispose();
            await Navigation.PopToRootAsync();
        }
    }
}
