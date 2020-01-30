using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ULFG.Core.Data.ItemManager;
using NSubstitute;
using ULFG.Core.Logic;
using ULFG.Core.Data.Item;

namespace ULFG.Tests
{
    /// <summary>
    /// Clase que contiene los test unitarios de la clase <see cref="MessageOperations"/>
    /// </summary>
    [TestClass]
    public class MessageOperationsTest
    { 
        /// <summary>
        /// Test de enviar un mensaje a un usuario con el que no se tiene un chat
        /// </summary>
        [TestMethod]
        public async Task CrearChatNoExiste()
        {
            IChatManager chatManager = Substitute.For<IChatManager>();
            IUserManager userManager = Substitute.For<IUserManager>();
            IMessageManager messageManager = Substitute.For<IMessageManager>();
            IPublicationManager publicationManager = Substitute.For<IPublicationManager>();

            MessageOperations messageOperations = new MessageOperations(userManager, chatManager, publicationManager, messageManager);

            chatManager.SaveChatAsync(Arg.Any<Chat>()).Returns(new Chat() { Id = "67", Member1_id="123", Member2_id="124" });
                
            await messageOperations.SendMessage("123", "124", "hola");
            await chatManager.Received().SaveChatAsync(Arg.Is<Chat>(chat => chat.Member1_id.Equals("123") && chat.Member2_id.Equals("124")));
            await messageManager.Received().SaveMsgAsync(Arg.Is<Message>(m=> m.Chat_Id.Equals("67") && m.Creator_Id.Equals("123")&& m.Text.Equals("hola")));
        }

        /// <summary>
        /// Test de enviar un mensaje a un usuario con el que ya se tiene un chat
        /// </summary>
        [TestMethod]
        public async Task CrearChatExiste()
        {
            IChatManager chatManager = Substitute.For<IChatManager>();
            IUserManager userManager = Substitute.For<IUserManager>();
            IMessageManager messageManager = Substitute.For<IMessageManager>();
            IPublicationManager publicationManager = Substitute.For<IPublicationManager>();

            MessageOperations messageOperations = new MessageOperations(userManager, chatManager, publicationManager, messageManager);

            chatManager.GetChatsByMembersAsync("123", "124", Arg.Any<bool>()).Returns(new Chat() { Id = "56", Member1_id = "123", Member2_id = "124" });

            await messageOperations.SendMessage("123", "124", "hola");
            await chatManager.DidNotReceive().SaveChatAsync(Arg.Any<Chat>());
        }
    }
}
