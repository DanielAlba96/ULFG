using Microsoft.Azure.Mobile.Server;
using System;

namespace ULFGService.DataObjects
{
    /// <summary>
    /// Mapea una Publicación en la base de datos
    /// </summary>
    public class Publication:EntityData
    {
        /// <summary>
        /// Texto de la publicación
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Imagen adjunta a la publicacíón
        /// </summary>
        public byte[] Attachment { get; set; }

        /// <summary>
        /// ID del creador de la publicación
        /// </summary>
        public string Autor_id { get; set; }

        /// <summary>
        /// Fecha de la última actualización en el cliente
        /// </summary>
        public DateTime Client_updated { get; set; }

        /// <summary>
        /// Propiedad de navegación
        /// </summary>
        public User Autor { get; set; }       
    }
}