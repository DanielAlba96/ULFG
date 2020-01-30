using System;

using Android.App;
using Android.Content.PM;
using Android.OS;
using Plugin.Permissions;
using Android.Gms.Common;
using Acr.UserDialogs;
using FFImageLoading.Forms.Droid;
#if DEBUG
using Firebase.Iid;
using System.Threading.Tasks;
#endif

namespace ULFG.Droid
{
    /// <summary>
    /// Representa la aplicación nativa de Android y actúa como punto de entrada
    /// </summary>
    [Activity(Label = "ULFG", Icon = "@mipmap/ulfg", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        /// <summary>
        /// Crea e inicia la aplicación
        /// </summary>
        /// <param name="savedInstanceState">Datos adicionales para la creación</param>
        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            Plugin.CurrentActivity.CrossCurrentActivity.Current.Activity = this;
            AndroidResourceManager.Resources = Resources;

            base.OnCreate(savedInstanceState);
            UserDialogs.Init(this);
            CachedImageRenderer.Init(true);

            Xamarin.Forms.Forms.SetFlags("FastRenderers_Experimental");
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            LoadApplication(new ULFG.Forms.App());

            IsPlayServicesAvailable();
#if DEBUG
            // Force refresh of the token. If we redeploy the app, no new token will be sent but the old one will
            // be invalid.
            Task.Run(() =>
            {
                // This may not be executed on the main thread.
                FirebaseInstanceId.Instance.DeleteInstanceId();
                Console.WriteLine("Forced token: " + FirebaseInstanceId.Instance.Token);
            });
#endif
        }

        /// <summary>
        /// Gestiona las solicitudes de permisos cuando se necesitan
        /// </summary>
        /// <param name="requestCode">código</param>
        /// <param name="permissions">permisos solicitados</param>
        /// <param name="grantResults">resultado de cada permiso solicitado tras la respuesta del usuario</param>
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Android.Content.PM.Permission[] grantResults)
        {
            PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        /// <summary>
        /// Comprueba si el dispositivo Android tiene Google Play services disponible.
        /// </summary>
        /// <remarks>Si Google Play Services no está disponible, se muestra un mensaje y se cierra la aplicación</remarks>
        /// <returns>true si Google Play Services está disponible para su uso, false en caso contrario</returns>
        public bool IsPlayServicesAvailable()
        {
            int resultCode = GoogleApiAvailability.Instance.IsGooglePlayServicesAvailable(this);
            if (resultCode != ConnectionResult.Success)
            {
                if (GoogleApiAvailability.Instance.IsUserResolvableError(resultCode))
                {
                    Console.WriteLine($"Error: {GoogleApiAvailability.Instance.GetErrorString(resultCode)}");
                    Finish();
                }
                else
                {
                    Console.WriteLine("Error: Play services not supported!");
                    Finish();
                }
                return false;
            }
            else
            {
                Console.WriteLine("Play Services available.");
                return true;
            }
        }
    }
}

