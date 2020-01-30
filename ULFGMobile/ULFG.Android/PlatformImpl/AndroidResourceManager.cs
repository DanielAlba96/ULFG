using Android.Content.Res;
using Android.Graphics;
using System.IO;
using ULFG.Droid;
using ULFG.Forms.PlatformInterfaces;

[assembly: Xamarin.Forms.Dependency(typeof(AndroidResourceManager))]
namespace ULFG.Droid
{
    /// <summary>
    /// Implementación de la interfaz  <see cref="IResourceManager"/> de inyección de dependencias para las operaciones relacionadas con los recursos en Android
    /// </summary>
    public class AndroidResourceManager : IResourceManager
    {
        /// <summary>
        /// Referencia s los recursos de la aplicación
        /// </summary>
        public static Resources Resources { get; set; }

        /// <summary>
        /// Implementación de <see cref="IResourceManager.GetBasicProfileImageAsByteArray"/>
        /// </summary>
        public byte[] GetBasicProfileImageAsByteArray()
        {
            var profileImg = BitmapFactory.DecodeResource(Resources, Resource.Drawable.basic_avatar);
            MemoryStream stream = new MemoryStream();
            profileImg.Compress(Bitmap.CompressFormat.Png, 100, stream);
            var res = stream.ToArray();
            return res;
        }

        /// <summary>
        /// Implementación de <see cref="IResourceManager.GetBasicGuildImageAsByteArray"/>
        /// </summary>
        public byte[] GetBasicGuildImageAsByteArray()
        {
            var guildImg = BitmapFactory.DecodeResource(Resources, Resource.Drawable.basic_guild);
            MemoryStream stream = new MemoryStream();
            guildImg.Compress(Bitmap.CompressFormat.Png, 100, stream);
            var res = stream.ToArray();
            return res;
        }

        /// <summary>
        /// Implementación de <see cref="IResourceManager.GetResourcesPath(string)"/>
        /// </summary>
        public string GetResourcesPath(string resourceName)
        {
            return resourceName;
        }
    }
}