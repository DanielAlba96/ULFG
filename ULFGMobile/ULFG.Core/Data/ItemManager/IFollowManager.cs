using System.Collections.ObjectModel;
using System.Threading.Tasks;
using ULFG.Core.Data.Item;

namespace ULFG.Core.Data.ItemManager
{
    /// <summary>
    /// Define las operaciones sobre la tabla Follows
    /// </summary>
    public interface IFollowManager
    {
        /// <summary>
        /// Borra todos los seguimientos de un usuario
        /// </summary>
        /// <param name="user">El usuario cuyos seguimientos se quiere borrar</param>
        Task DeleteAllFollowsFromUser(User user);

        /// <summary>
        /// Borra un seguimiento
        /// </summary>
        /// <param name="item">El seguimiento a borrar</param>
        Task DeleteFollowAsync(Follow item);

        /// <summary>
        /// Obtiene todos los seguimientos 
        /// </summary>
        /// <param name="syncItems">Indica si actualizar la tabla local antes de realizar la operación</param>
        /// <returns>Una lista completa de seguimientos</returns>
        Task<ObservableCollection<Follow>> GetFollowsAsync(bool syncItems = false);

        /// <summary>
        /// Busca un follow concreto
        /// </summary>
        /// <param name="following">El id del usuario que sigue</param>
        /// <param name="followed">El id del usuario seguido</param>
        /// <param name="syncItems">Indica si actualizar la tabla local antes de realizar la operación</param>
        /// <returns>El follow buscado o null si no existe</returns>
        Task<Follow> GetFollowsByBothSidesAsync(string following, string followed, bool syncItems = false);

        /// <summary>
        /// Obtiene los usuarios seguidos por otro
        /// </summary>
        /// <param name="follow">El usuario que sigue</param>
        /// <param name="syncItems">Indica si actualizar la tabla local antes de realizar la operación</param>
        /// <returns>Una lista filtrada de usuarios</returns>
        Task<ObservableCollection<User>> GetUsersFollowedBy(string follow, bool syncItems = false);

        /// <summary>
        /// Obtiene los seguidores de un usuario
        /// </summary>
        /// <param name="follow">El usuario seguido</param>
        /// <param name="syncItems">Indica si actualizar la tabla local antes de realizar la operación</param>
        /// <returns>Una lista filtrada de usuarios</returns>
        Task<ObservableCollection<User>> GetUsersFollowing(string follow, bool syncItems = false);

        /// <summary>
        /// Inserta un seguimiento a la tabla
        /// </summary>
        /// <param name="item">El seguimiento a insertar</param>
        Task SaveFollowAsync(Follow item);
    }
}