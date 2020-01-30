using System.Collections.ObjectModel;
using System.Threading.Tasks;
using ULFG.Core.Data.Item;

namespace ULFG.Core.Data.ItemManager
{
    /// <summary>
    /// Define las operaciones de la tabla Chats
    /// </summary>
    public interface IChatManager
    {
        /// <summary>
        /// Borra todos los chats de un usuario
        /// </summary>
        /// <param name="user">El usuario cuyos chats se quiere borrar</param>
        Task DeleteAllFromUser(User user);

        /// <summary>
        /// Borra un chat
        /// </summary>
        /// <param name="chat">El chat a borrar</param>
        Task DeleteChat(Chat chat);

        /// <summary>
        /// Busca un chat por su id
        /// </summary>
        /// <param name="id">El id del chat a buscar </param>
        /// <param name="syncItems">Indica si actualizar la tabla local antes de realizar la operación</param>
        /// <returns>El chat buscado si se encuentra, en caso contrario null</returns>
        Task<Chat> GetChatByIdAsync(string id, bool syncItems = false);

        /// <summary>
        /// Obtiene todos los chats
        /// </summary>
        /// <param name="syncItems">Indica si actualizar la tabla local antes de realizar la operación</param>
        /// <returns>Una lista completa de chats</returns>
        Task<ObservableCollection<Chat>> GetChatsAsync(bool syncItems = false);

        /// <summary>
        /// Obtiene todos los chats de un usuario
        /// </summary>
        /// <param name="memberId">El id del usuario cuyos chats se quiere buscar</param>
        /// <param name="syncItems">Indica si actualizar la tabla local antes de realizar la operación</param>
        /// <returns>Una lista filtrada de chats</returns>
        Task<ObservableCollection<Chat>> GetChatsByMemberIdAsync(string memberId, bool syncItems = false);

        /// <summary>
        /// Busca un chat concreto entre dos usuarios
        /// </summary>
        /// <param name="member1">El id de uno de los miembros</param>
        /// <param name="member2">El id del otro miembro</param>
        /// <param name="syncItems">Indica si actualizar la tabla local antes de realizar la operación</param>
        /// <returns>El chat buscado o null si no existe</returns>
        Task<Chat> GetChatsByMembersAsync(string member1, string member2, bool syncItems = false);

        /// <summary>
        /// Inserta un chat en la tabla
        /// </summary>
        /// <param name="item">El chat a insertar</param>
        /// <returns>El chat insertado</returns>
        Task<Chat> SaveChatAsync(Chat item);
    }
}