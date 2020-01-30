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
    /// Implemente <see cref="IPublicationManager"/>
    /// </summary>
    public partial class PublicationManager : IPublicationManager
    {
        static IPublicationManager defaultInstance = new PublicationManager();
        readonly MobileServiceClient client;

#if OFFLINE_SYNC_ENABLED
        readonly IMobileServiceSyncTable<Publication> publicationTable;
#else
        readonly IMobileServiceTable<Publication> publicationTable;
#endif
        private PublicationManager()
        {
#if OFFLINE_SYNC_ENABLED
            this.client = SyncClientProvider.DefaultInstance.Client;

            this.publicationTable = client.GetSyncTable<Publication>();
#else
            this.client = new MobileServiceClient(Constants.ApplicationURL);
            this.publicationTable = client.GetTable<Publication>();
#endif
        }

        /// <summary>
        /// Instancia unica de la clase
        /// </summary>
        public static IPublicationManager DefaultManager
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
        /// Implementación de <see cref="IPublicationManager.GetPublicationsAsync(bool)"/>
        /// </summary>
        public async Task<ObservableCollection<Publication>> GetPublicationsAsync(bool syncItems = false)
        {
            try
            {
#if OFFLINE_SYNC_ENABLED
                if (syncItems)
                {
                    await this.SyncAsync();
                }
#endif
                IEnumerable<Publication> items = await publicationTable
                    .ToEnumerableAsync();

                return new ObservableCollection<Publication>(items);
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
        /// Implementación de <see cref="IPublicationManager.GetPublicationsByUserAsync(string, bool)"/>
        /// </summary>
        public async Task<ObservableCollection<Publication>> GetPublicationsByUserAsync(string user, bool syncItems = false)
        {
            try
            {
#if OFFLINE_SYNC_ENABLED
                if (syncItems)
                {
                    await this.SyncAsync();
                }
#endif
                IEnumerable<User> following = await FollowManager.DefaultManager.GetUsersFollowedBy(user, syncItems);

                IEnumerable<string> ids = following.Select(u => u.Id);
                var ids2 = ids.ToList();
                ids2.Add(user);

                IEnumerable<Publication> items = await publicationTable
                    .OrderByDescending(m => m.CreatedAt)
                    .ToEnumerableAsync();
                items = items.Where(x => (DateTime.Now - x.CreatedAt).Days < 7);

                var result = items.Join(ids2, x => x.AutorId, y => y, (p, e) => p);

                return new ObservableCollection<Publication>(result);
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
        /// Implementación de <see cref="IPublicationManager.GetPublicationByIdAsync(string, bool)"/>
        /// </summary>
        public async Task<Publication> GetPublicationByIdAsync(string id, bool syncItems = false)
        {
            try
            {
#if OFFLINE_SYNC_ENABLED
                if (syncItems)
                {
                    await this.SyncAsync();
                }
#endif
                var item = await publicationTable
                    .Where(x => x.Id == id)
                    .ToEnumerableAsync();

                return item.FirstOrDefault();
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
        /// Implementación de <see cref="IPublicationManager.SavePublicationAsync(Publication)"/>
        /// </summary>
        public async Task SavePublicationAsync(Publication item)
        {
            item.UpdatedInClient = DateTime.Now;
            if (item.Id == null)
                await publicationTable.InsertAsync(item);
            else
                await publicationTable.UpdateAsync(item);
            if (CrossConnectivity.Current.IsConnected)
                await this.SyncAsync();
        }

        /// <summary>
        /// Implementación de <see cref="IPublicationManager.DeletePubAsync(Publication)"/>
        /// </summary>
        public async Task DeletePubAsync(Publication item)
        {
            await publicationTable.DeleteAsync(item);
            if (CrossConnectivity.Current.IsConnected)
                await this.SyncAsync();
        }

        /// <summary>
        /// Implementación de <see cref="IPublicationManager.DeleteAllPublicationsFromUser(User)"/>
        /// </summary>
        public async Task DeleteAllPublicationsFromUser(User user)
        {
            var list = await publicationTable.Where(p => p.AutorId == user.Id).ToListAsync();
            list.ForEach(async c => await DeletePubAsync(c));
            if (CrossConnectivity.Current.IsConnected)
                await this.SyncAsync();
        }

#if OFFLINE_SYNC_ENABLED
        /// <summary>
        /// Sincroniza la tabla local con el servidor
        /// </summary>
        public async Task SyncAsync()
        {
            await SyncClientProvider.DefaultInstance.SyncAsync(SyncPublications());
        }

        /// <summary>
        /// Acción de sincronización de la tabla
        /// </summary>
        private async Task SyncPublications()
        {
            await this.client.SyncContext.PushAsync();

            await this.publicationTable.PullAsync(
                //The first parameter is a query name that is used internally by the client SDK to implement incremental sync.
                //Use a different query name for each unique query in your program
                "allPublications",
                this.publicationTable.CreateQuery());
        }
#endif
    }
}