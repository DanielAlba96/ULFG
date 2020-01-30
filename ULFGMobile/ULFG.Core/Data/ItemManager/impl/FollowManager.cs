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
    /// Implemente <see cref="IFollowManager"/>
    /// </summary>
    public partial class FollowManager : IFollowManager
    {
        static IFollowManager defaultInstance = new FollowManager();
        readonly MobileServiceClient client;

#if OFFLINE_SYNC_ENABLED
        readonly IMobileServiceSyncTable<Follow> followTable;
#else
        readonly IMobileServiceTable<Follow> followTable;
#endif
        private FollowManager()
        {
#if OFFLINE_SYNC_ENABLED
            this.client = SyncClientProvider.DefaultInstance.Client;
            this.followTable = client.GetSyncTable<Follow>();
#else
            this.client = new MobileServiceClient(Constants.ApplicationURL);
            this.followTable = client.GetTable<Follow>();
#endif
        }

        /// <summary>
        /// Instancia unica de la clase
        /// </summary>
        public static IFollowManager DefaultManager
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
        /// Implementación de <see cref="IFollowManager.GetFollowsAsync(bool)"/>
        /// </summary>
        public async Task<ObservableCollection<Follow>> GetFollowsAsync(bool syncItems = false)
        {
            try
            {
#if OFFLINE_SYNC_ENABLED
                if (syncItems)
                {
                    await this.SyncAsync();
                }
#endif
                IEnumerable<Follow> items = await followTable
                    .ToEnumerableAsync();

                return new ObservableCollection<Follow>(items);
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
        /// Implementación de <see cref="IFollowManager.GetFollowsByBothSidesAsync(string, string, bool)"/>
        /// </summary>
        public async Task<Follow> GetFollowsByBothSidesAsync(string following, string followed, bool syncItems = false)
        {
            try
            {
#if OFFLINE_SYNC_ENABLED
                if (syncItems)
                {
                    await this.SyncAsync();
                }
#endif
                IEnumerable<Follow> items = await followTable
                    .Where(x => x.FollowingUser == following && x.FollowedUser == followed)
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
        /// Implementación de <see cref="IFollowManager.GetUsersFollowedBy(string, bool)"/>
        /// </summary>
        public async Task<ObservableCollection<User>> GetUsersFollowedBy(string follow, bool syncItems = false)
        {
            try
            {
#if OFFLINE_SYNC_ENABLED
                if (syncItems)
                {
                    await this.SyncAsync();
                }
#endif
                IEnumerable<string> items = await followTable
                    .Where(x => x.FollowingUser == follow)
                    .Select(x => x.FollowedUser)
                    .ToEnumerableAsync();

                return new ObservableCollection<User>(await GetUsersFromFollows(items));
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
        /// Implementación de <see cref="IFollowManager.GetUsersFollowing(string, bool)"/>
        /// </summary>
        public async Task<ObservableCollection<User>> GetUsersFollowing(string follow, bool syncItems = false)
        {
            try
            {
#if OFFLINE_SYNC_ENABLED
                if (syncItems)
                {
                    await this.SyncAsync();
                }
#endif
                IEnumerable<string> items = await followTable
                    .Where(x => x.FollowedUser == follow)
                    .Select(x => x.FollowingUser)
                    .ToEnumerableAsync();

                return new ObservableCollection<User>(await GetUsersFromFollows(items));
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
        /// Devuelve una lista de usuarios a partir de una lista de ids de seguimientos
        /// </summary>
        /// <param name="items">Lista de ids de seguimentos</param>
        /// <returns>Una lista filtrada de usuarios</returns>
        private async Task<IEnumerable<User>> GetUsersFromFollows(IEnumerable<string> items)
        {
            IEnumerable<User> users = await UserManager.DefaultManager.GetUsersAsync();
            IEnumerable<User> result = users.Join(items, x => x.Id, y => y, (us, id) => us);
            return result;
        }

        /// <summary>
        /// Implementación de <see cref="IFollowManager.SaveFollowAsync(Follow)"/>
        /// </summary>
        public async Task SaveFollowAsync(Follow item)
        {
            item.UpdatedInClient = DateTime.Now;
            if (item.Id == null)
                await followTable.InsertAsync(item);
            else
                await followTable.UpdateAsync(item);

            if (CrossConnectivity.Current.IsConnected)
                await this.SyncAsync();
        }

        /// <summary>
        /// Implementación de <see cref="IFollowManager.DeleteFollowAsync(Follow)"/>
        /// </summary>
        public async Task DeleteFollowAsync(Follow item)
        {
            await followTable.DeleteAsync(item);
            if (CrossConnectivity.Current.IsConnected)
                await this.SyncAsync();
        }

        /// <summary>
        /// Implementación de <see cref="IFollowManager.DeleteAllFollowsFromUser(User)"/>
        /// </summary>
        public async Task DeleteAllFollowsFromUser(User user)
        {
            var list = await followTable.Where(f => f.FollowingUser == user.Id || f.FollowedUser == user.Id).ToListAsync();
            list.ForEach(async c => await DeleteFollowAsync(c));
            if (CrossConnectivity.Current.IsConnected)
                await this.SyncAsync();
        }


#if OFFLINE_SYNC_ENABLED
        /// <summary>
        /// Sincroniza la tabla local con el servidor
        /// </summary>
        public async Task SyncAsync()
        {
            await SyncClientProvider.DefaultInstance.SyncAsync(SyncFollows());
        }

        /// <summary>
        /// Acción de sincronización de la tabla
        /// </summary>
        private async Task SyncFollows()
        {
            await this.client.SyncContext.PushAsync();

            await this.followTable.PullAsync(
                //The first parameter is a query name that is used internally by the client SDK to implement incremental sync.
                //Use a different query name for each unique query in your program
                "allFollows",
                this.followTable.CreateQuery());
        }
#endif
    }
}