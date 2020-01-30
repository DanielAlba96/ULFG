using Microsoft.Azure.Mobile.Server;
using System;

namespace ULFGService.DataObjects
{
    /// <summary>
    /// Mapea un Miembro de Gremio en la base de datos
    /// </summary>
    public class GuildMember:EntityData
    {
        /// <summary>
        /// Propiedad de navegación
        /// </summary>
        public Guild Guild { get; set; }

        /// <summary>
        /// ID del gremio
        /// </summary>
        public string Guild_id { get; set; }

        /// <summary>
        /// Propiedad de navegación
        /// </summary>
        public User Member { get; set; }

        /// <summary>
        /// ID del miembro
        /// </summary>
        public string Member_id { get; set; }

        /// <summary>
        /// Fecha de la última actualización en el cliente
        /// </summary>
        public DateTime Client_updated { get; set; }
    }
}