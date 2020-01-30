#define OFFLINE_SYNC_ENABLED

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices;
using ULFG.Core.Data.Item;
using Plugin.Connectivity;
using System.Linq;
using ULFG.Core.Data.ItemManager.Sync;

#if OFFLINE_SYNC_ENABLED
using Microsoft.WindowsAzure.MobileServices.Sync;
#endif

namespace ULFG.Core.Data.ItemManager.Impl
{
    /// <summary>
    /// Implemente <see cref="IMessageManager"/>
    /// </summary>
    public partial class MessageManager : IMessageManager
    {
        static IMessageManager defaultInstance = new MessageManager();
        readonly MobileServiceClient client;

#if OFFLINE_SYNC_ENABLED
        readonly IMobileServiceSyncTable<Message> messageTable;
#else
        readonly IMobileServiceTable<Message> messageTable;
#endif
        private MessageManager()
        {
#if OFFLINE_SYNC_ENABLED
            this.client = SyncClientProvider.DefaultInstance.Client;
            this.messageTable = client.GetSyncTable<Message>();
#else
            this.client = new MobileServiceClient(Constants.ApplicationURL);
            this.messageTable = client.GetTable<Message>();
#endif
        }

        /// <summary>
        /// Instancia unica de la clase
        /// </summary>
        public static IMessageManager DefaultManager
        {
            get
            {
                return defaultInstance;
            }
            private set
            {
                defaultInstance = value;
            }
        }

        /// <summary>
        /// Implementación de <see cref="IMessageManager.GetMessagesAsync(bool)"/>
        /// </summary>
        public async Task<ObservableCollection<Message>> GetMessagesAsync(bool syncItems = false)
        {
            try
            {
#if OFFLINE_SYNC_ENABLED
                if (syncItems)
                {
                    await this.SyncAsync();
                }
#endif
                IEnumerable<Message> items = await messageTable
                    .ToEnumerableAsync();

                return new ObservableCollection<Message>(items);
            }
            catch (MobileServiceInvalidOperationException msioe)
            {
                Debug.WriteLine(@"Invalid sync operation: {0}", msioe.Message);
            }
            catch (Exception e)
            {
                Debug.WriteLine(@"Sync error: {0}", e.Message);
            }
            return null;
        }

        /// <summary>
        /// Implementación de <see cref="IMessageManager.GetMessagesByChatIdAsync(string, bool)"/>
        /// </summary>
        public async Task<ObservableCollection<Message>> GetMessagesByChatIdAsync(string chatid, bool syncItems = false)
        {
            try
            {
#if OFFLINE_SYNC_ENABLED
                if (syncItems)
                {
                    await this.SyncAsync();
                }
#endif
                IEnumerable<Message> items = await messageTable
                    .Where(m => m.Chat_Id == chatid)
                    .OrderBy(m => m.CreationDate)
                    .ToEnumerableAsync();

                return new ObservableCollection<Message>(items);
            }
            catch (MobileServiceInvalidOperationException msioe)
            {
                Debug.WriteLine(@"Invalid sync operation: {0}", msioe.Message);
            }
            catch (Exception e)
            {
                Debug.WriteLine(@"Sync error: {0}", e.Message);
            }
            return null;
        }

        /// <summary>
        /// Implementación de <see cref="IMessageManager.GetMessagesByGuildIdAsync(string, bool)"/>
        /// </summary>
        public async Task<ObservableCollection<Message>> GetMessagesByGuildIdAsync(string guildid, bool syncItems = false)
        {
            try
            {
#if OFFLINE_SYNC_ENABLED
                if (syncItems)
                {
                    await this.SyncAsync();
                }
#endif
                IEnumerable<Message> items = await messageTable
                    .Where(m => m.Guild_Id == guildid)
                    .OrderBy(m => m.CreationDate)
                    .ToEnumerableAsync();

                return new ObservableCollection<Message>(items);
            }
            catch (MobileServiceInvalidOperationException msioe)
            {
                Debug.WriteLine(@"Invalid sync operation: {0}", msioe.Message);
            }
            catch (Exception e)
            {
                Debug.WriteLine(@"Sync error: {0}", e.Message);
            }
            return null;
        }

