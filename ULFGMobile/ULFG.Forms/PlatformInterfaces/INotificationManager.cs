
namespace ULFG.Forms.PlatformInterfaces
{
    /// <summary>
    /// Interfaz empleada para la inyeccción de dependencias que contiene la funcionalidad nativa relacionada con las notificaciones push
    /// </summary>
    public interface INotificationManager
    { 
        /// <summary>
        /// Obtiene el Token nativo que identifica el dispositivo en el servidor de notificaciones
        /// </summary>
        /// <returns>Una cadena con el token de identificación</returns>
        string GetNativePNSToken();

        /// <summary>
        /// Obtiene el nombre del servidor de notificaciones
        /// </summary>
        /// <returns>Una cadena con el nombre del servidor</returns>
        string GetNativePNSName();
    }
}
