using System.Collections.ObjectModel;
using System.Threading.Tasks;
using ULFG.Core.Data.Item;

namespace ULFG.Core.Data.ItemManager
{
    /// <summary>
    /// Define las operaciones de la tabla Blocks
    /// </summary>
    public interface IBlockManager
    {
        /// <summary>
        /// Borra todos los bloqueos de un usuario
        /// </summary>
        /// <param name="user">El usuario cuyos bloqueos se quiere borrar</param>
        Task DeleteAllBlocksFromUser(User user);

        /// <summary>
        /// Borra un bloqueo
        /// </summary>
        /// <param name="item">El bloqueo a borrar</param>
        Task DeleteBlockAsync(Block item);

        /// <summary>
        /// Obtiene todos los bloqueos
        /// </summary>
        /// <param name="syncItems">Indica si actualizar la tabla antes de realizar la operacion</param>
        /// <returns>Una lista completa de bloqueos</returns>
        Task<ObservableCollection<Block>> GetBlocksAsync(bool syncItems = false);

        /// <summary>
        /// Obtiene todos los bloqueos de un usuario
        /// </summary>
        /// <param name="user">El usuario cuyos bloqueos se quiere obtener</param>
        /// <param name="syncItems">Indica si actualizar la tabla antes de realizar la operacion</param>
        /// <returns>Una lista filtrada de usuarios</returns>
        Task<ObservableCollection<User>> GetBlocksByBlockedUserAsync(string user, bool syncItems = false);

        /// <summary>
        /// Obtiene todos los bloqueos hacia un usuario
        /// </summary>
        /// <param name="user">El usuario cuyos bloqueos se quiere obtener</param>
        /// <param name="syncItems">Indica si actualizar la tabla antes de realizar la operacion</param>
        /// <returns>Una lista filtrada de usuarios</returns>
        Task<ObservableCollection<User>> GetBlocksByBlockingUserAsync(string user, bool syncItems = false);

        /// <summary>
        /// Obtiene un bloqueo determinado
        /// </summary>
        /// <param name="blocking">El identificador del usuario que bloquea</param>
        /// <param name="blocked">El identificador del usuario bloqueado</param>
        /// <param name="syncItems">Indica si actualizar la tabla antes de realizar la operacion</param>
        /// <returns>El bloqueo busqueda o null si no existe</returns>
        Task<Block> GetBlocksByBothSidesAsync(string blocking, string blocked, bool syncItems = false);

        /// <summary>
        /// Inserta un bloqueo a la tabla
        /// </summary>
        /// <param name="item">El bloqueo a insertar</param>
        Task SaveBlockAsync(Block item);
    }
}