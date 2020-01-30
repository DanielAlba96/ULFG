using FFImageLoading.Forms;
using Xamarin.Forms;

namespace ULFG.Forms.Shared
{
    /// <summary>
    /// <see cref="ViewCell"/> base empleada para las cells que muestran los datos del usuario
    /// </summary>
    public abstract class UserViewCellBase : ViewCell
    {
        /// <summary>
        /// Referencia a la navegación actual de la aplicación
        /// </summary>
        protected INavigation nav;
        /// <summary>
        /// Labels para los datos del usuario
        /// </summary>
        protected Label lblNick, lblName, lblSeparator;
        /// <summary>
        /// Imagen de perfil del usuario
        /// </summary>
        protected CachedImage userIcon;

        protected UserViewCellBase()
        {
            InitializeBasicComponent();
        }
        /// <summary>
        /// Crea todos los componentes de la <see cref="UserViewCellBase"/> que son usados
        /// por todas las clases hijas
        /// </summary>
        protected void InitializeBasicComponent()
        {
            this.nav = (INavigation)Application.Current.Properties["userNavigation"];
            lblNick = new Label
            {
                FontSize = 16,
                FontAttributes = FontAttributes.Bold,
                TextColor = Color.FromHex("A45F00"),
                HorizontalOptions = LayoutOptions.StartAndExpand,
            };

            lblSeparator = new Label
            {
                Text = "@",
                FontSize = 14,
                TextColor = Color.FromHex("424242"),
                HorizontalOptions = LayoutOptions.StartAndExpand
            };

            lblName = new Label
            {
                FontSize = 14,
                TextColor = Color.FromHex("424242"),
                HorizontalOptions = LayoutOptions.StartAndExpand,
            };
            userIcon = new CachedImage();
        }
    }
}
