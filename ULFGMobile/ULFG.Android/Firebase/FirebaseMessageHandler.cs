using System;
using Android.App;
using Android.Content;
using Firebase.Messaging;
using Xamarin.Forms;

namespace ULFG.Droid
{
    /// <summary>
    /// Gestiona las notificaciones push mediante <see cref="FirebaseMessagingService"/>
    /// </summary>
    [Service]
    [IntentFilter(new[] { "com.google.firebase.MESSAGING_EVENT" })]
    public class FirebaseMessageHandler : FirebaseMessagingService
    {
        /// <summary>
        /// Gestiona las notificaciones recibidas 
        /// </summary>
        /// <param name="message">La notificación</param>
        public override void OnMessageReceived(RemoteMessage message)
        {
            base.OnMessageReceived(message);

            Console.WriteLine("Received: " + message);

            try
            {
                string data = message.Data["content"];
                if (data.Equals(string.Empty))
                    return;
                MessagingCenter.Send<object, string>(this, ULFG.Forms.App.NotificationReceivedKey, data);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error extracting message: " + ex);
            }
        }
    }
}