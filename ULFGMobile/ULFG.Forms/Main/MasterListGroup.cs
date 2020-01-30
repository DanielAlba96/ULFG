using System.Collections.Generic;

namespace ULFG.Forms
{
    /// <summary>
    /// <see cref="List{MasterPageItem}"/> que representa la estructura de cada grupo de elementos del menú de navegación
    /// </summary>
    public class MasterListGroup : List<MasterPageItem>
    {
        /// <summary>
        /// Nombre identificativo del grupo
        /// </summary>
        public string Key { get; set; } 
        public MasterListGroup(string key)
        {
            Key = key;
        }
    }
}
