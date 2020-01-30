using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ULFG.Core.Data.Item;
using ULFG.Core.Logic;
using ULFG.Core.Data.ItemManager;
using NSubstitute;

namespace ULFG.Tests
{
    /// <summary>
    /// Clase que contiene los test unitarios de <see cref="UserOperations"/>
    /// </summary>
    [TestClass]
    public class UserOperationsTest
    {

        /// <summary>
        /// Test de login correcto
        /// </summary>
        [TestMethod]
        public async Task TestLoginCorrecto()
        {
            IUserManager userManger = Substitute.For<IUserManager>();

            UserOperations userOperations = new UserOperations(userManger);

            userManger.GetUserByNameAsync("usuario", Arg.Any<bool>()).Returns(new User
            {
                Salt = "1234",
                Password = CryptoHelper.GenerateSHA256String("clave1234")
            });

            User user = await userOperations.LoginUser("usuario", "clave", "", "");
            Assert.IsNotNull(user);
        }

        /// <summary>
        /// Test de login incorrecto
        /// </summary>
        [TestMethod]
        public async Task TestLoginClaveIncorrecta()
        {
            IUserManager userManger = Substitute.For<IUserManager>();

            UserOperations userOperations = new UserOperations(userManger);

            userManger.GetUserByNameAsync("usuario", Arg.Any<bool>()).Returns(new User
            {
                Salt = "1234",
                Password = CryptoHelper.GenerateSHA256String("otra1234")
            });

            User user = await userOperations.LoginUser("usuario", "clave", "", "");
            Assert.IsNull(user);
        }

        /// <summary>
        /// Test de login con usuario inexistente
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task TestLoginNoExiste()
        {
            IUserManager userManger = Substitute.For<IUserManager>();

            UserOperations userOperations = new UserOperations(userManger);

            User user = await userOperations.LoginUser("usuario", "clave", "", "");
            Assert.IsNull(user);
        }

        /// <summary>
        /// Test de registro de usuario correcto
        /// </summary>
        [TestMethod]
        public async Task TestRegistroDeUsuarioCorrecto()
        {
            IUserManager userManger = Substitute.For<IUserManager>();

            UserOperations userOperations = new UserOperations(userManger);

            await userOperations.RegisterUser("username", "nickname", "email@prueba.es", "clave", new byte[] { 1, 2, 3, 4 });
            await userManger.Received().SaveUserAsync(Arg.Is<User>(user =>
                user.Email.Equals("email@prueba.es")
                && user.Image.SequenceEqual(new byte[] { 1, 2, 3, 4 })
                && user.Nickname.Equals("nickname")
                && user.Password.Equals(CryptoHelper.GenerateSHA256String("clave" + user.Salt))
                && user.Username.Equals("username")
                ));
        }

        /// <summary>
        /// Test de registro de un usuario existente
        /// </summary>
        [TestMethod]
        public async Task TestRegistroDeUsuarioExistente()
        {
            IUserManager userManger = Substitute.For<IUserManager>();

            UserOperations userOperations = new UserOperations(userManger);

            userManger.GetUserByNameAsync("username", Arg.Any<bool>()).Returns(new User());

            await userOperations.RegisterUser("username", "nickname", "email@prueba.es", "clave", new byte[] { 1, 2, 3, 4 });
            await userManger.DidNotReceive().SaveUserAsync(Arg.Any<User>());
        }

        /// <summary>
        /// Test de cambiar contraseña introduciendo correctamente la contraseña antigua
        /// </summary>
        [TestMethod]
        public async Task TestCambiarPasswordCorrecta()
        {
            IUserManager userManger = Substitute.For<IUserManager>();

            UserOperations userOperations = new UserOperations(userManger);

            userManger.GetUserByIdAsync("123", Arg.Any<bool>()).Returns(new User() { Id = "123", Salt = "salt", Password = CryptoHelper.GenerateSHA256String("clavesalt") });

            await userOperations.ChangePassword("123", "clave", "cla");
            await userManger.Received().SaveUserAsync(Arg.Is<User>(user =>
                user.Id.Equals("123")
                && user.Salt.Equals("salt")
                && user.Password.Equals(CryptoHelper.GenerateSHA256String("clasalt"))
            ));
        }

        /// <summary>
        /// Test de cambiar contraseña introduciendo incorrectamente la contraseña antigua
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task TestCambiarPasswordIncorrecta()
        {
            IUserManager userManger = Substitute.For<IUserManager>();

            UserOperations userOperations = new UserOperations(userManger);

            userManger.GetUserByIdAsync("123", Arg.Any<bool>()).Returns(new User() { Id = "123", Salt = "salt", Password = CryptoHelper.GenerateSHA256String("clavesalt") });

            var res = await userOperations.ChangePassword("123", "clavel", "cla");

            await userManger.DidNotReceive().SaveUserAsync(Arg.Any<User>());
            Assert.IsFalse(res);
        }

        /// <summary>
        /// Test de cambiar contraseña de un usuario que no existe
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task TestCambiarPasswordNoExisteUser()
        {
            IUserManager userManger = Substitute.For<IUserManager>();

            UserOperations userOperations = new UserOperations(userManger);

            var res = await userOperations.ChangePassword("123", "clave", "cla");

            await userManger.DidNotReceive().SaveUserAsync(Arg.Any<User>());
            Assert.IsFalse(res);
        }
    }
}
