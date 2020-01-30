using FFImageLoading.Forms;
using FFImageLoading.Forms.WinUWP;
using System;
using System.Collections.Generic;
using System.Reflection;
using ULFG.UWP.PlatformImpl;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Networking.PushNotifications;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace ULFG.UWP
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    sealed partial class App : Application
    {
        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            this.InitializeComponent();
            this.Suspending += OnSuspending;
        }

        /// <summary>
        /// Invoked when the application is launched normally by the end user.  Other entry points
        /// will be used such as when the application is launched to open a specific file.
        /// </summary>
        /// <param name="e">Details about the launch request and process.</param>
        protected async override void OnLaunched(LaunchActivatedEventArgs e)
        {

            Frame rootFrame = Window.Current.Content as Frame;

            // Do not repeat app initialization when the Window already has content,
            // just ensure that the window is active
            if (rootFrame == null)
            {
                // Create a Frame to act as the navigation context and navigate to the first page
                rootFrame = new Frame();

                rootFrame.NavigationFailed += OnNavigationFailed;

                var assembliesToInclude = new List<Assembly>()
                {
                    typeof(CachedImage).GetTypeInfo().Assembly,
                    typeof(CachedImageRenderer).GetTypeInfo().Assembly
                };

                Xamarin.Forms.Forms.Init(e, assembliesToInclude);
                CachedImageRenderer.Init();

                //Se registran las implementaciones de la inyección de dependencias
                Xamarin.Forms.DependencyService.Register<UWPResourceManager>();
                Xamarin.Forms.DependencyService.Register<UWPNotificationManager>();

                // Place the frame in the current Window
                Window.Current.Content = rootFrame;

                var channel = await PushNotificationChannelManager.CreatePushNotificationChannelForApplicationAsync();
                channel.PushNotificationReceived += OnPushNotificationReceived;

                UWPNotificationManager.Token = channel.Uri;
            }

            if (rootFrame.Content == null)
            {
                // When the navigation stack isn't restored navigate to the first page,
                // configuring the new page by passing required information as a navigation
                // parameter
                rootFrame.Navigate(typeof(MainPage), e.Arguments);
            }
            // Ensure the current window is active
            Window.Current.Activate();
        }

        /// <summary>
        /// Invoked when Navigation to a certain page fails
        /// </summary>
        /// <param name="sender">The Frame which failed navigation</param>
        /// <param name="e">Details about the navigation failure</param>
        void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
        }

        /// <summary>
        /// Invoked when application execution is being suspended.  Application state is saved
        /// without knowing whether the application will be terminated or resumed with the contents
        /// of memory still intact.
        /// </summary>
        /// <param name="sender">The source of the suspend request.</param>
        /// <param name="e">Details about the suspend request.</param>
        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            deferral.Complete();
        }

        /// <summary>
        /// Gestiona las notifiones push recibidas y las envía al canal adecuado de <see cref="Xamarin.Forms.MessagingCenter"/>
        /// </summary>
        /// <param name="sender"> El emisor del mensaje (servidor de notificaciones)</param>
        /// <param name="args">El contenido del mensaje con parámetros adicionales</param>
        private void OnPushNotificationReceived(PushNotificationChannel sender, PushNotificationReceivedEventArgs args)
        {
            var data = args.ToastNotification.Content.InnerText;
            string msg = string.Empty;
            if (data.Contains("grupal"))
                msg = "Message Guild";
            else if (data.Contains("directo"))
                msg = "Message Chat";
            else if (data.Contains("expulsado"))
            {
                string name = string.Empty;
                var split = data.Split(' ');
                for(int i = 0; i< split.Length; i++)
                {
                    if (split[i].Equals("gremio"))
                        name = split[i + 1];
                }
                if (name.Equals(string.Empty))
                    return;
                msg = "GuildKick $ " + name;
            }

            if(!msg.Equals(string.Empty))
                Xamarin.Forms.MessagingCenter.Send<object, string>(this, ULFG.Forms.App.NotificationReceivedKey, msg);
        }
    }
}
