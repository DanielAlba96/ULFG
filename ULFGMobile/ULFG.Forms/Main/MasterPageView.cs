using System;
using System.Collections.Generic;
using Xamarin.Forms;
using ULFG.Core.Data.Item;
using ULFG.Forms.Profiles.Views;
using ULFG.Forms.Network.Views;
using ULFG.Forms.Guilds.Views;
using FFImageLoading.Forms;
using System.Threading.Tasks;
using ULFG.Forms.PlatformInterfaces;
using ULFG.Core.Logic;
using Acr.UserDialogs;
using ULFG.Forms.Login.Views;
using ULFG.Forms.Shared;

namespace ULFG.Forms
{
    /// <summary>
    /// <see cref="MasterDetailPage"/> que representa la vista del menú de navegación
    /// </summary>
    public class MasterPageView : MasterDetailPage
    {
        /// <summary>
        /// Label con los datos del usuario
        /// </summary>
        readonly Label lbl;
        /// <summary>
        /// Imagen de perfil del usuario
        /// </summary>
        readonly CachedImage img;
        Page detail; // Página actual

        /// <summary>
        ///  Grupos de elementos de navegación
        /// </summary>
        readonly List<MasterListGroup> groups = new List<MasterListGroup>()
        {
            new MasterListGroup("first")
            {
                new MasterPageItem() { Title = "Editar perfil", IconSource = DependencyService.Get<IResourceManager>().GetResourcesPath("profile.png"), TargetType=typeof(OwnProfileView)},
                new MasterPageItem() { Title = "Mi red", IconSource = DependencyService.Get<IResourceManager>().GetResourcesPath("network.png"), TargetType=typeof(MyNetwork)},
                new MasterPageItem() { Title = "Gremios", IconSource = DependencyService.Get<IResourceManager>().GetResourcesPath("guild2.png"), TargetType=typeof(GuildTabView)}
            },
            new MasterListGroup("second")
            {
                new MasterPageItem() { Title = "Acerca De", IconSource = DependencyService.Get<IResourceManager>().GetResourcesPath("help.png"),TargetType=typeof(About)},
                new MasterPageItem() { Title = "Desconectar", IconSource = DependencyService.Get<IResourceManager>().GetResourcesPath("logout.png"),TargetType=null}
            }
        };

        public MasterPageView()
        {
            Task[] tasks = null;
            //Debido a una limitación de UWP, solo se realiza una precarga en Android. 
            if (Device.RuntimePlatform == Device.Android)
                tasks = EagerPageLoading();

            BindingContext = new MasterPageViewModel(Navigation);

            Title = "ULFG";

            lbl = new Label() { FontSize = 18, TextColor = Color.FromHex("DB8D28") };
            lbl.SetBinding(Label.TextProperty, "User");

            img = new CachedImage() { Aspect = Aspect.AspectFit, HorizontalOptions = LayoutOptions.Start, BitmapOptimizations = true, HeightRequest = 120, WidthRequest = 120 };
            img.SetBinding(CachedImage.SourceProperty, "UserIcon");

            BoxView separator = new BoxView { HeightRequest = 1, Color = Color.FromHex("424242") };

            StackLayout stack = new StackLayout()
            {
                Children =
                {
                    img,
                    lbl
                }
            };

            ContentView header = new ContentView
            {
                Padding = new Thickness(15),
                Content = stack
            };

            DataTemplate listTemplate = new DataTemplate(() =>
            {
                var grid = new Grid { Padding = new Thickness(5, 10) };
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(30) });
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });

                var image = new Image();
                image.SetBinding(Image.SourceProperty, "IconSource");
                var label = new Label { VerticalOptions = LayoutOptions.FillAndExpand, TextColor = Color.FromHex("212121") };
                label.SetBinding(Label.TextProperty, "Title");

                grid.Children.Add(image);
                grid.Children.Add(label, 1, 0);

                return new ViewCell { View = grid };
            });

            ListView list = new ListView
            {
                ItemsSource = groups,
                ItemTemplate = listTemplate,
                SeparatorVisibility = SeparatorVisibility.None,
                HeightRequest = 380,
                IsGroupingEnabled = true,
                GroupHeaderTemplate = new DataTemplate(typeof(ListGroupViewCell)),
                HasUnevenRows = true
            };
            list.ItemSelected += DetailSelected;

            StackLayout global = new StackLayout
            {
                Children = { header, list }
            };
            this.Master = new ContentPage
            {
                Title = "ULFG",
                Content = global,
                Padding = 0
            };

            //Si se hizo precarga esperamos a que se cargen todas en este punto
            if (Device.RuntimePlatform == Device.Android)
            {
                Task.WaitAll(tasks);
                Detail = new CustomNavigationPage(detail);
            }
            else
                Detail = new CustomNavigationPage(new MainPage());
        }

        /// <summary>
        /// Gestiona los eventos que tienen lugar al seleccionar un elemento del menú de navegación
        /// </summary>
        /// <param name="sender"> Emisor del mensaje</param>
        /// <param name="e">Objeto seleccionado</param>
        async void DetailSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem is MasterPageItem item)
            {
                //Si se selecciona el elemento de desconexión se cierra la sesión y se vuelve al login
                if (item.Title.Equals("Desconectar"))
                {
                    var config = new ConfirmConfig() { Title = "Salir", Message = "¿Seguro que deseas cerrar sesión?", OkText = "Si", CancelText = "No" };
                    var answer = await UserDialogs.Instance.ConfirmAsync(config);
                    if (answer)
                    {
                        var pconfig = new ProgressDialogConfig() { Title = "Cerrando sesion" };
                        var progress = UserDialogs.Instance.Progress(pconfig);
                        UserOperations op = new UserOperations();
                        await op.LogoutUser();                       
                        Application.Current.MainPage = new NavigationPage(new LoginView());
                        progress.Dispose();
                    }
                }
                //Si és otra página, se va directamente a ella
                else
                {

                    Page page;
                    //Volvemos a hacer distinción entre UWP y Android
                    if (Device.RuntimePlatform == Device.Android)
                        page = item.Page;
                    else
                        page = (Page)Activator.CreateInstance(item.TargetType);

                    await Detail.Navigation.PushAsync(page, false);
                }
                ((ListView)sender).SelectedItem = null;
                IsPresented = false;
            }
        }

        /// <summary>
        /// Precarga todas las paginas del menú mediante multithreading
        /// </summary>
        /// <returns> Una lista con referencias a las tareas lanzadas</returns>
        Task[] EagerPageLoading()
        {
            var group1 = groups[0];
            var group2 = groups[1];
            int count = 0;

            Task[] tasks = new Task[group1.Count + group2.Count];

            foreach (var item in group1)
            {
                tasks[count] = new Task(() => item.Page = (Page)Activator.CreateInstance(item.TargetType));
                tasks[count].Start();
                count++;
            }

            foreach (var item in group2)
            {
                if (item.TargetType != null)
                {
                    tasks[count] = new Task(() => item.Page = (Page)Activator.CreateInstance(item.TargetType));
                    tasks[count].Start();
                    count++;
                }
            }

            tasks[count] = new Task(() => detail = new MainPage());
            tasks[count].Start();

            return tasks;
        }
    }
}
