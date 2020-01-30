using Plugin.Connectivity;
using System.Threading.Tasks;
using ULFG.Core.Data.Item;
using ULFG.Core.Data.ItemManager;
using ULFG.Core.Data.ItemManager.Impl;

namespace ULFG.Core.Logic
{
    /// <summary>
    /// Clase que contiene toda la lógica de los mensajes y chats
    /// </summary>
    public class MessageOperations
    {
        readonly IUserManager userManager;
        readonly IChatManager chatManager;
        readonly IPublicationManager publicationManager;
        readonly IMessageManager messageManager;

        /// <summary>
        /// Constructor con parmametros
        /// </summary>
        public MessageOperations(IUserManager userManager, IChatManager chatManager, 
            IPublicationManager publicationManager, IMessageManager messageManager)
        {
            this.userManager = userManager;
            this.chatManager = chatManager;
            this.publicationManager = publicationManager;
            this.messageManager = messageManager;
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public MessageOperations()
        {
            this.userManager = UserManager.DefaultManager;
            this.chatManager = ChatManager.DefaultManager;
            this.publicationManager = PublicationManager.DefaultManager;
            this.messageManager = MessageManager.DefaultManager;
        }

        /// <summary>
        /// Envía un nuevo mensaje creando un chat si no existe
        /// </summary>
        /// <param name="currentUser">El identificador del usuario emisor</param>
        /// <param name="receiver">El identificador del usuario receptor</param>
        /// <param name="message">El contenido del mensaje</param>
        /// <returns>El identificador del nuevo mensaje</returns>
        public async Task<string> SendMessage(string currentUser, string receiver, string message)
        {            
            var chat = await chatManager.GetChatsByMembersAsync(currentUser, receiver, CrossConnectivity.Current.IsConnected);

            if (chat == null)
            {
                chat = new Chat
                {
                    Member1_id = currentUser,
                    Member2_id = receiver
                };
                chat = await chatManager.SaveChatAsync(chat);
            }
            else
            {
                if (chat.Member1_id.Equals(currentUser) && chat.Member1_deleted)
                {
                    chat.Member1_deleted = false;
                    chat = await chatManager.SaveChatAsync(chat);
                }
                else if (chat.Member2_id.Equals(currentUser) && chat.Member2_deleted)
                {
                    chat.Member2_deleted = false;
                    chat = await chatManager.SaveChatAsync(chat);
                }
            }
            
           Message m = new Message
            {
                Text = message,
                Chat_Id = chat.Id,
                Creator_Id = currentUser
            };
            await messageManager.SaveMsgAsync(m);       
            return m.Id;
        }

        /// <summary>
        /// Borra un chat
        /// </summary>
        /// <param name="chatId">El identificador del chat a borrar</param>
        /// <param name="userId">El identificador del usuario que borra el chat</param>
        public async Task DeleteChat(string chatId, string userId)
        {
            var chat = await chatManager.GetChatByIdAsync(chatId, CrossConnectivity.Current.IsConnected);
            if (chat.Member1_id.Equals(userId))
                chat.Member1_deleted = true;
            else
                chat.Member2_deleted = true;

            if (chat.Member1_deleted && chat.Member2_deleted)
                await chatManager.DeleteChat(chat);
            else
                await chatManager.SaveChatAsync(chat);
        }
    }
}
