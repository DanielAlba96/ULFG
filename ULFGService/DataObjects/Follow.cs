using Microsoft.Azure.Mobile.Server;
using System;

namespace ULFGService.DataObjects
{
    /// <summary>
    /// Mapea un Seguimiento en la base de datos
    /// </summary>
    public class Follow:EntityData
    {
        /// <summary>
        /// Usuario que sigue
        /// </summary>
        public string Following_user { get; set; }

        /// <summary>
        /// Propiedad de navegación
        /// </summary>
        public User User1 { get; set; }

        /// <summary>
        /// Usuario seguido
        /// </summary>
        public string Followed_user { get; set; }

        /// <summary>
        /// Propiedad de navegación
        /// </summary>
        public User User2 { get; set; }

        /// <summary>
        /// Fecha de la última actualización en el cliente
        /// </summary>
        public DateTime Client_updated { get; set; }
    }
}