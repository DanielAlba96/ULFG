using FFImageLoading.Forms;
using System.IO;
using Xamarin.Forms;

namespace ULFG.Forms.Publications.Views
{
    /// <summary>
    /// <see cref="ContentPage"/> que muestra el detalle de una imagen a pantalla completa
    /// </summary>
    public class ImageDetail:ContentPage
    {
        public ImageDetail(byte[] img)
        {
            CachedImage content = new CachedImage()
            {
                Source = ImageSource.FromStream(() => { return new MemoryStream(img); }),
                Aspect = Aspect.AspectFit,
                HeightRequest = 600
            };
            Content = content;
        }
    }
}
