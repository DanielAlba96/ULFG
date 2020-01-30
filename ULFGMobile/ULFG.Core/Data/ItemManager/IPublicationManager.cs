using System.Collections.ObjectModel;
using System.Threading.Tasks;
using ULFG.Core.Data.Item;

namespace ULFG.Core.Data.ItemManager
{
    /// <summary>
    /// Define las operaciones de la tabla Publication
    /// </summary>
    public interface IPublicationManager
    {
        /// <summary>
        /// Borra todas las publicaciones de un usuario
        /// </summary>
        /// <param name="user">El usuario cuyas publicaciones se quiere borrar</param>
        Task DeleteAllPublicationsFromUser(User user);

        /// <summary>
        /// Borra una publicación
        /// </summary>
        /// <param name="item">La publicación a borrar</param>
        Task DeletePubAsync(Publication item);

        /// <summary>
        /// Obtiene todas las publicaciones
        /// </summary>
        /// <param name="syncItems">Indica si actualizar la tabla local antes de realizar la operación</param>
        /// <returns>Una lista completa de publicaciones</returns>
        Task<ObservableCollection<Publication>> GetPublicationsAsync(bool syncItems = false);

        /// <summary>
        /// Busca una publicación por id
        /// </summary>
        /// <param name="id"> El id de la publicación a buscar</param>
        /// <param name="syncItems">Indica si actualizar la tabla local antes de realizar la operación</param>
        /// <returns>La publicación buscada o null si no existe</returns>
        Task<Publication> GetPublicationByIdAsync(string id, bool syncItems = false);

        /// <summary>
        /// Busca todas las publicaciones de un usuario
        /// </summary>
        /// <param name="user">El usuario cuyas publicaciones se quiere buscar</param>
        /// <param name="syncItems">Indica si actualizar la tabla local antes de realizar la operación</param>
        /// <returns>Una lista filtrada de publicaciones</returns>
        Task<ObservableCollection<Publication>> GetPublicationsByUserAsync(string user, bool syncItems = false);

        /// <summary>
        /// Inserta una publicación en la tabla
        /// </summary>
        /// <param name="item">La publicación a insertar</param>
        Task SavePublicationAsync(Publication item);
    }
}