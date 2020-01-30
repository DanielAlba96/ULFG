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
    /// Implemente <see cref="IBlockManager"/>
    /// </summary>
    public partial class BlockManager : IBlockManager
    {
        static IBlockManager defaultInstance = new BlockManager();
        readonly MobileServiceClient client;

#if OFFLINE_SYNC_ENABLED
        readonly IMobileServiceSyncTable<Block> blockTable;
#else
        readonly IMobileServiceTable<Block> blockTable;
#endif
        private BlockManager()
        {
#if OFFLINE_SYNC_ENABLED
            this.client = SyncClientProvider.DefaultInstance.Client;
            this.blockTable = client.GetSyncTable<Block>();
#else
            this.client = new MobileServiceClient(Constants.ApplicationURL);
            this.blockTable = client.GetTable<Block>();
#endif
        }

        /// <summary>
        /// Insntancia unica de la clase
        /// </summary>
        public static IBlockManager DefaultManager
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
        /// Implementación de <see cref="IBlockManager.GetBlocksAsync(bool)"/>
        /// </summary>
        public async Task<ObservableCollection<Block>> GetBlocksAsync(bool syncItems = false)
        {
            try
            {
#if OFFLINE_SYNC_ENABLED
                if (syncItems)
                {
                    await this.SyncAsync();
                }
#endif
                IEnumerable<Block> items = await blockTable
                    .ToEnumerableAsync();

                return new ObservableCollection<Block>(items);
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
        /// Implementación de <see cref="IBlockManager.GetBlocksByBlockingUserAsync(string, bool)"/>
        /// </summary>
        public async Task<ObservableCollection<User>> GetBlocksByBlockingUserAsync(string user, bool syncItems = false)
        {
            try
            {
#if OFFLINE_SYNC_ENABLED
                if (syncItems)
                {
                    await this.SyncAsync();
                }
#endif
                IEnumerable<string> items = await blockTable
                    .Where(x => x.BlockingUser == user)
                    .Select(x => x.BlockedUser)
                    .ToEnumerableAsync();

                var users = await UserManager.DefaultManager.GetUsersAsync();

                var result = users.Join(items, x => x.Id, y => y, (us, id) => us);

                return new ObservableCollection<User>(result);
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
        /// Implementación de <see cref="IBlockManager.GetBlocksByBlockedUserAsync(string, bool)"/>
        /// </summary>
        public async Task<ObservableCollection<User>> GetBlocksByBlockedUserAsync(string user, bool syncItems = false)
        {
            try
            {
#if OFFLINE_SYNC_ENABLED
                if (syncItems)
                {
                    await this.SyncAsync();
                }
#endif
                IEnumerable<string> items = await blockTable
                    .Where(x => x.BlockedUser == user)
                    .Select(x => x.BlockingUser)
                    .ToEnumerableAsync();

                var users = await UserManager.DefaultManager.GetUsersAsync();

                var result = users.Join(items, x => x.Id, y => y, (us, id) => us);

                return new ObservableCollection<User>(result);
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
        /// Implementación de <see cref="IBlockManager.GetBlocksByBothSidesAsync(string, string, bool)"/>
        /// </summary>
        public async Task<Block> GetBlocksByBothSidesAsync(string blocking, string blocked, bool syncItems = false)
        {
            try
            {
#if OFFLINE_SYNC_ENABLED
                if (syncItems)
                {
                    await this.SyncAsync();
                }
#endif
                IEnumerable<Block> items = await blockTable
                    .Where(x => x.BlockingUser == blocking && x.BlockedUser == blocked)
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
        /// Implementación de <see cref="IBlockManager.SaveBlockAsync(Block)"/>
        /// </summary>
        public async Task SaveBlockAsync(Block item)
        {
            item.UpdatedInClient = DateTime.Now;
            if (item.Id == null)
                await blockTable.InsertAsync(item);
            else
                await blockTable.UpdateAsync(item);
            if (CrossConnectivity.Current.IsConnected)
                await this.SyncAsync();
        }

        /// <summary>
        /// Implementación de <see cref="IBlockManager.DeleteBlockAsync(Block)"/>
        /// </summary>
        public async Task DeleteBlockAsync(Block item)
        {
            Block block;
            if (item.Id == null)
                block = await GetBlocksByBothSidesAsync(item.BlockingUser, item.BlockedUser);
            else
                block = item;
            await blockTable.DeleteAsync(block);

            if (CrossConnectivity.Current.IsConnected)
                await this.SyncAsync();
        }

        /// <summary>
        /// Implementación de <see cref="IBlockManager.DeleteAllBlocksFromUser(User)"/>
        /// </summary>
        public async Task DeleteAllBlocksFromUser(User user)
        {
            var list = await blockTable.Where(b => b.BlockingUser == user.Id || b.BlockedUser == user.Id).ToListAsync();
            list.ForEach(async c => await DeleteBlockAsync(c));
            if (CrossConnectivity.Current.IsConnected)
                await this.SyncAsync();
        }

#if OFFLINE_SYNC_ENABLED  
        /// <summary>
        /// Sincroniza la tabla local con el servidor
        /// </summary>
        public async Task SyncAsync()
        {
            await SyncClientProvider.DefaultInstance.SyncAsync(SyncBlocks());
        }

        /// <summary>
        /// Acción de sincronización de la tabla
        /// </summary>
        private async Task SyncBlocks()
        {
            await this.client.SyncContext.PushAsync();

            await this.blockTable.PullAsync(
                //The first parameter is a query name that is used internally by the client SDK to implement incremental sync.
                //Use a different query name for each unique query in your program
                "allBlocks",
                this.blockTable.CreateQuery());
        }
#endif
    }
}
