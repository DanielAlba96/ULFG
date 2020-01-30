using Microsoft.WindowsAzure.MobileServices;
using Newtonsoft.Json;
using System;

namespace ULFG.Core.Data.Item
{
    /// <summary>
    /// Modela un usuario en el sistema
    /// </summary>
    public class User
    {
        string id;
        string username;
        string nickname;
        string bio;
        string email;
        string salt;
        string password;
        byte[] image;

        /// <summary>
        /// ID del usuario
        /// </summary>
        [JsonProperty(PropertyName = "id")]
        public string Id
        {
            get { return id; }
            set { id = value; }
        }

        /// <summary>
        /// Nombre del usuario
        /// </summary>
        [JsonProperty(PropertyName = "username")]
        public string Username
        {
            get { return username; }
            set { username = value; }
        }

        /// <summary>
        /// Apodo del usuario
        /// </summary>
        [JsonProperty(PropertyName = "nickname")]
        public string Nickname
        {
            get { return nickname; }
            set { nickname = value; }
        }

        /// <summary>
        /// Biografía del usuario
        /// </summary>
        [JsonProperty(PropertyName = "bio")]
        public string Bio
        {
            get { return bio; }
            set { bio = value; }
        }

        /// <summary>
        /// Correo electrónico del usuario
        /// </summary>
        [JsonProperty(PropertyName = "email")]
        public string Email
        {
            get { return email; }
            set { email = value; }
        }

        /// <summary>
        /// Salt de encriptación
        /// </summary>
        [JsonProperty(PropertyName = "salt")]
        public string Salt
        {
            get { return salt; }
            set { salt = value; }
        }

        /// <summary>
        /// Contraséña encriptada
        /// </summary>
        [JsonProperty(PropertyName = "password")]
        public string Password
        {
            get { return password; }
            set { password = value; }
        }

        /// <summary>
        /// Imagen de perfil del usuario
        /// </summary>
        [JsonProperty(PropertyName = "image")]
        public byte[] Image
        {
            get { return image; }
            set { image = value; }
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
