#define OFFLINE_SYNC_ENABLED

using Microsoft.WindowsAzure.MobileServices;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using ULFG.Core.Data.Item;
using Plugin.Connectivity;
using ULFG.Core.Data.ItemManager.Sync;

#if OFFLINE_SYNC_ENABLED
using Microsoft.WindowsAzure.MobileServices.Sync;
#endif

namespace ULFG.Core.Data.ItemManager.Impl
{
    /// <summary>
    /// Implemente <see cref="IGuildManager"/>
    /// </summary>
    public partial class GuildManager : IGuildManager
    {
        static IGuildManager defaultInstance = new GuildManager();
        readonly MobileServiceClient client;
        readonly IGuildMemberManager guildMemberManager = GuildMemberManager.DefaultManager;
        readonly IMessageManager messageManager = MessageManager.DefaultManager;

#if OFFLINE_SYNC_ENABLED
        readonly IMobileServiceSyncTable<Guild> guildTable;
#else
        readonly IMobileServiceTable<Guild> guildTable;
#endif
        private GuildManager()
        {
#if OFFLINE_SYNC_ENABLED
            this.client = SyncClientProvider.DefaultInstance.Client;
            this.guildTable = client.GetSyncTable<Guild>();
#else
            this.client = new MobileServiceClient(Constants.ApplicationURL);
            this.guildTable = client.GetTable<Guild>();
#endif
        }

