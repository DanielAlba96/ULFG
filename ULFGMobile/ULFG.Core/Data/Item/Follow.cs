using Microsoft.WindowsAzure.MobileServices;
using Newtonsoft.Json;
using System;

namespace ULFG.Core.Data.Item
{
    /// <summary>
    /// Modela un seguimiento en el sistema
    /// </summary>
    public class Follow
    {
        string id;
        string followingUser;
        string followedUser;

        /// <summary>
        /// ID del seguimiento
        /// </summary>
        [JsonProperty(PropertyName = "id")]
        public string Id
        {
            get { return id; }
            set { id = value; }
        }

        /// <summary>
        /// ID del usuario que sigue
        /// </summary>
        [JsonProperty(PropertyName = "following_user")]
        public string FollowingUser
        {
            get { return followingUser; }
            set { followingUser = value; }
        }

        /// <summary>
        /// ID del usuario seguido
        /// </summary>
        [JsonProperty(PropertyName = "followed_user")]
        public string FollowedUser
        {
            get { return followedUser; }
            set { followedUser = value; }
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
