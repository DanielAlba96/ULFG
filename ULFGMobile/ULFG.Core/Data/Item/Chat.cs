using Microsoft.WindowsAzure.MobileServices;
using Newtonsoft.Json;
using System;

namespace ULFG.Core.Data.Item
{
    /// <summary>
    /// Modela una agrupación de  mensajes individuales entre usuarios en el sistema
    /// </summary>
    public class Chat
    {
        string id;
        string title;
        string image;
        string member1Id;
        string member2Id;
        bool member1Deleted;
        bool member2Deleted;

        /// <summary>
        /// ID del chat
        /// </summary>
        [JsonProperty(PropertyName = "id")]
        public string Id
        {
            get { return id; }
            set { id = value; }
        }

        /// <summary>
        /// Titulo del chat
        /// </summary>
        [JsonProperty(PropertyName = "title")]
        public string Title
        {
            get { return title; }
            set { title = value; }
        }

        /// <summary>
        /// Imagen del chat
        /// </summary>
        [JsonProperty(PropertyName = "image")]
        public string Image
        {
            get { return image; }
            set { image = value; }
        }

        /// <summary>
        /// ID de un miembro
        /// </summary>
        [JsonProperty(PropertyName = "member1_id")]
        public string Member1_id
        {
            get { return member1Id; }
            set { member1Id = value; }
        }

        /// <summary>
        /// ID de un miembro
        /// </summary>
        [JsonProperty(PropertyName = "member2_id")]
        public string Member2_id
        {
            get { return member2Id; }
            set { member2Id = value; }
        }

        /// <summary>
        /// Indica si el miembro1 ha borrado el chat
        /// </summary>
        [JsonProperty(PropertyName = "member1_deleted")]
        public bool Member1_deleted
        {
            get { return member1Deleted; }
            set { member1Deleted = value; }
        }

        /// <summary>
        /// Indica si el miembro2 ha borrado el chat
        /// </summary>
        [JsonProperty(PropertyName = "member2_deleted")]
        public bool Member2_deleted
        {
            get { return member2Deleted; }
            set { member2Deleted = value; }
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
