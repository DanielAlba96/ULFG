#define OFFLINE_SYNC_ENABLED

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices;
using ULFG.Core.Data.Item;
using Plugin.Connectivity;
using ULFG.Core.Data.ItemManager.Sync;

#if OFFLINE_SYNC_ENABLED
using Microsoft.WindowsAzure.MobileServices.Sync;
#endif

namespace ULFG.Core.Data.ItemManager.Impl
{
    /// <summary>
    /// Implemente <see cref="IChatManager"/>
    /// </summary>
    public partial class ChatManager : IChatManager
    {
        static IChatManager defaultInstance = new ChatManager();
        readonly MobileServiceClient client;
        readonly IMessageManager messageManager = MessageManager.DefaultManager;

#if OFFLINE_SYNC_ENABLED
        readonly IMobileServiceSyncTable<Chat> chatTable;
#else
        readonly IMobileServiceTable<Chat> chatTable;
#endif
        private ChatManager()
        {
#if OFFLINE_SYNC_ENABLED
            this.client = SyncClientProvider.DefaultInstance.Client;
            this.chatTable = client.GetSyncTable<Chat>();
#else
            this.client = new MobileServiceClient(Constants.ApplicationURL);
            this.chatTable = client.GetTable<Chat>();
#endif
        }

        /// <summary>
        /// Insntancia unica de la clase
        /// </summary>
        public static IChatManager DefaultManager
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
        /// Implementación de <see cref="IChatManager.GetChatsAsync(bool)"/>
        /// </summary>
        public async Task<ObservableCollection<Chat>> GetChatsAsync(bool syncItems = false)
        {
            try
            {
#if OFFLINE_SYNC_ENABLED
                if (syncItems)
                {
                    await this.SyncAsync();
                }
#endif
                IEnumerable<Chat> items = await chatTable
                    .ToEnumerableAsync();

                return new ObservableCollection<Chat>(items);
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
        /// Implementación de <see cref="IChatManager.GetChatByIdAsync(string, bool)"/>
        /// </summary>
        public async Task<Chat> GetChatByIdAsync(string id, bool syncItems = false)
        {
            try
            {
#if OFFLINE_SYNC_ENABLED
                if (syncItems)
                {
                    await this.SyncAsync();
                }
#endif
                IEnumerable<Chat> items = await chatTable
                    .Where(ch => ch.Id == id)
                    .ToEnumerableAsync();
                if (items.Count() == 0)
                    return null;
                else
                    return items.Single();
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
        /// Implementación de <see cref="IChatManager.GetChatsByMemberIdAsync(string, bool)"/>
        /// </summary>
        public async Task<ObservableCollection<Chat>> GetChatsByMemberIdAsync(string memberId, bool syncItems = false)
        {
            try
            {
#if OFFLINE_SYNC_ENABLED
                if (syncItems)
                {
                    await this.SyncAsync();
                }
#endif
                var items = await chatTable
                    .Where(ch => (ch.Member1_id == memberId && !ch.Member1_deleted) || (ch.Member2_id == memberId && !ch.Member2_deleted))
                    .ToListAsync();

                return new ObservableCollection<Chat>(items);
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
        /// Implementación de <see cref="IChatManager.GetChatsByMembersAsync(string, string, bool)"/>
        /// </summary>
        public async Task<Chat> GetChatsByMembersAsync(string member1, string member2, bool syncItems = false)
        {
            try
            {
#if OFFLINE_SYNC_ENABLED
                if (syncItems)
                {
                    await this.SyncAsync();
                }
#endif
                var items = await chatTable
                    .Where(ch => ch.Member1_id == member1 && ch.Member2_id == member2 || ch.Member1_id == member2 && ch.Member2_id == member1)
                    .ToEnumerableAsync();
                if (items.Count() == 0)
                    return null;
                else
                    return items.Single();
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
        /// Implementación de <see cref="IChatManager.SaveChatAsync(Chat)"/>
        /// </summary>
        public async Task<Chat> SaveChatAsync(Chat item)
        {
            item.UpdatedInClient = DateTime.Now;
            if (item.Id == null)
                await chatTable.InsertAsync(item);
            else
                await chatTable.UpdateAsync(item);

            if (CrossConnectivity.Current.IsConnected)
                await this.SyncAsync();
            return item;
        }

        /// <summary>
        /// Implementación de <see cref="IChatManager.DeleteChat(Chat)"/>
        /// </summary>
        public async Task DeleteChat(Chat chat)
        {
            await messageManager.DeleteAllFromChat(chat.Id);
            await chatTable.DeleteAsync(chat);
            if (CrossConnectivity.Current.IsConnected)
                await this.SyncAsync();
        }

        /// <summary>
        /// Implementación de <see cref="IChatManager.DeleteAllFromUser(User)"/>
        /// </summary>
        public async Task DeleteAllFromUser(User user)
        {
            var list = await chatTable.Where(c => c.Member1_id == user.Id || c.Member2_id == user.Id).ToListAsync();
            list.ForEach(async m => await DeleteChat(m));
            if (CrossConnectivity.Current.IsConnected)
                await this.SyncAsync();
        }

#if OFFLINE_SYNC_ENABLED
        /// <summary>
        /// Sincroniza la tabla local con el servidor
        /// </summary>
        public async Task SyncAsync()
        {
            await SyncClientProvider.DefaultInstance.SyncAsync(SyncChats());
        }

        /// <summary>
        /// Acción de sincronización de la tabla
        /// </summary>
        private async Task SyncChats()
        {
            await this.client.SyncContext.PushAsync();

            await this.chatTable.PullAsync(
                //The first parameter is a query name that is used internally by the client SDK to implement incremental sync.
                //Use a different query name for each unique query in your program
                "allChats",
                this.chatTable.CreateQuery());
        }
#endif
    }
}
