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
    /// Implemente <see cref="IUserManager"/>
    /// </summary>
    public partial class UserManager : IUserManager
    {
        static IUserManager defaultInstance = new UserManager();
        readonly MobileServiceClient client;

#if OFFLINE_SYNC_ENABLED
        readonly IMobileServiceSyncTable<User> userTable;
#else
        readonly IMobileServiceTable<User> userTable;
#endif
        private UserManager()
        {
#if OFFLINE_SYNC_ENABLED
            this.client = SyncClientProvider.DefaultInstance.Client;
            this.userTable = client.GetSyncTable<User>();
#else
            this.client = new MobileServiceClient(Constants.ApplicationURL);
            this.userTable = client.GetTable<User>();
#endif
        }

        /// <summary>
        /// Instancia unica de la clase
        /// </summary>
        public static IUserManager DefaultManager
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
        /// Implementación de <see cref="IUserManager.GetUsersAsync(bool)"/>
        /// </summary>
        public async Task<ObservableCollection<User>> GetUsersAsync(bool syncItems = false)
        {
            try
            {
#if OFFLINE_SYNC_ENABLED
                if (syncItems)
                {
                    await this.SyncAsync();
                }
#endif
                IEnumerable<User> items = await userTable
                    .ToEnumerableAsync();

                return new ObservableCollection<User>(items);
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
        /// Implementación de <see cref="IUserManager.GetUsersExceptActualAsync(string, bool)"/>
        /// </summary>
        public async Task<ObservableCollection<User>> GetUsersExceptActualAsync(string user, bool syncItems = false)
        {
            try
            {
#if OFFLINE_SYNC_ENABLED
                if (syncItems)
                {
                    await this.SyncAsync();
                }
#endif
                IEnumerable<User> items = await userTable
                    .Where(x => x.Id != user)
                    .ToEnumerableAsync();

                return new ObservableCollection<User>(items);
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
        /// Implementación de <see cref="IUserManager.GetUserByNameAsync(string, bool)"/>
        /// </summary>
        public async Task<User> GetUserByNameAsync(String name, bool syncItems = false)
        {
            try
            {
#if OFFLINE_SYNC_ENABLED
                if (syncItems)
                {
                    await this.SyncAsync();
                }
#endif
                IEnumerable<User> items = await userTable
                    .Where(u => u.Username == name)
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
        /// Implementación de <see cref="IUserManager.GetUserByIdAsync(string, bool)"/>
        /// </summary>
        public async Task<User> GetUserByIdAsync(String id, bool syncItems = false)
        {
            try
            {
#if OFFLINE_SYNC_ENABLED
                if (syncItems)
                {
                    await this.SyncAsync();
                }
#endif
                IEnumerable<User> items = await userTable
                    .Where(u => u.Id == id)
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
        /// Implementación de <see cref="IUserManager.GetUserByWordAsync(string, string, bool)"/>
        /// </summary>
        public async Task<ObservableCollection<User>> GetUserByWordAsync(String word, string user, bool syncItems = false)
        {
            try
            {
#if OFFLINE_SYNC_ENABLED
                if (syncItems)
                {
                    await this.SyncAsync();
                }
#endif
                IEnumerable<User> items = await userTable
                    .Where(u => u.Username.Contains(word) || u.Bio.Contains(word) && u.Id != user)
                    .ToEnumerableAsync();

                return new ObservableCollection<User>(items);
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
        /// Implementación de <see cref="IUserManager.GetUserByNameAndPassword(string, string, bool)"/>
        /// </summary>
        public async Task<User> GetUserByNameAndPassword(string login, string password, bool syncItems = false)
        {
            try
            {
#if OFFLINE_SYNC_ENABLED
                if (syncItems)
                {
                    await this.SyncAsync();
                }
#endif
                IEnumerable<User> items = await userTable
                    .Where(u => ((u.Username == login || u.Email == login) && u.Password == password))
                    .ToEnumerableAsync();

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
        /// Implementación de <see cref="IUserManager.DeleteUserAsync(User)"/>
        /// </summary>
        public async Task DeleteUserAsync(User item)
        {
            await MessageManager.DefaultManager.DeleteAllFromUser(item);
            await ChatManager.DefaultManager.DeleteAllFromUser(item);
            await GuildManager.DefaultManager.DeleteAllGuildsFromUser(item);
            await GuildMemberManager.DefaultManager.DeleteAllMembershipsFromUser(item);
            await PublicationManager.DefaultManager.DeleteAllPublicationsFromUser(item);
            await FollowManager.DefaultManager.DeleteAllFollowsFromUser(item);
            await BlockManager.DefaultManager.DeleteAllBlocksFromUser(item);
            await userTable.DeleteAsync(item);
            if (CrossConnectivity.Current.IsConnected)
                await this.SyncAsync();
        }

        /// <summary>
        /// Implementación de <see cref="IUserManager.SaveUserAsync(User)"/>
        /// </summary>
        public async Task SaveUserAsync(User item)
        {
            item.UpdatedInClient = DateTime.Now;
            if (item.Id == null)
                await userTable.InsertAsync(item);
            else
                await userTable.UpdateAsync(item);
            if (CrossConnectivity.Current.IsConnected)
                await this.SyncAsync();
        }

#if OFFLINE_SYNC_ENABLED
        /// <summary>
        /// Sincroniza la tabla local con el servidor
        /// </summary>
        public async Task SyncAsync()
        {
            await SyncClientProvider.DefaultInstance.SyncAsync(SyncUsers());
        }

        /// <summary>
        /// Acción de sincronización de la tabla
        /// </summary>
        private async Task SyncUsers()
        {
            await this.client.SyncContext.PushAsync();

            await this.userTable.PullAsync(
                //The first parameter is a query name that is used internally by the client SDK to implement incremental sync.
                //Use a different query name for each unique query in your program
                "allUsers",
                this.userTable.CreateQuery());
        }
#endif
    }
}
