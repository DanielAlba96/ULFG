using Plugin.Connectivity;
using System.Threading.Tasks;
using ULFG.Core.Data.Item;
using ULFG.Core.Data.ItemManager;
using ULFG.Core.Data.ItemManager.Impl;

namespace ULFG.Core.Logic
{
    /// <summary>
    /// Clase que contiene toda la lógica de los seguimientos y los bloqueos
    /// </summary>
    public class SocialOperations
    {
        readonly IFollowManager followManager;
        readonly IBlockManager blockManager;

        /// <summary>
        /// Constructor con parmametros
        /// </summary>
        public SocialOperations(IFollowManager followManager, IBlockManager blockManager)
        {
            this.followManager = followManager;
            this.blockManager = blockManager;
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public SocialOperations()
        {
            this.followManager = FollowManager.DefaultManager;
            this.blockManager = BlockManager.DefaultManager;
        }

        /// <summary>
        /// Bloquea un usuario
        /// </summary>
        /// <remarks>Borra todos los seguimientos activos de ambas partes antes de crear el bloqueo</remarks>
        /// <param name="blockingUser">El identificador del usuario que bloquea</param>
        /// <param name="blockedUser">El identificador del usuario a bloquear</param>
        public async Task BlockUser(string blockingUser, string blockedUser)
        {
            Block block = new Block() { BlockingUser = blockingUser, BlockedUser = blockedUser };
            var f = await followManager.GetFollowsByBothSidesAsync(blockingUser, blockedUser, CrossConnectivity.Current.IsConnected);
            var f2 = await followManager.GetFollowsByBothSidesAsync(blockedUser, blockingUser, CrossConnectivity.Current.IsConnected);
            if (f != null)
                await followManager.DeleteFollowAsync(f);
            if (f2 != null)
                await followManager.DeleteFollowAsync(f2);
            await blockManager.SaveBlockAsync(block);
        }

        /// <summary>
        /// Sigue a un usuario
        /// </summary>
        /// <remarks>Solo se podrá seguir si no existe ningun bloqueo entre los usuarios implicados</remarks>
        /// <param name="testingUser">El identificador del usuario que sigue</param>
        /// <param name="testedUser">El identificador del usuario a seguir</param>
        /// <returns></returns>
        public async Task<bool> FollowUser(string testingUser, string testedUser)
        {
            var ownBlock = await blockManager.GetBlocksByBothSidesAsync(testingUser, testedUser, CrossConnectivity.Current.IsConnected);
            var extBlock = await blockManager.GetBlocksByBothSidesAsync(testedUser, testingUser, CrossConnectivity.Current.IsConnected);

            if (ownBlock != null || extBlock != null)
                return false;
            else
            {
                await followManager.SaveFollowAsync(new Follow() { FollowingUser = testingUser, FollowedUser = testedUser });
                return true;
            }
        }

        /// <summary>
        /// Desbloquea a un usuario
        /// </summary>
        /// <param name="actualUser">El identificador del usuario que desbloquear</param>
        /// <param name="user">El identificador del usuario a desbloquear</param>
        public async Task UnblockUser(string actualUser, string user)
        {
            Block b = await blockManager.GetBlocksByBothSidesAsync(actualUser, user, CrossConnectivity.Current.IsConnected);
            if (b == null)
                return;
            await blockManager.DeleteBlockAsync(b);
        }

        /// <summary>
        /// Deja de seguir a un usuario
        /// </summary>
        /// <param name="loggedUser">El identificador del usuario que quiere dejar de seguir</param>
        /// <param name="user">El identificador del usuario a dejar se seguir</param>
        /// <returns></returns>
        public async Task UnfollowUser(string loggedUser, string user)
        {
            Follow f = await followManager.GetFollowsByBothSidesAsync(loggedUser, user, CrossConnectivity.Current.IsConnected);
            if (f == null)
                return;
            await followManager.DeleteFollowAsync(f);
        }
    }
}
