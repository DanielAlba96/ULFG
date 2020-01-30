using Newtonsoft.Json;
using Microsoft.WindowsAzure.MobileServices;
using System;

namespace ULFG.Core.Data.Item
{
    /// <summary>
    /// Modela un texto corto que puede contener una imagen adjunta en el sistema
    /// </summary>
    public class Publication
    {
        string id;
        string autorId;
        string text;
        byte[] attachment;

        /// <summary>
        /// ID de la publicación
        /// </summary>
        [JsonProperty(PropertyName = "id")]
        public string Id
        {
            get { return id; }
            set { id = value; }
        }

        /// <summary>
        /// Creador de la publicación
        /// </summary>
        [JsonProperty(PropertyName = "autor_id")]
        public string AutorId
        {
            get { return autorId; }
            set { autorId = value; }
        }

        /// <summary>
        /// Texto de la publicación
        /// </summary>
        [JsonProperty(PropertyName = "text")]
        public string Text
        {
            get { return text; }
            set { text = value; }
        }

        /// <summary>
        /// Imagen adjunta de la publicación
        /// </summary>
        [JsonProperty(PropertyName = "attachment")]
        public byte[] Attachment
        {
            get { return attachment; }
            set { attachment = value; }
        }

        /// <summary>
        /// Fecha de la ultima actualización en un cliente
        /// </summary>
        [JsonProperty(PropertyName = "client_updated")]
        public DateTime UpdatedInClient { get; set; }

        /// <summary>
        /// Indica si ha sido borrado
        /// </summary>
        [Deleted]
        public bool Deleted { get; set; }

        /// <summary>
        /// Version 
        /// </summary>
        [Version]
        public string Version { get; set; }

        /// <summary>
        /// Fecha de ultima actualización en el servidor
        /// </summary>
        [UpdatedAt]
        public DateTime UpdatedAt { get; set; }

        /// <summary>
        /// Fecha de creación
        /// </summary>
        [CreatedAt]
        public DateTime CreatedAt { get; set; }
    }
}
