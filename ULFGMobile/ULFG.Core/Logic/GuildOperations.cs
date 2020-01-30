using Plugin.Connectivity;
using System.Threading.Tasks;
using ULFG.Core.Data.Item;
using ULFG.Core.Data.ItemManager;
using ULFG.Core.Data.ItemManager.Impl;

namespace ULFG.Core.Logic
{
    /// <summary>
    /// Clase que contiene toda la lógica de los gremios
    /// </summary>
    public class GuildOperations
    {
        readonly IGuildManager guildManager;
        readonly IGuildMemberManager guildMemberManager;
        readonly IMessageManager messageManager;

        /// <summary>
        /// Constructor con parmametros
        /// </summary>
        public GuildOperations (IGuildManager guildManager, IGuildMemberManager guildMemberManager,
            IMessageManager messageManager)
        {
            this.guildManager = guildManager;
            this.guildMemberManager = guildMemberManager;
            this.messageManager = messageManager;
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public GuildOperations()
        {
            this.guildManager = GuildManager.DefaultManager;
            this.guildMemberManager = GuildMemberManager.DefaultManager;
            this.messageManager = MessageManager.DefaultManager;
        }

        /// <summary>
        /// Crea un nuevo gremio
        /// </summary>
        /// <param name="name"> Nombre del gremio</param>
        /// <param name="description">Descripción del gremio</param>
        /// <param name="user">El identificador del usuario que crea el gremio</param>
        /// <param name="visibility">Si es gremio es público o privado</param>
        /// <param name="img">La imagen por defecto de un gremio nuevo</param>
        public async Task<Guild> CreateGuild(string name, string description, string user, bool visibility, byte[] img)
        {
            Guild guild = new Guild
            {
                Name = name,
                Description = description,
                IsPublic = visibility,
                Leader = user,
                Image = img
            };
            guild = await guildManager.SaveGuildAsync(guild);
            GuildMember m = new GuildMember
            {
                GuildId = guild.Id,
                MemberId = user
            };
            await guildMemberManager.SaveGuildMemberAsync(m);
            return guild;
        }
        /// <summary>
        /// Abandona o borra un gremio
        /// </summary>
        /// <remarks>Esto tambien abarca las expulsiones.
        /// Si el usuario que abandona es el creador, el gremio se borra y se expulsa a todos los miembros, 
        /// en caso contrario solo se expulsa al usuario que se pasa como parámetro</remarks>
        /// <param name="user">El identificador del usuario que abandona</param>
        /// <param name="guild">El ientificador del gremio del usuario</param>
        /// <returns></returns>
        public async Task LeaveGuild(string user, string guild)
        {
            var member = await guildMemberManager.GetGuildMember(guild, user, CrossConnectivity.Current.IsConnected);
            if (member != null)
                await guildMemberManager.DeleteGuildMemberAsync(member);
        }
    }
}
