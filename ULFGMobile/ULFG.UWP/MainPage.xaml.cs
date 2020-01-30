namespace ULFG.UWP
{
    /// <summary>
    /// Representa la página principal de la aplicación y actúa como punto de entrada para  la <see cref="ULFG.Forms.App"/> de Xamarin
    /// </summary>
    public sealed partial class MainPage
    {
        public MainPage()
        {
            this.InitializeComponent();

            LoadApplication(new ULFG.Forms.App());
        }
    }
}
