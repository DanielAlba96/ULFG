using Plugin.Connectivity;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using ULFG.Core.Data.Item;
using ULFG.Core.Data.ItemManager.Impl;
using ULFG.Forms.Shared;
using Xamarin.Forms;

namespace ULFG.Forms.Profiles.ViewModels
{
    /// <summary>
    /// <see cref="ViewModelBase"/> que representa el modelo de la página de la lista de usuarios
    /// </summary>
    public class UserListViewModel : ViewModelBase
    {
        /// <summary>
        /// Usuario actual
        /// </summary>
        readonly User loggedUser = (User)Application.Current.Properties["user"];

        ObservableCollection<User> _items;
        string _searchText;

        DelegateCommand _search;

        /// <summary>
        /// Inicializa el modelo
        /// </summary>
        /// <param name="nav">Referencia a la navegación actual de la aplicación</param>
        public UserListViewModel(INavigation nav) : base(nav)
        {
            _items = Task.Run(() => { return UserManager.DefaultManager.GetUsersExceptActualAsync(loggedUser.Id, CrossConnectivity.Current.IsConnected); }).Result;
            _searchText = string.Empty;
        }

        /// <summary>
        /// Lista de usuarios
        /// </summary>
        public ObservableCollection<User> Items { get => _items; set => _items = value; }

        /// <summary>
        /// Texto del filtro de búsqueda
        /// </summary>
        public string SearchText { get => _searchText; set => _searchText = value; }

        /// <summary>
        /// Comando para filtrar usuarios
        /// </summary>
        public ICommand Search
        {
            get { return _search = _search ?? new DelegateCommand(async () => await SearchCommand()); }
        }

        /// <summary>
        /// Acción del comando <seealso cref="Search"/>
        /// </summary>
        /// <returns></returns>
        async Task SearchCommand()
        {
            _items = await UserManager.DefaultManager.GetUserByWordAsync(_searchText, loggedUser.Id, CrossConnectivity.Current.IsConnected);
            RaisePropertyChanged("Items");
        }

        /// <summary>
        /// Gestiona el evento de cambiar el texto del campo de búsqueda
        /// </summary>
        public void SearchTextChanged(object sender, TextChangedEventArgs textChangedEventArgs)
        {
            if (_searchText.Equals(string.Empty))
            {
                _items = Task.Run(async () => { return await UserManager.DefaultManager.GetUsersExceptActualAsync(loggedUser.Id, CrossConnectivity.Current.IsConnected); }).Result;
                RaisePropertyChanged("Items");
            }
        }
    }
}
