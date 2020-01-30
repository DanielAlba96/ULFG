using Microsoft.Azure.Mobile.Server;
using System;

namespace ULFGService.DataObjects
{
    /// <summary>
    /// Mapea un Bloqueo en la base de datos
    /// </summary>
    public class Block: EntityData
    {
       /// <summary>
       /// Usuario que bloquea
       /// </summary>
        public string Blocking_user { get; set; }

        /// <summary>
        /// Propiedad de navegación
        /// </summary>
        public User User1 { get; set; }

        /// <summary>
        /// Usuario bloqueado
        /// </summary>
        public string Blocked_user { get; set; }

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