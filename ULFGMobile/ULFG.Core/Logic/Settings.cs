using Plugin.Settings;
using Plugin.Settings.Abstractions;

namespace ULFG.Core.Logic
{
    /// <summary>
    /// This is the Settings static class that can be used in your Core solution or in any
    /// of your client applications. All settings are laid out the same exact way with getters
    /// and setters. 
    /// </summary>
    public static class Settings
    {
        private static ISettings AppSettings
        {
            get
            {
                return CrossSettings.Current;
            }
        }

        #region Setting Constants
        // Clave para settings generales temporales
        private const string SettingsKey = "settings_key";
        //Clave para el setting que almacena el id del usuario logeado
        private const string UserIdKey = "userid_key";
        //Clave para almacenar el identificador del dispositivo en el servicio de notificaciones nativo
        private const string NHKey = "nhub_key";

        private static readonly string SettingsDefault = string.Empty;

        #endregion


        /// <summary>
        /// Almacenamiento general temporal
        /// </summary>
        public static string GeneralSettings
        {
            get
            {
                return AppSettings.GetValueOrDefault(SettingsKey, SettingsDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue(SettingsKey, value);
            }
        }

        /// <summary>
        /// ID del usuario logeado con sesión
        /// </summary>
        public static string LoggedUser
        {
            get
            {
                return AppSettings.GetValueOrDefault(UserIdKey, SettingsDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue(UserIdKey, value);
            }
        }

        /// <summary>
        /// ID de registro en el servicio de notificacions
        /// </summary>
        public static string NHRegistrationID
        {
            get
            {
                return AppSettings.GetValueOrDefault(NHKey, SettingsDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue(NHKey, value);
            }
        }
    }
}