        /// <summary>
        /// Instancia unica de la clase
        /// </summary>
        public static IGuildManager DefaultManager
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
        /// Implementación de <see cref="IGuildManager.GetGuildsAsync(bool)"/>
        /// </summary>
        public async Task<ObservableCollection<Guild>> GetGuildsAsync(bool syncItems = false)
        {
            try
            {
#if OFFLINE_SYNC_ENABLED
                if (syncItems)
                {
                    await this.SyncAsync();
                }
#endif
                IEnumerable<Guild> items = await guildTable
                    .ToEnumerableAsync();

                return new ObservableCollection<Guild>(items);
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
        /// Implementación de <see cref="IGuildManager.GetGuildByNameAsync(string, bool)"/>
        /// </summary>
        public async Task<Guild> GetGuildByNameAsync(string name, bool syncItems = false)
        {
            try
            {
#if OFFLINE_SYNC_ENABLED
                if (syncItems)
                {
                    await this.SyncAsync();
                }
#endif
                var items = await guildTable
                     .Where(x => x.Name == name)
                     .ToEnumerableAsync();

                return items.FirstOrDefault();
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
        /// Implementación de <see cref="IGuildManager.GetGuildsByMemberIdAsync(string, bool)"/>
        /// </summary>
        public async Task<ObservableCollection<Guild>> GetGuildsByMemberIdAsync(string id, bool syncItems = false)
        {
            try
            {
#if OFFLINE_SYNC_ENABLED
                if (syncItems)
                {
                    await this.SyncAsync();
                }

#endif
                var guilds = await guildMemberManager.GetGuildsByMemberIdAsync(id, syncItems);

                return new ObservableCollection<Guild>(guilds);
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
        /// Implementación de <see cref="IGuildManager.GetGuildsByMembersIdsAsync(List{User}, bool)"/>
        /// </summary>
        public async Task<ObservableCollection<Guild>> GetGuildsByMembersIdsAsync(List<User> members, bool syncItems = false)
        {
            try
            {
#if OFFLINE_SYNC_ENABLED
                if (syncItems)
                {
                    await this.SyncAsync();
                }
#endif
                IEnumerable<Guild> items = await guildTable
                    .ToEnumerableAsync();

                var users = await UserManager.DefaultManager.GetUsersAsync(syncItems);
                var items2 = items.ToList();



                return new ObservableCollection<Guild>(items2);
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
        /// Implementación de <see cref="IGuildManager.GetGuildByIdAsync(string, bool)"/>
        /// </summary>
        public async Task<Guild> GetGuildByIdAsync(string id, bool syncItems = false)
        {
            try
            {
#if OFFLINE_SYNC_ENABLED
                if (syncItems)
                {
                    await this.SyncAsync();
                }
#endif
                IEnumerable<Guild> items = await guildTable
                    .Where(c => c.Id == id)
                    .ToEnumerableAsync();

                return items.FirstOrDefault(x => x.Id == id);
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
        /// Implementación de <see cref="IGuildManager.GetGuildByWordAsync(string, bool)"/>
        /// </summary>
        public async Task<ObservableCollection<Guild>> GetGuildByWordAsync(string word, bool syncItems = false)
        {
            try
            {
#if OFFLINE_SYNC_ENABLED
                if (syncItems)
                {
                    await this.SyncAsync();
                }
#endif
                IEnumerable<Guild> items = await guildTable
                    .Where(c => (c.Name.Contains(word) || c.Description.Contains(word)))
                    .ToEnumerableAsync();

                return new ObservableCollection<Guild>(items);
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
        /// Implementación de <see cref="IGuildManager.IsMemberAsync(string, string, bool)"/>
        /// </summary>
        public async Task<Boolean> IsMemberAsync(string guild, string member, bool syncItems = false)
        {
            try
            {
#if OFFLINE_SYNC_ENABLED
                if (syncItems)
                {
                    await this.SyncAsync();
                }
#endif
                var items = await guildMemberManager.GetMembersByGuildIdAsync(guild, syncItems);

                foreach (var c in items)
                {
                    if (c.Id.Equals(member))
                        return true;
                }

                return false;
            }
            catch (MobileServiceInvalidOperationException msioe)
            {
                Debug.WriteLine(@"Invalid sync operation: {0}", msioe.Message);
            }
            catch (Exception e)
            {
                Debug.WriteLine(@"Sync error: {0}", e.Message);
            }
            return false;
        }

        /// <summary>
        /// Implementación de <see cref="IGuildManager.SaveGuildAsync(Guild))"/>
        /// </summary>
        public async Task<Guild> SaveGuildAsync(Guild item)
        {
            item.UpdatedInClient = DateTime.Now;
            if (item.Id == null)
                await guildTable.InsertAsync(item);
            else
                await guildTable.UpdateAsync(item);
            if (CrossConnectivity.Current.IsConnected)
                await this.SyncAsync();
            return item;
        }

        /// <summary>
        /// Implementación de <see cref="IGuildManager.DeleteGuildAsync(Guild)"/>
        /// </summary>
        public async Task DeleteGuildAsync(Guild guild)
        {
            await messageManager.DeleteAllFromGuild(guild.Id);
            await guildMemberManager.DeleteAllMemberOfGuildAsync(guild.Id);
            await guildTable.DeleteAsync(guild);
            if (CrossConnectivity.Current.IsConnected)
                await this.SyncAsync();
        }

        /// <summary>
        /// Implementación de <see cref="IGuildManager.DeleteAllGuildsFromUser(User)"/>
        /// </summary>
        public async Task DeleteAllGuildsFromUser(User user)
        {
            var list = await guildTable.Where(c => c.Leader == user.Id).ToListAsync();
            list.ForEach(async c => await DeleteGuildAsync(c));
            if (CrossConnectivity.Current.IsConnected)
                await this.SyncAsync();
        }

#if OFFLINE_SYNC_ENABLED
        /// <summary>
        /// Sincroniza la tabla local con el servidor
        /// </summary>
        public async Task SyncAsync()
        {
            await SyncClientProvider.DefaultInstance.SyncAsync(SyncGuilds());
        }

        /// <summary>
        /// Acción de sincronización de la tabla
        /// </summary>
        private async Task SyncGuilds()
        {
            await this.client.SyncContext.PushAsync();

            await this.guildTable.PullAsync(
                //The first parameter is a query name that is used internally by the client SDK to implement incremental sync.
                //Use a different query name for each unique query in your program
                "allGuilds",
                this.guildTable.CreateQuery());
        }
#endif
    }
}
