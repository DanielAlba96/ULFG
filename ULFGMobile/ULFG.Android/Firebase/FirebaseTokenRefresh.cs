using System;
using Android.App;
using Android.Content;
using Firebase.Iid;

namespace ULFG.Droid
{
    /// <summary>
    /// Obtiene o refresca el token identificador del dispositivo
    /// </summary>
    [Service]
    [IntentFilter(new[] { "com.google.firebase.INSTANCE_ID_EVENT" })]
    public class FirebaseTokenRefresh : FirebaseInstanceIdService
    {
        /// <summary>
        /// Refresca el token de identificacion del dispositivo
        /// </summary>
        public override void OnTokenRefresh()
        {
            base.OnTokenRefresh();
            var refreshedToken = FirebaseInstanceId.Instance.Token;
            Console.WriteLine($"Token received: {refreshedToken}");
        }
    }
}