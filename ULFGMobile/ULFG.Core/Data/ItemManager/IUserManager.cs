using System.Collections.ObjectModel;
using System.Threading.Tasks;
using ULFG.Core.Data.Item;

namespace ULFG.Core.Data.ItemManager
{
    /// <summary>
    /// Define las operaciones sobre la tabla Users
    /// </summary>
    public interface IUserManager
    {
        /// <summary>
        /// Borra un usuario
        /// </summary>
        /// <param name="item">El usuario a borrar</param>
        Task DeleteUserAsync(User item);

        /// <summary>
        /// Busca un usuario por id
        /// </summary>
        /// <param name="id">El id del usuario a buscar</param>
        /// <param name="syncItems">Indica si actualizar la tabla local antes de realizar la operación</param>
        Task<User> GetUserByIdAsync(string id, bool syncItems = false);

        /// <summary>
        /// Busca un usuario por username
        /// </summary>
        /// <param name="name">El username del usuario a buscar</param>
        /// <param name="syncItems">Indica si actualizar la tabla local antes de realizar la operación</param>
        Task<User> GetUserByNameAsync(string name, bool syncItems = false);

        /// <summary>
        /// Busca un usuario por username y password
        /// </summary>
        /// <param name="login">El username del usuario</param>
        /// <param name="password">La contraseña del usuario</param>
        /// <param name="syncItems">Indica si actualizar la tabla local antes de realizar la operación</param>
        /// <returns>El usuario a buscar o null si no existe</returns>
        Task<User> GetUserByNameAndPassword(string login, string password, bool syncItems = false);

        /// <summary>
        /// Busca usuarios que contengan una determinada cadena en su username o biografia
        /// </summary>
        /// <param name="word">La cadena a buscar</param>
        /// <param name="user">El usuario actual</param>
        /// <param name="syncItems">Indica si actualizar la tabla local antes de realizar la operación</param>
        /// <returns>Una lista filtrada de usuarios</returns>
        Task<ObservableCollection<User>> GetUserByWordAsync(string word, string user, bool syncItems = false);

        /// <summary>
        /// Obtiene todos los usuarios
        /// </summary>
        /// <param name="syncItems">Indica si actualizar la tabla local antes de realizar la operación</param>>
        /// <returns>Una lista completa de usuarios</returns>
        Task<ObservableCollection<User>> GetUsersAsync(bool syncItems = false);

        /// <summary>
        /// Obtiene todos los usuarios excepto que se pasa como parámetro
        /// </summary>
        /// <param name="user">El id del usuario excluir</param>
        /// <param name="syncItems">Indica si actualizar la tabla local antes de realizar la operación</param>
        /// <returns>Una lista filtrada de usuarios</returns>
        Task<ObservableCollection<User>> GetUsersExceptActualAsync(string user, bool syncItems = false);

        /// <summary>
        /// Inserta un usuario en la tabla
        /// </summary>
        /// <param name="item">El usuario a insertar</param>
        Task SaveUserAsync(User item);
    }
}