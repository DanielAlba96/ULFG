using System;
using System.IO;
using System.Threading.Tasks;
using ULFG.Forms.PlatformInterfaces;
using Windows.Storage;

namespace ULFG.UWP.PlatformImpl
{
    /// <summary>
    /// Implementación de la interfaz  <see cref="IResourceManager"/> de inyección de dependencias para las operaciones relacionadas con los recursos en UWP
    /// </summary>
    public class UWPResourceManager : IResourceManager
    {
        /// <summary>
        /// Implementación de <see cref="IResourceManager.GetBasicGuildImageAsByteArray"/>
        /// </summary>
        public byte[] GetBasicGuildImageAsByteArray()
        {
            string fname = @"Assets\basic_guild.png";

            byte[] result = null;

            StorageFolder InstallationFolder = Windows.ApplicationModel.Package.Current.InstalledLocation;
            StorageFile file = Task.Run(async ()=> { return await InstallationFolder.GetFileAsync(fname); }).Result;
            result = File.ReadAllBytes(file.Path);

            return result;
        }

        /// <summary>
        /// Implementación de <see cref="IResourceManager.GetBasicProfileImageAsByteArray"/>
        /// </summary>
        public byte[] GetBasicProfileImageAsByteArray()
        {
            string fname = @"Assets\basic_avatar.png";

            byte[] result = null;

            StorageFolder InstallationFolder = Windows.ApplicationModel.Package.Current.InstalledLocation;
            StorageFile file = Task.Run(async () => { return await InstallationFolder.GetFileAsync(fname); }).Result;
            result = File.ReadAllBytes(file.Path);

            return result;
        }

        /// <summary>
        /// Implementación de <see cref="IResourceManager.GetResourcesPath(string)"/>
        /// </summary>
        public string GetResourcesPath(string resourceName)
        {
            return "Assets/" + resourceName;
        }
    }
}
