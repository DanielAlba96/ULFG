using Newtonsoft.Json;
using Microsoft.WindowsAzure.MobileServices;
using System;

namespace ULFG.Core.Data.Item
{
    /// <summary>
    /// Modela los usuarios que forman parte de un grupo en el sistema
    /// </summary>
    public class GuildMember
    {
        string id;
        string guildId;
        string memberId;

        /// <summary>
        /// ID del miembro de gremio
        /// </summary>
        [JsonProperty(PropertyName = "id")]
        public string Id
        {
            get { return id; }
            set { id = value; }
        }

        /// <summary>
        /// ID del gremio
        /// </summary>
        [JsonProperty(PropertyName = "guild_id")]
        public string GuildId
        {
            get { return guildId; }
            set { guildId = value; }
        }

        /// <summary>
        /// ID del miembro
        /// </summary>
        [JsonProperty(PropertyName = "member_id")]
        public string MemberId
        {
            get { return memberId; }
            set { memberId = value; }
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
