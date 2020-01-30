using Xamarin.Forms;

namespace ULFG.Forms
{
    /// <summary>
    /// <see cref="ViewCell"/> de los elementos del menú de navegación
    /// </summary>
    public class ListGroupViewCell :ViewCell
    {
        readonly BoxView separator;
        public ListGroupViewCell()
        {
            separator = new BoxView { HeightRequest = 1, Color = Color.FromHex("424242")};
            View = separator;
        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();
            if (BindingContext is MasterListGroup item)
            {
                if (item.Key.Equals("first"))
                    separator.Opacity = 0;
                else
                    separator.Opacity = 1;
            }
        }
    }
}
