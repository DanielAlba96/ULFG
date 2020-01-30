using Microsoft.WindowsAzure.MobileServices;
using Newtonsoft.Json;
using System;

namespace ULFG.Core.Data.Item
{
    /// <summary>
    /// Modela un grupo de usuarios personalizable en el sistema
    /// </summary>
    public class Guild
    {
        string id;
        string name;
        byte[] image;
        string description;
        string message;
        string leader;
        bool isPublic;

        /// <summary>
        /// ID del gremio
        /// </summary>
        [JsonProperty(PropertyName = "id")]
        public string Id
        {
            get { return id; }
            set { id = value; }
        }

        /// <summary>
        /// Nombre del gremio
        /// </summary>
        [JsonProperty(PropertyName = "name")]
        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        /// <summary>
        /// Imagen del gremio
        /// </summary>
        [JsonProperty(PropertyName = "image")]
        public byte[] Image
        {
            get { return image; }
            set { image = value; }
        }

        /// <summary>
        /// Descripción del gremio
        /// </summary>
        [JsonProperty(PropertyName = "description")]
        public string Description
        {
            get { return description; }
            set { description = value; }
        }
        /// <summary>
        /// Indica si el gremio es público o privado
        /// </summary>
        [JsonProperty(PropertyName = "is_public")]
        public bool IsPublic
        {
            get { return isPublic; }
            set { isPublic = value; }
        }

        /// <summary>
        /// Líder o creador del gremio
        /// </summary>
        [JsonProperty(PropertyName = "leader_id")]
        public string Leader
        {
            get { return leader; }
            set { leader = value; }
        }

        /// <summary>
        /// Mensaje fijado del gremio
        /// </summary>
        [JsonProperty(PropertyName = "guild_message")]
        public string Message
        {
            get { return message; }
            set { message = value; }
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

