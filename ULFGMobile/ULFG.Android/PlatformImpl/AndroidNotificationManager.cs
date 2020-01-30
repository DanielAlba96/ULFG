using Firebase.Iid;
using ULFG.Droid;
using ULFG.Forms.PlatformInterfaces;

[assembly: Xamarin.Forms.Dependency(typeof(AndroidNotificationManager))]
namespace ULFG.Droid
{
    /// <summary>
    /// Implementación de la interfaz <see cref="INotificationManager"/> de inyección de dependencias para las operaciones relacionadas con las notificaciones en Android
    /// </summary>
    public class AndroidNotificationManager:INotificationManager
    {
        /// <summary>
        /// Implementación de <see cref="INotificationManager.GetNativePNSToken"/>
        /// </summary>
        public string GetNativePNSToken() { return FirebaseInstanceId.Instance.Token; }

        /// <summary>
        /// Implementación de <see cref="INotificationManager.GetNativePNSName"/>
        /// </summary>
        public string GetNativePNSName() { return "gcm"; }
    }
}