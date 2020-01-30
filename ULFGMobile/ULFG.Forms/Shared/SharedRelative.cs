using FFImageLoading.Forms;
using Xamarin.Forms;

namespace ULFG.Forms.Shared
{
    /// <summary>
    /// <see cref="RelativeLayout"/> personalizado para compartir código entre varias vistas que se basan en los datos del usuario.
    /// Incluye por defecto la imagen de perfil, el nombre y el apodo del usuario
    /// </summary>
    public class SharedRelative: RelativeLayout
    {
        /// <summary>
        /// Constructor que añade los elementos que recibe como parametro al layout
        /// </summary>
        /// <param name="userIcon">Imagen  de perfil del usuario</param>
        /// <param name="lblNick">Label con el apodo del usuario</param>
        /// <param name="lblSeparator">Label con un separador</param>
        /// <param name="lblName">Label con el username</param>
        public SharedRelative(CachedImage userIcon, Label lblNick, Label lblSeparator, Label lblName)
        {
            Children.Add(userIcon,
               Constraint.Constant(10),
               Constraint.Constant(10),
               Constraint.Constant(60),
               Constraint.Constant(60));

            Children.Add(lblNick,
                Constraint.RelativeToView(userIcon, (parent, sibling) =>
                {
                    return sibling.X + sibling.Width + 10;
                }),
                Constraint.RelativeToView(userIcon, (parent, sibling) =>
                {
                    return sibling.Y - 10;
                }));

            Children.Add(lblSeparator,
                Constraint.RelativeToView(lblNick, (parent, sibling) =>
                {
                    return sibling.X + sibling.Width + 5;
                }),
                Constraint.RelativeToView(lblNick, (parent, sibling) =>
                {
                    return sibling.Y;
                }));

            Children.Add(lblName,
               Constraint.RelativeToView(lblSeparator, (parent, sibling) =>
               {
                   return sibling.X + sibling.Width;
               }),
               Constraint.RelativeToView(lblSeparator, (parent, sibling) =>
               {
                   return sibling.Y;
               }));
        }
    }
}
