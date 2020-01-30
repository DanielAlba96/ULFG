using Microsoft.WindowsAzure.MobileServices;
using Newtonsoft.Json;
using System;

namespace ULFG.Core.Data.Item
{
    /// <summary>
    /// Modela un mensaje individual o grupal en el sistema
    /// </summary>
    public class Message
    {
        string id;       
        string text;
        string chat_id;
        string guild_id;
        string creator_id;
        DateTime creationDate;

        /// <summary>
        /// ID del mensaje
        /// </summary>
        [JsonProperty(PropertyName = "id")]
        public string Id
        {
            get { return id; }
            set { id = value; }
        }

        /// <summary>
        /// Contenido del mensaje
        /// </summary>
        [JsonProperty(PropertyName = "text")]
        public string Text
        {
            get { return text; }
            set { text = value; }
        }

        /// <summary>
        /// ID del chat al que pertenece
        /// </summary>
        [JsonProperty(PropertyName = "chat_id")]
        public string Chat_Id
        {
            get { return chat_id; }
            set { chat_id = value; }
        }

        /// <summary>
        /// ID del gremio al que pertenece
        /// </summary>
        [JsonProperty(PropertyName = "guild_id")]
        public string Guild_Id
        {
            get { return guild_id; }
            set { guild_id = value; }
        }

        /// <summary>
        /// ID del creador
        /// </summary>
        [JsonProperty(PropertyName = "creator_id")]
        public string Creator_Id
        {
            get { return creator_id; }
            set { creator_id = value; }

        }
        /// <summary>
        /// Fecha de creación
        /// </summary>
        [CreatedAt]
        public DateTime CreationDate
        {
            get { return creationDate; }
            set { creationDate = value; }
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
    }
}
