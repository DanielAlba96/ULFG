using System;
using Xamarin.Forms;

namespace ULFG.Forms
{
    /// <summary>
    /// Representa cada objeto individual del menú de navegación
    /// </summary>
    public class MasterPageItem
    {
        /// <summary>
        /// Texto a mostrar del elemento
        /// </summary>
        public string Title { get; set;  }
        /// <summary>
        /// Imagen del elemento
        /// </summary>
        public string IconSource { get; set; } 
        /// <summary>
        /// Página asociada
        /// </summary>
        public Page Page { get; set; } 
        /// <summary>
        /// Tipo de la página asociada
        /// </summary>
        public Type TargetType { get; set; } 
    }
}
