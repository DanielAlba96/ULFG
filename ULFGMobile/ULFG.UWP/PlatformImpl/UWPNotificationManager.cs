using ULFG.Forms.PlatformInterfaces;

namespace ULFG.UWP.PlatformImpl
{
    /// <summary>
    /// Implementación de la interfaz <see cref="INotificationManager"/> de inyección de dependencias para las operaciones relacionadas con las notificaciones en UWP
    /// </summary>
    public class UWPNotificationManager : INotificationManager
    {
        public static string Token;

        /// <summary>
        /// Implementación de <see cref="INotificationManager.GetNativePNSName"/>
        /// </summary>
        public string GetNativePNSName()
        {
            return "wns";
        }

        /// <summary>
        /// Implementación de <see cref="INotificationManager.GetNativePNSToken"/>
        /// </summary>
        public string GetNativePNSToken()
        {
            return Token;
        }
    }
}
