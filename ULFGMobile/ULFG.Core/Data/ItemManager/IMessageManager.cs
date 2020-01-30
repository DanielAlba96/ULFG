using System.Collections.ObjectModel;
using System.Threading.Tasks;
using ULFG.Core.Data.Item;

namespace ULFG.Core.Data.ItemManager
{
    /// <summary>
    /// Define las operaciones sobre la tabla Messages
    /// </summary>
    public interface IMessageManager
    {
        /// <summary>
        /// Borra todos los mensajes de un usuario
        /// </summary>
        /// <param name="user">El usuario cuyos mensajes se quiere borrar</param>
        Task DeleteAllFromUser(User user);

        /// <summary>
        /// Borra todos los mensajes de un chat
        /// </summary>
        /// <param name="chatId">El id del chat cuyos mensajes se quiere borrar</param>
        Task DeleteAllFromChat(string chatId);

        /// <summary>
        /// Borra todos los mensajes de un gremio
        /// </summary>
        /// <param name="guildId">El id del gremio cuyos mensajes se quiere borrar</param>
        Task DeleteAllFromGuild(string guildId);

        /// <summary>
        /// Borra un mensaje
        /// </summary>
        /// <param name="item">El mensaje a borrar</param>
        Task DeleteMsgAsync(Message item);

        /// <summary>
        /// Obtiene todos los mensajes
        /// </summary>
        /// <param name="syncItems">Indica si actualizar la tabla local antes de realizar la operación</param>
        /// <returns>Una lista completa de mensajes</returns>
        Task<ObservableCollection<Message>> GetMessagesAsync(bool syncItems = false);

        /// <summary>
        /// Busca mensajes por gremio
        /// </summary>
        /// <param name="guildid">El id del gremio cuyos mensajes se quiere buscar</param>
        /// <param name="syncItems">Indica si actualizar la tabla local antes de realizar la operación</param>
        /// <returns>Una lista filtrada de mensajes</returns>
        Task<ObservableCollection<Message>> GetMessagesByGuildIdAsync(string guildid, bool syncItems = false);

        /// <summary>
        /// Busca mensajes por chat
        /// </summary>
        /// <param name="chatid">El id del chat cuyos mensajes se quiere buscar</param>
        /// <param name="syncItems"></param>
        /// <returns>Una lista filtrada de mensajes</returns>
        Task<ObservableCollection<Message>> GetMessagesByChatIdAsync(string chatid, bool syncItems = false);

        /// <summary>
        /// Busca un mensaje por id
        /// </summary>
        /// <param name="id">El id del mensaje a buscar</param>
        /// <param name="syncItems">Indica si actualizar la tabla local antes de realizar la operación</param>
        /// <returns>El mensaje buscado o null si no existe</returns>
        Task<Message> GetMessageByIdAsync(string id, bool syncItems = false);

        /// <summary>
        /// Obtiene el ultimo mensaje de un chat que no es de un usuario
        /// </summary>
        /// <param name="user">El id del usuario al que no pertenece el mensaje</param>
        /// <param name="chatid">El id del chatcuyo último mensaje se quiere obtener</param>
        /// <param name="syncItems">Indica si actualizar la tabla local antes de realizar la operación</param>
        /// <returns>El mensaje buscado o null si no existe</returns>
        Task<Message> GetLastMessageOfChatAsync(string user, string chatid, bool syncItems = false);

        /// <summary>
        /// Inserta un mensaje en la tabla
        /// </summary>
        /// <param name="item">El mensaje a insertar</param>
        Task SaveMsgAsync(Message item);
    }
}