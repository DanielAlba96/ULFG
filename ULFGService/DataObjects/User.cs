using Microsoft.Azure.Mobile.Server;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System;

namespace ULFGService.DataObjects
{
    /// <summary>
    /// Mapea un Usuario en la base de datos
    /// </summary>
    public class User: EntityData
    {
        /// <summary>
        /// Nombre de usuario
        /// </summary>
        [Index(IsUnique = true)]
        [StringLength(450)]
        public string Username { get; set; }

        /// <summary>
        /// Apodo del usuario
        /// </summary>
        public string Nickname { get; set; }

        /// <summary>
        /// Biografía del usuario
        /// </summary>
        public string Bio { get; set; }

        /// <summary>
        /// Correo electrónico del usuario
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Contraseña encriptada
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Salt para encriptación
        /// </summary>
        public string Salt { get; set; }
        
        /// <summary>
        /// Imagen de perfil del usuario
        /// </summary>
        public byte[] Image { get; set; }

        /// <summary>
        /// Fecha de la última actualización en el cliente
        /// </summary>
        public DateTime Client_updated { get; set; }

        /// <summary>
        /// Propiedad de navegación
        /// </summary>
        public ICollection<Message> Messages { get; set; }

        /// <summary>
        /// Propiedad de navegación
        /// </summary>
        public ICollection<GuildMember> Guilds { get; set; }

        /// <summary>
        /// Propiedad de navegación
        /// </summary>
        public ICollection<Chat> ChatsM1 { get; set; }

        /// <summary>
        /// Propiedad de navegación
        /// </summary>
        public ICollection<Chat> ChatsM2 { get; set; }

        /// <summary>
        /// Propiedad de navegación
        /// </summary>
        public ICollection<Guild> GuildsOwned { get; set; }

        /// <summary>
        /// Propiedad de navegación
        /// </summary>
        public ICollection<Follow> Follow1 { get; set; }

        /// <summary>
        /// Propiedad de navegación
        /// </summary>
        public ICollection<Follow> Follow2 { get; set; }

        /// <summary>
        /// Propiedad de navegación
        /// </summary>
        public ICollection<Block> Block1 { get; set; }

        /// <summary>
        /// Propiedad de navegación
        /// </summary>
        public ICollection<Block> Block2 { get; set; }

        /// <summary>
        /// Propiedad de navegación
        /// </summary>
        public ICollection<Publication> Publications { get; set; }
    }
}