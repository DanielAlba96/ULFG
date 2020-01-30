using Xamarin.Forms;

namespace ULFG.Core.Data
{
    /// <summary>
    /// Clase estática con constantes empleadas por la capa de persistencia
    /// </summary>
    public static class Constants
    {
        private static string applicationURL = "https://ulfg.azurewebsites.net/";
        private static Application application;
        /// <summary>
        /// URL de la aplicación web
        /// </summary>
        public static string ApplicationURL
        {
            get { return applicationURL; }
            set { applicationURL = value; }
        }

        /// <summary>
        /// Referencia a la aplicación de cliente
        /// </summary>
        public static Application Application
        {
            get { return application; }
            set { application = value; }
        }
    }
}

