using Microsoft.WindowsAzure.MobileServices;
using Microsoft.WindowsAzure.MobileServices.SQLiteStore;
using Microsoft.WindowsAzure.MobileServices.Sync;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using ULFG.Core.Data.Item;

namespace ULFG.Core.Data.ItemManager.Sync
{
    /// <summary>
    /// Clase que encapsula la conexión del cliente con el servidor
    /// </summary>
    /// <remarks> Gestiona la sicronización sin conexión</remarks>
    public class SyncClientProvider
    {
        static SyncClientProvider defaultInstance = new SyncClientProvider();
        readonly MobileServiceClient client;
        const string offlineDbPath = @"localstore.db";
        int numErrors;

        /// <summary>
        /// Constructor
        /// </summary>
        private SyncClientProvider()
        {
            //Crea el cliente y todas las tablas en local y las vincula con las del servidor
            numErrors = 0;
            client = new MobileServiceClient(Constants.ApplicationURL);
            var store = new MobileServiceSQLiteStore(offlineDbPath);

            store.DefineTable<User>();
            store.DefineTable<Publication>();
            store.DefineTable<Chat>();
            store.DefineTable<Message>();
            store.DefineTable<Follow>();
            store.DefineTable<Block>();
            store.DefineTable<Guild>();
            store.DefineTable<GuildMember>();
            Task.Run(() => client.SyncContext.InitializeAsync(store)).Wait();
        }
        /// <summary>
        /// Instancia unica de la clase
        /// </summary>
        public static SyncClientProvider DefaultInstance
        {
            get { return defaultInstance; }
        }
        /// <summary>
        /// Cliente de base de datos
        /// </summary>
        public MobileServiceClient Client
        {
            get { return client; }
        }

        /// <summary>
        /// Sincroniza una tabla local con el servidor
        /// </summary>
        /// <remarks>Se gestionan los errores y se intenta repetir la operación</remarks>
        public async Task SyncAsync(Task syncAction)
        {
            ReadOnlyCollection<MobileServiceTableOperationError> syncErrors = null;
            try
            {
                await syncAction;
            }
            catch (MobileServicePushFailedException exc)
            {
                if (exc.PushResult != null)
                    syncErrors = exc.PushResult.Errors;
            }

            // Conflict Handle
            if (syncErrors != null)
            {
                numErrors = syncErrors.Count;
                foreach (var error in syncErrors)
                {
                    if (error.OperationKind == MobileServiceTableOperationKind.Update && error.Result != null)
                    {
                        //Handle update error
                        await HandleUpdate(error);
                    }
                    else
                    {
                        // Discard local change.
                        await error.CancelAndDiscardItemAsync();
                        Debug.WriteLine(@"Error executing sync operation. Item: {0} ({1}). Operation discarded.", error.TableName, error.Item["id"]);
                        Debugger.Break();
                    }
                }
                if (numErrors == 0)
                    await this.client.SyncContext.PushAsync();
            }
        }

        /// <summary>
        /// Gestiona los errores de actualización
        /// </summary>
        /// <remarks>Si hay problemas durante la actualización se comprueba si la version del cliente es más actual que la que tiene el servidor.
        /// En caso de cumplirse, se fuerza la version del cliente en el servidor. Si no se cumple, la versión del cliente se descarta</remarks>
        /// <param name="error">La operación que causó el error</param>
        private async Task HandleUpdate(MobileServiceTableOperationError error)
        {
            var a = error.Item.Value<DateTime>("client_updated");
            var b = error.Result.ToObject<Block>().UpdatedAt;
            if (a > b)
            {
                error.Item["version"] = error.Result["version"];
                await error.UpdateOperationAsync(error.Item);
                numErrors--;
            }
            else
            {
                await error.CancelAndUpdateItemAsync(error.Result);
                Debug.WriteLine(@"Error executing sync operation. Item: {0} ({1}). Operation canceled.", error.TableName, error.Item["id"]);
                Debugger.Break();
            }
        }
    }
}
