namespace ULFG.Forms.PlatformInterfaces
{
    /// <summary>
    /// Interfaz empleada para la inyeccción de dependencias que contiene la funcionalidad nativa relacionada con los recursos nativos
    /// </summary>
    public interface IResourceManager
    {
        /// <summary>
        /// Obtiene la imagen de perfil por defecto
        /// </summary>
        /// <returns>Array de Bytes con la imagen</returns>
        byte[] GetBasicProfileImageAsByteArray();

        /// <summary>
        /// Obtiene la imagen de gremio por defecto
        /// </summary>
        /// <returns>Array de bytes con la imagen</returns>
        byte[] GetBasicGuildImageAsByteArray();

        /// <summary>
        /// Obtiene la ruta del recurso especificado
        /// </summary>
        /// <param name="resourceName">Nombre del recurso</param>
        /// <returns>Una cadena con la ruta del recurso</returns>
        string GetResourcesPath(string resourceName);
    }
}
