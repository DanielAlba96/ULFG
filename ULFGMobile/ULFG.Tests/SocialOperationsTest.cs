using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using ULFG.Core.Data.ItemManager;
using ULFG.Core.Logic;
using ULFG.Core.Data.Item;

namespace ULFG.Tests
{
    /// <summary>
    /// Clase que contiene los test unitarios de <see cref="SocialOperations"/>
    /// </summary>
    [TestClass]
    public class SocialOperationsTest
    {
        /// <summary>
        /// Test de bloquear un usuario
        /// </summary>
        [TestMethod]
        public async Task TestBloquear()
        {
            IBlockManager blockManager = Substitute.For<IBlockManager>();
            IFollowManager followManager = Substitute.For<IFollowManager>();

            SocialOperations socialOperations = new SocialOperations(followManager, blockManager);

            followManager.GetFollowsByBothSidesAsync("user1", "user2", Arg.Any<bool>()).Returns(new Follow()
            {
                FollowingUser = "user1",
                FollowedUser = "user2"
            });

            followManager.GetFollowsByBothSidesAsync("user2", "user1", Arg.Any<bool>()).Returns(new Follow()
            {
                FollowingUser = "user2",
                FollowedUser = "user1"
            });

            await socialOperations.BlockUser("user1", "user2");
            await followManager.Received().DeleteFollowAsync(Arg.Is<Follow>(follow =>
                follow.FollowingUser.Equals("user1") && follow.FollowedUser.Equals("user2")));
            await followManager.Received().DeleteFollowAsync(Arg.Is<Follow>(follow =>
                follow.FollowingUser.Equals("user2") && follow.FollowedUser.Equals("user1")));
            await blockManager.Received().SaveBlockAsync(Arg.Is<Block>(block =>
                block.BlockingUser.Equals("user1") && block.BlockedUser.Equals("user2")));
        }

        /// <summary>
        /// Test de seguir un usuario al que se está bloqueando
        /// </summary>
        [TestMethod]
        public async Task TestSeguirConBloqueoPropio()
        {
            IBlockManager blockManager = Substitute.For<IBlockManager>();
            IFollowManager followManager = Substitute.For<IFollowManager>();

            SocialOperations socialOperations = new SocialOperations(followManager, blockManager);

            blockManager.GetBlocksByBothSidesAsync("user1", "user2", Arg.Any<bool>()).Returns(new Block() { BlockingUser = "user1", BlockedUser = "user2" });

            await socialOperations.FollowUser("user1", "user2");
            await followManager.DidNotReceive().SaveFollowAsync(Arg.Any<Follow>());
        }

        /// <summary>
        /// Test de seguir a un usuario que bloquea al que intenta seguir
        /// </summary>
        [TestMethod]
        public async Task TestSeguirConBloqueoExterno()
        {
            IBlockManager blockManager = Substitute.For<IBlockManager>();
            IFollowManager followManager = Substitute.For<IFollowManager>();

            SocialOperations socialOperations = new SocialOperations(followManager, blockManager);

            blockManager.GetBlocksByBothSidesAsync("user2", "user1", Arg.Any<bool>()).Returns(new Block() { BlockingUser = "user2", BlockedUser = "user1" });

            await socialOperations.FollowUser("user1", "user2");
            await followManager.DidNotReceive().SaveFollowAsync(Arg.Any<Follow>());
        }

        /// <summary>
        /// Test de seguir a un usuario sin ningun bloqueo
        /// </summary>
        [TestMethod]
        public async Task TestSeguirSinBloqueo()
        {
            IBlockManager blockManager = Substitute.For<IBlockManager>();
            IFollowManager followManager = Substitute.For<IFollowManager>();

            SocialOperations socialOperations = new SocialOperations(followManager, blockManager);

            await socialOperations.FollowUser("user1", "user2");
            await followManager.Received().SaveFollowAsync(Arg.Is<Follow>(follow => 
            follow.FollowingUser.Equals("user1") && follow.FollowedUser.Equals("user2")));
        }

        /// <summary>
        /// Test de dejar de seguir a un usuario al que no se sigue
        /// </summary>
        [TestMethod]
        public async Task TestDejarDeSeguirNoExiste()
        {
            IBlockManager blockManager = Substitute.For<IBlockManager>();
            IFollowManager followManager = Substitute.For<IFollowManager>();

            SocialOperations socialOperations = new SocialOperations(followManager, blockManager);

            await socialOperations.UnfollowUser("user1","user2");
            await followManager.DidNotReceive().DeleteFollowAsync(Arg.Any<Follow>());

        }

        /// <summary>
        /// Test de dejar de seguir a un usuario al que se sigue
        /// </summary>
        [TestMethod]
        public async Task TestDejarDeSeguirExiste()
        {
            IBlockManager blockManager = Substitute.For<IBlockManager>();
            IFollowManager followManager = Substitute.For<IFollowManager>();

            SocialOperations socialOperations = new SocialOperations(followManager, blockManager);

            followManager.GetFollowsByBothSidesAsync("user1", "user2", Arg.Any<bool>()).Returns(new Follow() { FollowingUser = "user1", FollowedUser = "user2" });

            await socialOperations.UnfollowUser("user1", "user2");
            await followManager.Received().DeleteFollowAsync(Arg.Is<Follow>(follow => 
            follow.FollowingUser.Equals("user1") && follow.FollowedUser.Equals("user2")));
        }

        /// <summary>
        /// Test de desbloquear a un usuario bloqueado
        /// </summary>
        [TestMethod]
        public async Task TestDesbloquearNoExiste()
        {
            IBlockManager blockManager = Substitute.For<IBlockManager>();
            IFollowManager followManager = Substitute.For<IFollowManager>();

            SocialOperations socialOperations = new SocialOperations(followManager, blockManager);

            await socialOperations.UnblockUser("user1", "user2");
            await blockManager.DidNotReceive().DeleteBlockAsync(Arg.Any<Block>());
        }

        /// <summary>
        /// Test de desbloquear a un usuario desbloqueado
        /// </summary>
        [TestMethod]
        public async Task TestDesbloquearExiste()
        {
            IBlockManager blockManager = Substitute.For<IBlockManager>();
            IFollowManager followManager = Substitute.For<IFollowManager>();

            SocialOperations socialOperations = new SocialOperations(followManager, blockManager);

            blockManager.GetBlocksByBothSidesAsync("user1", "user2", Arg.Any<bool>()).Returns(new Block() { BlockingUser = "user1", BlockedUser = "user2" });

            await socialOperations.UnblockUser("user1", "user2");
            await blockManager.Received().DeleteBlockAsync(Arg.Is<Block>(block =>
            block.BlockingUser.Equals("user1") && block.BlockedUser.Equals("user2")));
        }
    }
}
