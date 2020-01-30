using Microsoft.Azure.Mobile.Server;
using System;

namespace ULFGService.DataObjects
{
    /// <summary>
    /// Mapea un Mensaje en la base de datos
    /// </summary>
    public class Message : EntityData
    {
        /// <summary>
        /// Texto del mensaje
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Propiedad de navegación
        /// </summary>
        public User Creator { get; set; }

        /// <summary>
        /// ID del creador del mensaje
        /// </summary>
        public string Creator_id { get; set; }

        /// <summary>
        /// Propiedad de navegación
        /// </summary>
        public Chat Chat { get; set; }

        /// <summary>
        /// ID del chat que que pertenece el mensaje
        /// </summary>
        public string Chat_id { get; set; }

        /// <summary>
        /// Propiedad de navegación
        /// </summary>
        public Guild Guild { get; set; }

        /// <summary>
        /// ID del gremio que que pertenece el mensaje
        /// </summary>
        public string Guild_id { get; set; }

        /// <summary>
        /// Fecha de la última actualización en el cliente
        /// </summary>
        public DateTime Client_updated { get; set; }
    }
}