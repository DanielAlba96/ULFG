using Plugin.Connectivity;
using System;
using System.Security.Cryptography;
using System.Threading.Tasks;
using ULFG.Core.Data.Item;
using ULFG.Core.Data.ItemManager;
using ULFG.Core.Data.ItemManager.Impl;

namespace ULFG.Core.Logic
{
    /// <summary>
    /// Clase que contiene toda la lógica de los usuarios
    /// </summary>
    public class UserOperations
    {
        readonly IUserManager userManager;

        /// <summary>
        /// Constructor con parmametros
        /// </summary>
        public UserOperations(IUserManager userManager)
        {
            this.userManager = userManager;
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public UserOperations()
        {
            this.userManager = UserManager.DefaultManager;
        }

        /// <summary>
        /// Registra un nuevo usuario en el sistema
        /// </summary>
        /// <remarks>La contraseña se guarda encriptada mediante SHA256 y salt dinámica</remarks>
        /// <param name="username">El nombre de usuario</param>
        /// <param name="nickname">El apodo</param>
        /// <param name="email">El email</param>
        /// <param name="password">La contraseña</param>
        /// <param name="img">La imagen por defecto de un nuevo usuario</param>
        /// <returns>El nuevo usuario creado</returns>
        public async Task<User> RegisterUser(string username, string nickname, string email, string password, byte[] img)
        {
            User u = null;
            var test = await userManager.GetUserByNameAsync(username, CrossConnectivity.Current.IsConnected);
            if (test == null)
            {
                u = new User { Username = username, Nickname = nickname, Email = email, Image = img };
                var salt = CreateSalt();
                var fullPass = CryptoHelper.GenerateSHA256String(password + salt);
                u.Password = fullPass;
                u.Salt = salt;
                await userManager.SaveUserAsync(u);
            }
            return u;
        }

        private string CreateSalt()
        {
            var salt = new byte[32];
            using (var random = new RNGCryptoServiceProvider())
            {
                random.GetNonZeroBytes(salt);
            }
            return Convert.ToBase64String(salt);
        }

        /// <summary>
        /// Entra en sesión introduciendo las credenciales
        /// </summary>
        /// <param name="login">El nombre de usuario</param>
        /// <param name="password">La contraseña</param>
        /// <param name="userToken">El identificador del dispositivo en el servicio de notificaciones </param>
        /// <param name="userPlatform">El nombre del servicio de notificaciones</param>
        /// <returns>El usuario que entra en sesión</returns>
        public async Task<User> LoginUser(string login, string password, string userToken, string userPlatform)
        {
            var user = await userManager.GetUserByNameAsync(login, CrossConnectivity.Current.IsConnected);
            if (user == null)
                return user;
            var salt = user.Salt;
            var fullPass = CryptoHelper.GenerateSHA256String(password + salt);
            if (user.Password.Equals(fullPass))
            {
                if (userToken != null && userPlatform != null && !userToken.Equals(string.Empty) && !userPlatform.Equals(string.Empty))
                {
                    RegisterClient reg = new RegisterClient();
                    await reg.RegisterAsync(userPlatform, userToken, user.Id);
                }
            }
            else
                user = null;
            return user;
        }

        /// <summary>
        /// Cierra la sesión en la aplicación
        /// </summary>
        /// <remarks>Invalida la sesión del usuario en el servicio de notificaciones</remarks>
        public async Task LogoutUser()
        {
            RegisterClient reg = new RegisterClient();
            await reg.UnregisterUser();
        }

        /// <summary>
        /// Cambia la contraseña de un usuario
        /// </summary>
        /// <param name="user">El identificador del usauario que cambia su contraseña</param>
        /// <param name="oldPassword">La contraseña actual</param>
        /// <param name="newPassword">La nueva contraseña</param>
        /// <returns>True si se pudo cambiar, False en caso contrario</returns>
        public async Task<bool> ChangePassword(string user, string oldPassword, string newPassword)
        {
            var actual = await userManager.GetUserByIdAsync(user, CrossConnectivity.Current.IsConnected);
            if (actual == null)
                return false;
            var salt = actual.Salt;
            var old = CryptoHelper.GenerateSHA256String(oldPassword + salt);
            var good = actual.Password.Equals(old) ? true : false;
            if (!good)
                return false;
            var newFullPass = CryptoHelper.GenerateSHA256String(newPassword + salt);
            actual.Password = newFullPass;
            await userManager.SaveUserAsync(actual);
            return true;
        }
    }
}
