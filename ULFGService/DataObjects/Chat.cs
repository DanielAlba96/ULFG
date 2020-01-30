using Microsoft.Azure.Mobile.Server;
using System;
using System.Collections.Generic;

namespace ULFGService.DataObjects
{
    /// <summary>
    /// Mapea un Chat en la base de datos
    /// </summary>
    public class Chat: EntityData
    {
        /// <summary>
        /// Titulo del chat
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Imagen del chat
        /// </summary>
        public string Image { get; set; }

        /// <summary>
        /// ID de un miembro
        /// </summary>
        public string Member1_id { get; set; }

        /// <summary>
        /// ID de un miembro
        /// </summary>
        public string Member2_id { get; set; }

        /// <summary>
        /// Indica si el miembro1 ha borrado el chat
        /// </summary>
        public bool Member1_deleted { get; set; }

        /// <summary>
        /// Indica si el miembro2 ha borrado el chat
        /// </summary>
        public bool Member2_deleted { get; set; }

        /// <summary>
        /// Propiedad de navegación
        /// </summary>
        public User Member1 { get; set; }

        /// <summary>
        /// Propiedad de navegación
        /// </summary>
        public User Member2 { get; set; }

        /// <summary>
        /// Propiedad de navegación
        /// </summary>
        public ICollection<Message> Messages { get; set; }

        /// <summary>
        /// Fecha de la última actualización en el cliente
        /// </summary>
        public DateTime Client_updated { get; set; }
    }
}