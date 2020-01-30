using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using ULFG.Core.Data.Item;

namespace ULFG.Core.Data.ItemManager
{
    /// <summary>
    /// Define las operaciones sobre la tabla de Blocks
    /// </summary>
    public interface IGuildManager
    {
        /// <summary>
        /// Borra todoso los gremios de un usuario
        /// </summary>
        /// <param name="user">El usuario cuyos gremios se quiere borrar</param>
        Task DeleteAllGuildsFromUser(User user);

        /// <summary>
        /// Borra un gremio
        /// </summary>
        /// <param name="guild">El gremio a borrar</param>
        Task DeleteGuildAsync(Guild guild);

        /// <summary>
        /// Busca un gremio por id
        /// </summary>
        /// <param name="id">El id del gremio a buscar</param>
        /// <param name="syncItems">Indica si actualizar la tabla local antes de realizar la operación</param>
        /// <returns>El gremio buscado o null si no existe</returns>
        Task<Guild> GetGuildByIdAsync(string id, bool syncItems = false);

        /// <summary>
        /// Busca un gremio por una cadena en su nombre o descripción
        /// </summary>
        /// <param name="word">La cadena a buscar</param>
        /// <param name="syncItems">Indica si actualizar la tabla local antes de realizar la operación</param>
        /// <returns>Una lista filtrada de gremios</returns>
        Task<ObservableCollection<Guild>> GetGuildByWordAsync(string word, bool syncItems = false);

        /// <summary>
        /// Obtiene todos los gremios
        /// </summary>
        /// <param name="syncItems">Indica si actualizar la tabla local antes de realizar la operación</param>
        /// <returns>Una lista completa de gremios</returns>
        Task<ObservableCollection<Guild>> GetGuildsAsync(bool syncItems = false);

        /// <summary>
        /// Obtiene los gremios de un usuario
        /// </summary>
        /// <param name="id">El id del usuario cuyos gremios se quiere obtener</param>
        /// <param name="syncItems">Indica si actualizar la tabla local antes de realizar la operación</param>
        /// <returns>Una lista filtrada de gremios</returns>
        Task<ObservableCollection<Guild>> GetGuildsByMemberIdAsync(string id, bool syncItems = false);

        /// <summary>
        /// Obtiene los gremios que tienen un conjunto de miembros
        /// </summary>
        /// <param name="members">Una lista con los miembros que debe contener el gremio</param>
        /// <param name="syncItems">Indica si actualizar la tabla local antes de realizar la operación</param>
        /// <returns>Una lista filtrada de gremios</returns>
        Task<ObservableCollection<Guild>> GetGuildsByMembersIdsAsync(List<User> members, bool syncItems = false);

        /// <summary>
        /// Busca un gremio por su nombre
        /// </summary>
        /// <param name="name">El nombre del gremio</param>
        /// <param name="syncItems">Indica si actualizar la tabla local antes de realizar la operación</param>
        /// <returns>El gremio buscado o null si no existe</returns>
        Task<Guild> GetGuildByNameAsync(string name, bool syncItems = false);

        /// <summary>
        /// Determina si un usuario es miembro de un gremio
        /// </summary>
        /// <param name="guild">El gremio al que pertenece</param>
        /// <param name="member">El usuario que se quiere comprobar</param>
        /// <param name="syncItems">Indica si actualizar la tabla local antes de realizar la operación</param>
        /// <returns>True si el usuario es miembro del gremio, False en caso contrario</returns>
        Task<bool> IsMemberAsync(string guild, string member, bool syncItems = false);

        /// <summary>
        /// inserta un gremio en la tabla
        /// </summary>
        /// <param name="item">El gremio a insertar</param>
        /// <returns>El gremio insertado</returns>
        Task<Guild> SaveGuildAsync(Guild item);
    }
}