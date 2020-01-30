using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ULFG.Forms.Login.Views;
using System.Threading.Tasks;
using Plugin.Connectivity;
using ULFG.Core.Data.ItemManager.Impl;
using ULFG.Core.Logic;

[assembly: XamlCompilation (XamlCompilationOptions.Compile)]
namespace ULFG.Forms
{
    /// <summary>
    /// <see cref="Application"/>
    /// </summary>
	public partial class App : Application
	{
        // Estas constantes se utilizan por el MessagingCenter para los canales de mensajería entre componentes
        #region MessagingKeys
        public const string NotificationReceivedKey = "NotificationReceived";
        public const string NewMessageKey = "NewMessage";
        public const string NewMessageScrollKey = "ScrollToLast";
        public const string NewGuildMessageKey = "NewGuildMessage";
        public const string GuildChangedKey = "GuildChanged";
        public const string GuildMemberChangedKey = "GuildMembersChanged";
        public const string NewChatKey = "NewChat";
        public const string ChatDeletedKey = "ChatDeleted";
        public const string UserChangedKey = "UserChanged";
        public const string PublicationChangedKey = "PublicationDeleted";
        public const string FollowsChanged = "FollowsChanged";
        public const string BlockChangedKey = "BlocksChanged";
        public const string GuildKickedKey = "GuildKicked";
        #endregion

        public App ()
		{
			InitializeComponent();

            //Ponemos toda la aplicación a escuchar por nuevas notificaciones
            MessagingCenter.Subscribe<object, string>(this, NotificationReceivedKey, (sender, e) => OnMessageReceived(e));

            //Si el usuario ya inició sesión va directamente a la página principal
            if (Settings.NHRegistrationID != string.Empty && Settings.LoggedUser != string.Empty)
            {
                var user = Task.Run(async () => { return await UserManager.DefaultManager.GetUserByIdAsync(Settings.LoggedUser, CrossConnectivity.Current.IsConnected); }).Result;
                // Si el usuario no inició nunca sesión ni la cerró va al login
                if (user == null)
                {
                    MainPage = new NavigationPage(new LoginView());
                    return;
                }
                Properties["user"] = user;
                MainPage = new MasterPageView();
            }
            else
                MainPage = new NavigationPage(new LoginView());           
        }
       
        /// <summary>
        /// Recibe las notificaciones nativas y las envía al canal adecuado de <see cref="MessagingCenter"/>
        /// </summary>
        /// <param name="data"></param>
        private void OnMessageReceived(string data)
        {
            var split = data.Split(' ');
            Console.WriteLine("Contenido: " + data);
            // Notificación de nuevo mensaje
            if (split[0].Equals("Message"))
            {
                // Nuevo mensaje de chat
                if (split[1].Equals("Chat")) // Message Chat $msgId
                    MessagingCenter.Send<object, string>(this, NewMessageKey, data);
                //Nuevo mensaje de gremio
                else // Message Guild $msgId
                    MessagingCenter.Send<object, string>(this, NewGuildMessageKey, data);
            }
            //Expulsión de gremio
            if (split[0].Equals("GuildKick"))
            {
                string id;
                //Si GuildKick $
                if (split[1].Equals("$"))
                    id = (Task.Run(() => { return GuildManager.DefaultManager.GetGuildByNameAsync(split[2]); }).Result).Id;
                //Si GuildKick userid
                else
                    id = split[1];
                MessagingCenter.Send<object, string>(this, GuildKickedKey, "GuildKick " + id);
            }
        }
    }
}
