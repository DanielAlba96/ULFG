using Acr.UserDialogs;
using Plugin.Media;
using Plugin.Media.Abstractions;
using System;
using System.IO;
using System.Threading.Tasks;

namespace ULFG.Forms.Shared
{
    /// <summary>
    /// Contiene las funciones que actúan sobre los archivos de imagen
    /// </summary>
    public class ImageHelper
    {
        /// <summary>
        /// Carga una imagen desde la galeria usando <see cref="CrossMedia"/>. Solicita permisos de lectura y muestra un aviso en el caso de que 
        /// no se concedan.
        /// </summary>
        /// <returns>Una referencia a una tarea que devuelve un array de bytes</returns>
        public async Task<byte[]> LoadImage()
        {
            try
            {
                await CrossMedia.Current.Initialize();
                //Carga una imagen de la galería y solita permisos de lectura
                var file = await CrossMedia.Current.PickPhotoAsync(new PickMediaOptions()
                {
                    MaxWidthHeight = 800,
                    PhotoSize = PhotoSize.MaxWidthHeight
                });
                //Comprueba si se cargó una imagen
                if (file != null)
                {
                    Stream stream = file.GetStream();
                    file.Dispose();
                    byte[] bytes;
                    using (MemoryStream ms = new MemoryStream())
                    {
                        stream.CopyTo(ms);
                        bytes = ms.ToArray();
                    }
                    //Comprueba que la imagen no es demasiado grande
                    if (bytes.Length > 850000)
                    {
                        var config = new AlertConfig() { Title = "Tamaño de imagen no soportado", Message = "La imagen es demasiado grande, elige otra (max 800 KB)" };
                        await UserDialogs.Instance.AlertAsync(config);
                        return null;
                    }
                    return bytes;
                }
                return null;
            }
            catch (MediaPermissionException ex)
            {
                var config = new AlertConfig() { Title = "Error de permisos", Message = " Se necesitan permisos de lectura para adjuntar una imagen" };
                await UserDialogs.Instance.AlertAsync(config);
                Console.WriteLine("Error de permisos: " + ex.Message);
                return null;
            }
        }
    }
}
