using Microsoft.WindowsAzure.MobileServices;
using Newtonsoft.Json;
using System;

namespace ULFG.Core.Data.Item
{
    /// <summary>
    /// Modela un bloqueo en el sistema
    /// </summary>
    public class Block
    {
        string id;
        string blockingUser;
        string blockedUser;

        /// <summary>
        /// ID del bloqueo
        /// </summary>
        [JsonProperty(PropertyName = "id")]
        public string Id
        {
            get { return id; }
            set { id = value; }
        }

        /// <summary>
        /// ID del usuario que bloquea
        /// </summary>
        [JsonProperty(PropertyName = "blocking_user")]
        public string BlockingUser
        {
            get { return blockingUser; }
            set { blockingUser = value; }
        }

        /// <summary>
        /// ID del usuario bloqueado
        /// </summary>
        [JsonProperty(PropertyName = "blocked_user")]
        public string BlockedUser
        {
            get { return blockedUser; }
            set { blockedUser = value; }
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
        public DateTime CreatedAt{ get; set; }
    }
}