        /// <summary>
        /// Implementación de <see cref="IMessageManager.GetMessageByIdAsync(string, bool)"/>
        /// </summary>
        public async Task<Message> GetMessageByIdAsync(string id, bool syncItems = false)
        {
            try
            {
#if OFFLINE_SYNC_ENABLED
                if (syncItems)
                {
                    await this.SyncAsync();
                }
#endif
                var item = await messageTable
                    .LookupAsync(id);

                return item;
            }
            catch (MobileServiceInvalidOperationException msioe)
            {
                Debug.WriteLine(@"Invalid sync operation: {0}", msioe.Message);
            }
            catch (Exception e)
            {
                Debug.WriteLine(@"Sync error: {0}", e.Message);
            }
            return null;
        }

        /// <summary>
        /// Implementación de <see cref="IMessageManager.GetLastMessageOfChatAsync(string, string, bool)"/>
        /// </summary>
        public async Task<Message> GetLastMessageOfChatAsync(string user, string chatid, bool syncItems = false)
        {
            try
            {
#if OFFLINE_SYNC_ENABLED
                if (syncItems)
                {
                    await this.SyncAsync();
                }
#endif
                IEnumerable<Message> items = await messageTable
                    .Where(m => m.Chat_Id == chatid && m.Creator_Id != user)
                    .OrderByDescending(m => m.CreationDate)
                    .ToEnumerableAsync();

                return items.First();
            }
            catch (MobileServiceInvalidOperationException msioe)
            {
                Debug.WriteLine(@"Invalid sync operation: {0}", msioe.Message);
            }
            catch (Exception e)
            {
                Debug.WriteLine(@"Sync error: {0}", e.Message);
            }
            return null;
        }

        /// <summary>
        /// Implementación de <see cref="IMessageManager.SaveMsgAsync(Message)"/>
        /// </summary>
        public async Task SaveMsgAsync(Message item)
        {
            item.UpdatedInClient = DateTime.Now;
            if (item.Id == null)
                await messageTable.InsertAsync(item);
            else
                await messageTable.UpdateAsync(item);
            if (CrossConnectivity.Current.IsConnected)
                await this.SyncAsync();
        }

        /// <summary>
        /// Implementación de <see cref="IMessageManager.DeleteMsgAsync(Message)"/>
        /// </summary>
        public async Task DeleteMsgAsync(Message item)
        {
            await messageTable.DeleteAsync(item);
            if (CrossConnectivity.Current.IsConnected)
                await this.SyncAsync();
        }

        /// <summary>
        /// Implementación de <see cref="IMessageManager.DeleteAllFromUser(User)"/>
        /// </summary>
        public async Task DeleteAllFromUser(User user)
        {
            var list = await messageTable.Where(m => m.Creator_Id == user.Id).ToListAsync();
            list.ForEach(async m => await DeleteMsgAsync(m));
            if (CrossConnectivity.Current.IsConnected)
                await this.SyncAsync();
        }

        /// <summary>
        /// Implementación de <see cref="IMessageManager.DeleteAllFromChat(string)"/>
        /// </summary>
        public async Task DeleteAllFromChat(string chatId)
        {
            var list = await messageTable.Where(m => m.Chat_Id == chatId).ToListAsync();
            list.ForEach(async m => await DeleteMsgAsync(m));
            if (CrossConnectivity.Current.IsConnected)
                await this.SyncAsync();
        }

        /// <summary>
        /// Implementación de <see cref="IMessageManager.DeleteAllFromGuild(string)"/>
        /// </summary>
        /// <param name="guildId">id del gremio</param>
        /// <returns></returns>
        public async Task DeleteAllFromGuild(string guildId)
        {
            var list = await messageTable.Where(m => m.Guild_Id == guildId).ToListAsync();
            list.ForEach(async m => await DeleteMsgAsync(m));
            if (CrossConnectivity.Current.IsConnected)
                await this.SyncAsync();
        }

#if OFFLINE_SYNC_ENABLED
        /// <summary>
        /// Sincroniza la tabla local con el servidor
        /// </summary>
        public async Task SyncAsync()
        {
            await SyncClientProvider.DefaultInstance.SyncAsync(SyncMessages());
        }

        /// <summary>
        /// Acción de sincronización de la tabla
        /// </summary>
        private async Task SyncMessages()
        {
            await this.client.SyncContext.PushAsync();

            await this.messageTable.PullAsync(
                //The first parameter is a query name that is used internally by the client SDK to implement incremental sync.
                //Use a different query name for each unique query in your program
                "allMessages",
                this.messageTable.CreateQuery());
        }
#endif
    }
}
