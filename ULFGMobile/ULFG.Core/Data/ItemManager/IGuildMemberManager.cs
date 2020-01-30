using System.Collections.ObjectModel;
using System.Threading.Tasks;
using ULFG.Core.Data.Item;

namespace ULFG.Core.Data.ItemManager
{
    /// <summary>
    /// Define las operaciones sobre la tabla GuildMembers
    /// </summary>
    public interface IGuildMemberManager
    {
        /// <summary>
        /// Borra todos los miembros de un gremio
        /// </summary>
        /// <param name="guildId">El id del gremio cuyos miembros se desea borrar</param>
        Task DeleteAllMemberOfGuildAsync(string guildId);

        /// <summary>
        /// Elimina todas las membresías de un usuario
        /// </summary>
        /// <param name="user">El usuario cuyas membresías se desea borrar</param>
        Task DeleteAllMembershipsFromUser(User user);

        /// <summary>
        /// Borra un miembro de un gremio
        /// </summary>
        /// <param name="item">El miembro del gremio a borrar</param>
        Task DeleteGuildMemberAsync(GuildMember item);

        /// <summary>
        /// Obtiene todos los miembros de todos los gremios
        /// </summary>
        /// <param name="syncItems">Indica si actualizar la tabla local antes de realizar la operación</param>
        /// <returns>Una lista completa de miembros de gremio</returns>
        Task<ObservableCollection<GuildMember>> GetGuildMembersAsync(bool syncItems = false);

        /// <summary>
        /// Obtiene todas las guilds de un usuario
        /// </summary>
        /// <param name="memberId">El id del usuario cuyas guilds se desea obtener</param>
        /// <param name="syncItems">Indica si actualizar la tabla local antes de realizar la operación</param>
        /// <returns>Una lista filtrada de gremios</returns>
        Task<ObservableCollection<Guild>> GetGuildsByMemberIdAsync(string memberId, bool syncItems = false);

        /// <summary>
        /// Obtiene todos los miembros de un gremio
        /// </summary>
        /// <param name="guildId">El id de la guild cuyos miembros se desea obtener</param>
        /// <param name="syncItems">Indica si actualizar la tabla local antes de realizar la operación</param>
        /// <returns>Una lista de usuarios</returns>
        Task<ObservableCollection<User>> GetMembersByGuildIdAsync(string guildId, bool syncItems = false);

        /// <summary>
        /// Obtiene los usuarios que no son miembros de un gremio
        /// </summary>
        /// <param name="guildId">El id del gremios cuyos no miembros se desea obtener</param>
        /// <param name="syncItems">Indica si actualizar la tabla local antes de realizar la operación</param>
        /// <returns>Una lista filtrada de usuarios</returns>
        Task<ObservableCollection<User>> GetNoMembersByGuildIdAsync(string guildId, bool syncItems = false);

        /// <summary>
        /// Obtiene el numero de miembros de un gremio
        /// </summary>
        /// <param name="guildId">El gremio cuyo numero de miembros se desea obtener</param>
        /// <param name="syncItems">Indica si actualizar la tabla local antes de realizar la operación</param>
        /// <returns>El numero de miembros del gremio</returns>
        Task<int> GetNumberOfMembers(string guildId, bool syncItems = false);

        /// <summary>
        /// Busca un miembro de gremio
        /// </summary>
        /// <param name="guildId">El gremio</param>
        /// <param name="memberId">El usuario</param>
        /// <param name="syncItems">Indica si actualizar la tabla local antes de realizar la operación</param>
        /// <returns>Un miembro de gremio o null si no existe</returns>
        Task<GuildMember> GetGuildMember(string guildId, string memberId, bool syncItems = false);

        /// <summary>
        /// Inserta un miembro de gremio en la tabla
        /// </summary>
        /// <param name="item">El miembro de gremio a insertar</param>
        Task SaveGuildMemberAsync(GuildMember item);
    }
}