using System.Security.Cryptography;
using System.Text;

namespace ULFG.Core.Logic
{
    /// <summary>
    /// Clase estática que proporciona funciones de criptografía para las contraseñas
    /// </summary>
    public static class CryptoHelper
    {
        /// <summary>
        /// Genera un string encriptado en SHA256
        /// </summary>
        /// <param name="inputString">cadena a encriptar</param>
        /// <returns>la cadena encriptada</returns>
        public static string GenerateSHA256String(string inputString)
        {
            SHA256 sha256 = SHA256.Create();
            byte[] bytes = Encoding.UTF8.GetBytes(inputString);
            byte[] hash = sha256.ComputeHash(bytes);
            return GetStringFromHash(hash);
        }

        private static string GetStringFromHash(byte[] hash)
        {
            StringBuilder result = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                result.Append(hash[i].ToString("X2"));
            }
            return result.ToString();
        }
    }
}
