using FFImageLoading.Forms;
using ULFG.Forms.PlatformInterfaces;
using Xamarin.Forms;

namespace ULFG.Forms
{
    /// <summary>
    /// <see cref="ContentPage"/> que contiene la vista del "Acerca De" de la aplicación
    /// </summary>
    public class About : ContentPage
    {
        public About()
        {
            Title = "Acerca De ULFG";

            //Titulo de la aplicación
            Label lblTitle = new Label()
            {
                Text = "ULFG: La red social para jugadores",
                HorizontalTextAlignment = TextAlignment.Center,
                FontAttributes = FontAttributes.Bold,
                FontSize = 18
            };

            //Logo de la aplicación
            CachedImage icon = new CachedImage()
            {
                Source = DependencyService.Get<IResourceManager>().GetResourcesPath("ulfg.png")
            };

            //Version y fecha de última actualización
            Label lblVersion = new Label()
            {
                Text = "Versión 1.1 (Junio 2018)",
                HorizontalTextAlignment = TextAlignment.Center,
                FontAttributes = FontAttributes.Bold,
                FontSize = 12
            };

            // Descripción 
            Label lblDesc = new Label()
            {
                Text = "Trabajo Fin de Grado de Ingeniería Informática del Software de la Escuela de Ingeniería Informática de Oviedo",
                LineBreakMode = LineBreakMode.WordWrap,
                HorizontalTextAlignment = TextAlignment.Center,
                FontAttributes = FontAttributes.Bold,
                TextColor = Color.Black,
            };

            // Autor de la aplicación
            Label lblAuthor = new Label()
            {
                Text = "Daniel Alba Muñiz",
                HorizontalTextAlignment = TextAlignment.Center,
                FontAttributes = FontAttributes.Bold,
                FontSize = 12
            };

            Grid grid = new Grid()
            {
                ColumnDefinitions =
                {
                    new ColumnDefinition{Width = GridLength.Star}
                },
                RowDefinitions =
                {
                    new RowDefinition{Height = 200},
                    new RowDefinition{Height = 30},
                    new RowDefinition{Height = 30},
                    new RowDefinition{Height = 30},
                    new RowDefinition{Height = GridLength.Auto}
                },
                RowSpacing = 10
            };
            grid.Children.Add(icon, 0, 0);
            grid.Children.Add(lblTitle, 0, 1);
            grid.Children.Add(lblVersion, 0, 2);
            grid.Children.Add(lblAuthor, 0, 3);
            grid.Children.Add(lblDesc, 0, 4);

            Content = new ScrollView() { Content = grid };
        }
    }
}