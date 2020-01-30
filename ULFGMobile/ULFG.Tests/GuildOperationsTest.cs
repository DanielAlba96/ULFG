using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ULFG.Core.Data.ItemManager;
using NSubstitute;
using ULFG.Core.Logic;
using ULFG.Core.Data.Item;
using System.Linq;

namespace ULFG.Tests
{
    /// <summary>
    /// Clase que contiene los test unitarios de <see cref="GuildOperations"/>
    /// </summary>
    [TestClass]
    public class GuildOperationsTest
    {
        /// <summary>
        /// Test de crear un gremio
        /// </summary>
        [TestMethod]
        public async Task TestCrearGremio()
        {
            IGuildManager guildManager = Substitute.For<IGuildManager>();
            IGuildMemberManager guildMemberManager = Substitute.For<IGuildMemberManager>();
            IMessageManager messageManager = Substitute.For<IMessageManager>();

            GuildOperations guildOperations = new GuildOperations(guildManager, guildMemberManager, messageManager);

            guildManager.SaveGuildAsync(Arg.Any<Guild>()).Returns(new Guild() { Id = "123" });

            await guildOperations.CreateGuild("test", "chill", "user1", true, new byte[] { 1, 2, 3 });
            await guildManager.Received().SaveGuildAsync(Arg.Is<Guild>(guild =>
            guild.Name.Equals("test") && guild.Description.Equals("chill") && guild.IsPublic && guild.Leader.Equals("user1")
                && guild.Image.SequenceEqual(new byte[] { 1, 2, 3 })));
            await guildMemberManager.Received().SaveGuildMemberAsync(Arg.Is<GuildMember>(g => g.GuildId.Equals("123") && g.MemberId.Equals("user1")));

        }

        /// <summary>
        /// Test de Abandonar y expulsar un miembro existente de un gremio
        /// </summary>
        [TestMethod]
        public async Task TestAbandonarExpulsarDeGremioExiste()
        {
            IGuildManager guildManager = Substitute.For<IGuildManager>();
            IGuildMemberManager guildMemberManager = Substitute.For<IGuildMemberManager>();
            IMessageManager messageManager = Substitute.For<IMessageManager>();

            GuildOperations guildOperations = new GuildOperations(guildManager, guildMemberManager, messageManager);

            guildMemberManager.GetGuildMember("guild1", "user1", Arg.Any<bool>()).Returns(new GuildMember() { Id = "123", GuildId = "guild1", MemberId = "user1" });

            await guildOperations.LeaveGuild("user1", "guild1");
            await guildMemberManager.Received().DeleteGuildMemberAsync(Arg.Is<GuildMember>(g => g.Id.Equals("123") && g.GuildId.Equals("guild1") && g.MemberId.Equals("user1")));
        }

        /// <summary>
        /// Test de abandonar y expulsar un miembro inexistente de un gremio 
        /// </summary>
        [TestMethod]
        public async Task TestAbandonarExpulsarDeGremioNoExiste()
        {
            IGuildManager guildManager = Substitute.For<IGuildManager>();
            IGuildMemberManager guildMemberManager = Substitute.For<IGuildMemberManager>();
            IMessageManager messageManager = Substitute.For<IMessageManager>();

            GuildOperations guildOperations = new GuildOperations(guildManager, guildMemberManager, messageManager);

            await guildOperations.LeaveGuild("user1", "guild1");
            await guildMemberManager.DidNotReceive().SaveGuildMemberAsync(Arg.Any<GuildMember>());
        }
    }
}
