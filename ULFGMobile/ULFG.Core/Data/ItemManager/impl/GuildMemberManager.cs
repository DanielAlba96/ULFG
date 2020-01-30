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
    /// Implemente <see cref="IGuildMemberManager"/>
    /// </summary>
    public partial class GuildMemberManager : IGuildMemberManager
    {
        static IGuildMemberManager defaultInstance = new GuildMemberManager();
        readonly MobileServiceClient client;

#if OFFLINE_SYNC_ENABLED
        readonly IMobileServiceSyncTable<GuildMember> guildMemberTable;
#else
        readonly IMobileServiceTable<GuildMember> guildMemberTable;
#endif
        private GuildMemberManager()
        {
#if OFFLINE_SYNC_ENABLED
            this.client = SyncClientProvider.DefaultInstance.Client;
            this.guildMemberTable = client.GetSyncTable<GuildMember>();
#else
            this.client = new MobileServiceClient(Constants.ApplicationURL);
            this.guildMemberTable = client.GetTable<GuildMember>();
#endif
        }

        /// <summary>
        /// Instancia unica de la clase
        /// </summary>
        public static IGuildMemberManager DefaultManager
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
        /// Implementación de <see cref="IGuildMemberManager.GetGuildMembersAsync(bool)"/>
        /// </summary>
        public async Task<ObservableCollection<GuildMember>> GetGuildMembersAsync(bool syncItems = false)
        {
            try
            {
#if OFFLINE_SYNC_ENABLED
                if (syncItems)
                {
                    await this.SyncAsync();
                }
#endif
                IEnumerable<GuildMember> items = await guildMemberTable
                    .ToEnumerableAsync();

                return new ObservableCollection<GuildMember>(items);
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
        /// Implementación de <see cref="IGuildMemberManager.GetGuildsByMemberIdAsync(string, bool)"/>
        /// </summary>
        public async Task<ObservableCollection<Guild>> GetGuildsByMemberIdAsync(string memberId, bool syncItems = false)
        {
            try
            {
#if OFFLINE_SYNC_ENABLED
                if (syncItems)
                {
                    await this.SyncAsync();
                }
#endif
                var guildIds = await guildMemberTable
                   .Where(cm => cm.MemberId == memberId)
                   .Select(cm => cm.GuildId)
                   .ToEnumerableAsync();

                IEnumerable<Guild> guilds = await GuildManager.DefaultManager
                     .GetGuildsAsync();

                var result = guilds
                     .Join(guildIds, ch => ch.Id, id => id, (ch, id) => ch);

                return new ObservableCollection<Guild>(result);
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
        /// Implementación de <see cref="IGuildMemberManager.GetMembersByGuildIdAsync(string, bool)"/>
        /// </summary>
        public async Task<ObservableCollection<User>> GetMembersByGuildIdAsync(string guildId, bool syncItems = false)
        {
            try
            {
#if OFFLINE_SYNC_ENABLED
                if (syncItems)
                {
                    await this.SyncAsync();
                }
#endif
                var guildIds = await guildMemberTable
                   .Where(gm => gm.GuildId == guildId)
                   .Select(gm => gm.MemberId)
                   .ToEnumerableAsync();

                IEnumerable<User> users = await UserManager.DefaultManager
                    .GetUsersAsync();

                var result = users
                     .Join(guildIds, us => us.Id, id => id, (us, id) => us);

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
        /// Implementación de <see cref="IGuildMemberManager.GetNoMembersByGuildIdAsync(string, bool)"/>
        /// </summary>
        public async Task<ObservableCollection<User>> GetNoMembersByGuildIdAsync(string guildId, bool syncItems = false)
        {
            try
            {
#if OFFLINE_SYNC_ENABLED
                if (syncItems)
                {
                    await this.SyncAsync();
                }
#endif
                var guildIds = await guildMemberTable
                   .Where(gm => gm.GuildId == guildId)
                   .Select(gm => gm.MemberId)
                   .ToEnumerableAsync();

                IEnumerable<User> users = await UserManager.DefaultManager
                    .GetUsersAsync();

                var result = users.Where(l => guildIds.All(l2 => l.Id != l2));

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
        /// Implementación de <see cref="IGuildMemberManager.GetNumberOfMembers(string, bool)"/>
        /// </summary>
        public async Task<int> GetNumberOfMembers(string guildId, bool syncItems = false)
        {
            try
            {
#if OFFLINE_SYNC_ENABLED
                if (syncItems)
                {
                    await this.SyncAsync();
                }
#endif
                var members = await guildMemberTable
                   .Where(x => x.GuildId == guildId)
                   .ToEnumerableAsync();

                var result = members.Count();

                return result;
            }
            catch (MobileServiceInvalidOperationException msioe)
            {
                Debug.WriteLine(@"Invalid sync operation: {0}", msioe.Message);
            }
            catch (Exception e)
            {
                Debug.WriteLine(@"Sync error: {0}", e.Message);
            }

            return 0;
        }

        /// <summary>
        /// Implementación de <see cref="IGuildMemberManager.GetGuildMember(string, string, bool)"/>
        /// </summary>
        public async Task<GuildMember> GetGuildMember(string guildId, string memberId, bool syncItems = false)
        {
            try
            {
#if OFFLINE_SYNC_ENABLED
                if (syncItems)
                {
                    await this.SyncAsync();
                }
#endif
                var members = await guildMemberTable
                   .Where(x => x.GuildId == guildId && x.MemberId == memberId)
                   .ToEnumerableAsync();
                if (memberId.Count() == 0)
                    return null;
                else
                    return members.Single();
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
        /// Implementación de <see cref="IGuildMemberManager.SaveGuildMemberAsync(GuildMember)"/>
        /// </summary>
        public async Task SaveGuildMemberAsync(GuildMember item)
        {
            item.UpdatedInClient = DateTime.Now;
            await guildMemberTable.InsertAsync(item);

            if (CrossConnectivity.Current.IsConnected)
                await this.SyncAsync();
        }

        /// <summary>
        /// Implementación de <see cref="IGuildMemberManager.DeleteGuildMemberAsync(GuildMember)"/>
        /// </summary>
        public async Task DeleteGuildMemberAsync(GuildMember item)
        {
            var query = await guildMemberTable
                .Where(x => x.GuildId == item.GuildId && x.MemberId == item.MemberId)
                .ToEnumerableAsync();
            item = query.Single();
            await guildMemberTable.DeleteAsync(item);
            if (CrossConnectivity.Current.IsConnected)
                await this.SyncAsync();
        }

        /// <summary>
        /// Implementación de <see cref="IGuildMemberManager.DeleteAllMemberOfGuildAsync(string)"/>
        /// </summary>
        public async Task DeleteAllMemberOfGuildAsync(string guildId)
        {
            var aBorrar = await guildMemberTable.Where(ch => ch.GuildId == guildId).ToListAsync();
            aBorrar.ForEach(async x => await guildMemberTable.DeleteAsync(x));
            if (CrossConnectivity.Current.IsConnected)
                await this.SyncAsync();
        }

        /// <summary>
        /// Implementación de <see cref="IGuildMemberManager.DeleteAllMembershipsFromUser(User)"/>
        /// </summary>
        public async Task DeleteAllMembershipsFromUser(User user)
        {
            var list = await guildMemberTable.Where(c => c.MemberId == user.Id).ToListAsync();
            list.ForEach(async c => await DeleteGuildMemberAsync(c));
            if (CrossConnectivity.Current.IsConnected)
                await this.SyncAsync();
        }

#if OFFLINE_SYNC_ENABLED
        /// <summary>
        /// Sincroniza la tabla local con el servidor
        /// </summary>
        public async Task SyncAsync()
        {
            await SyncClientProvider.DefaultInstance.SyncAsync(SyncGuildMembers());
        }

        /// <summary>
        /// Acción de sincronización de la tabla
        /// </summary>
        private async Task SyncGuildMembers()
        {
            await this.client.SyncContext.PushAsync();

            await this.guildMemberTable.PullAsync(
                //The first parameter is a query name that is used internally by the client SDK to implement incremental sync.
                //Use a different query name for each unique query in your program
                "allGuildMembers",
                this.guildMemberTable.CreateQuery());
        }
#endif
    }
}
