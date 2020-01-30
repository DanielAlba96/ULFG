using Microsoft.Azure.Mobile.Server;
using System;
using System.Collections.Generic;

namespace ULFGService.DataObjects
{
    /// <summary>
    /// Mapea un Gremio en la base de datos
    /// </summary>
    public class Guild: EntityData
    {
        /// <summary>
        /// Nombre del gremio
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Imagen del gremio
        /// </summary>
        public byte[] Image { get; set; }

        /// <summary>
        /// Descripción del gremio
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Indica si el gremio es público o no
        /// </summary>
        public bool Is_public { get; set; }

        /// <summary>
        /// ID del lider del gremio
        /// </summary>
        public string Leader_id { get; set; }

        /// <summary>
        /// Mensaje fijado del gremio
        /// </summary>
        public string Guild_message { get; set; }

        /// <summary>
        /// Propiedad de navegación
        /// </summary>
        public ICollection<GuildMember> Members { get; set; }

        /// <summary>
        /// Propiedad de navegación
        /// </summary>
        public ICollection<Message> Messages { get; set; }

        /// <summary>
        /// Propiedad de navegación
        /// </summary>
        public User Leader { get; set; }

        /// <summary>
        /// Fecha de la última actualización en el cliente
        /// </summary>
        public DateTime Client_updated { get; set; }
    }
}