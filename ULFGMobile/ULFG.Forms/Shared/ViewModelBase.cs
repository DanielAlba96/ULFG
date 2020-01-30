using System.ComponentModel;
using System.Runtime.CompilerServices;
using Xamarin.Forms;

namespace ULFG.Forms.Shared
{
    /// <summary>
    /// Clase que implementa <see cref="INotifyPropertyChanged"/> y que se usa como base para los modelos de todas las vistas
    /// </summary>
    public class ViewModelBase:INotifyPropertyChanged
    {
        /// <summary>
        /// Referencia a la navegación actual de la aplicación
        /// </summary>
        protected INavigation Navigation;

        /// <summary>
        /// Crea el ViewModel
        /// </summary>
        /// <param name="nav">referencia a la navegación actual de la aplicación</param>
        public ViewModelBase(INavigation nav)
        {
            this.Navigation = nav;
        }

        /// <summary>
        /// Se lanza para indicar que una propiedad ha cambiado
        /// </summary>
        /// <param name="propertyName">nombre de la propiedad</param>
        protected void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handle = PropertyChanged;
            if (handle != null)
                handle(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Representa el evento que indica que una propiedad ha cambiado
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
