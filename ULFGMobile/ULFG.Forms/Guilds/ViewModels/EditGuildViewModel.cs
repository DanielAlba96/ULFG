using Acr.UserDialogs;
using Plugin.Media;
using Plugin.Media.Abstractions;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Input;
using ULFG.Core.Data.Item;
using ULFG.Core.Data.ItemManager.Impl;
using ULFG.Forms.Shared;
using Xamarin.Forms;

namespace ULFG.Forms.Guilds.ViewModels
{
    /// <summary>
    /// <see cref="ViewModelBase"/> que representa el modelo de la página de edición de un gremio
    /// </summary>
    public class EditGuildViewModel : ViewModelBase
    {
        readonly IProgressDialog progress;
        /// <summary>
        /// Gremio actual
        /// </summary>
        readonly Guild guild;

        ImageSource _source;
        string _name;   
        string _desc;
        string _msg;
        string _btnName;
        string _btnDesc;
        string _btnMsg;
        bool _visibility;
        bool _nameEnabled;
        bool _descEnabled;
        bool _msgEnabled;
        Color _descBackground;
        Color _msgBackground;

        DelegateCommand _changeName;
        DelegateCommand _changeDesc;
        DelegateCommand _changeMsg;
        DelegateCommand _changeVisibility;
        DelegateCommand _changeImage;

        /// <summary>
        /// Inicializa el modelo
        /// </summary>
        /// <param name="nav">Referencia a la navegación actual de la aplicación</param>
        /// <param name="g">Gremio actual</param>
        public EditGuildViewModel(INavigation nav, Guild g) : base(nav)
        {
            _btnDesc = "Editar";
            _btnName = "Editar";
            _btnMsg = "Editar";
            _name = g.Name;
            _desc = g.Description;
            _msg = g.Message;
            _source = ImageSource.FromStream(() => new MemoryStream(g.Image));
            if (g.IsPublic)
                _visibility = true;
            this.guild = g;
            _msgBackground = Color.FromHex("#E0E0E0");
            _descBackground = Color.FromHex("#E0E0E0");
            progress = (UserDialogs.Instance.Progress(new ProgressDialogConfig() { Title = "Actualizando gremio", AutoShow = false }));
        }

        /// <summary>
        /// Imagen del gremio
        /// </summary>
        public ImageSource Source { get => _source; set => _source = value; }

        /// <summary>
        /// Nombre del gremio
        /// </summary>
        public string Name { get => _name; set => _name = value; }

        /// <summary>
        /// Descripción del gremio
        /// </summary>
        public string Desc { get => _desc; set => _desc = value; }

        /// <summary>
        /// Mensaje fijado del gremio
        /// </summary>
        public string Msg { get => _msg; set => _msg = value; }

        /// <summary>
        /// Texto del boton editar nombre
        /// </summary>
        public string BtnName { get => _btnName; set => _btnName = value; }

        /// <summary>
        /// Texto del boton editar descripción
        /// </summary>
        public string BtnDesc { get => _btnDesc; set => _btnDesc = value; }

        /// <summary>
        /// Texto del boton editar mensaje dijado
        /// </summary>
        public string BtnMsg { get => _btnMsg; set => _btnMsg = value; }

        /// <summary>
        /// Indica si el gremio es público o privado
        /// </summary>
        public bool Visibility { get => _visibility; set => _visibility = value; }

        /// <summary>
        /// Indica si el campo de editar nombre está habilitado
        /// </summary>
        public bool NameEnabled { get => _nameEnabled; set => _nameEnabled = value; }

        /// <summary>
        /// Indica si el campo de editar descripción está habilitado
        /// </summary>
        public bool DescEnabled { get => _descEnabled; set => _descEnabled = value; }

        /// <summary>
        /// Indica si el campo de editar mensaje fijado está habilitado
        /// </summary>
        public bool MsgEnabled { get => _msgEnabled; set => _msgEnabled = value; }

        /// <summary>
        /// Color de fondo del campo de la descripción
        /// </summary>
        public Color DescBackground { get => _descBackground; set => _descBackground = value; }

        /// <summary>
        /// Color de fondo del campo del mensaje fijado
        /// </summary>
        public Color MsgBackground { get => _msgBackground; set => _msgBackground = value; }

        /// <summary>
        /// Comando para cambiar la visibilidad del gremio
        /// </summary>
        public ICommand ChangeVisibility
        {
            get { return _changeVisibility = _changeVisibility ?? new DelegateCommand(async()=> await ChangeVisibilityCommand()); }
        }

        /// <summary>
        /// Comando para cambiar la imagen del gremio
        /// </summary>
        public ICommand ChangeImage
        {
            get { return _changeImage = _changeImage ?? new DelegateCommand(async()=> await ChangeImageCommand()); }
        }

        /// <summary>
        /// Comando para cambiar el nombre del gremio
        /// </summary>
        public ICommand ChangeName
        {
            get { return _changeName = _changeName ?? new DelegateCommand(async()=> await ChangeNameCommand()); }
        }

        /// <summary>
        /// Comando para cambiar la descripción del gremio
        /// </summary>
        public ICommand ChangeDesc
        {
            get { return _changeDesc = _changeDesc ?? new DelegateCommand(async()=> await ChangeDescCommand()); }
        }

        /// <summary>
        /// Comando para cambiar el mensaje fijado del gremio
        /// </summary>
        public ICommand ChangeMsg
        {
            get { return _changeMsg = _changeMsg ?? new DelegateCommand(async()=> await ChangeMsgCommand()); }
        }

        /// <summary>
        /// Acción del comando <seealso cref="ChangeImage"/>
        /// </summary>
        async Task ChangeImageCommand()
        {
            progress.Show();
            try
            {
                await CrossMedia.Current.Initialize();
                var file = await CrossMedia.Current.PickPhotoAsync();
                if (file != null)
                {
                    Stream stream = file.GetStream();
                    file.Dispose();
                    byte[] bytes;
                    using (MemoryStream ms = new MemoryStream())
                    {
                        stream.CopyTo(ms);
                        bytes = ms.ToArray();
                    }

                    if (bytes.Length > 320000)
                    {
                        progress.Hide();
                        var config = new AlertConfig() { Title = "Tamaño de imagen no soportado", Message = "La imagen es demasiado grande, elige otra (max 300 KB)" };
                        await UserDialogs.Instance.AlertAsync(config);
                        return;
                    }

                    guild.Image = bytes;
                    await GuildManager.DefaultManager.SaveGuildAsync(guild);

                    _source = ImageSource.FromStream(() =>
                    {
                        return new MemoryStream(bytes);
                    });
                    RaisePropertyChanged("Source");
                    MessagingCenter.Send<object, string>(this, ULFG.Forms.App.GuildChangedKey, "Edit " + guild.Id);
                }
            }
            catch (MediaPermissionException ex)
            {
                var config = new AlertConfig() { Title = "Error de permisos", Message = " Se necesitan permisos de lectura para adjuntar una imagen" };
                await UserDialogs.Instance.AlertAsync(config);
                Console.WriteLine("Error de permisos: " + ex.Message);
            }
            progress.Hide();
        }

        /// <summary>
        /// Acción del comando <seealso cref="ChangeName"/>
        /// </summary>
        async Task ChangeNameCommand()
        {           
            if (_name.Length > 3)
            {
                progress.Show();
                if (_btnName.Equals("Ok"))
                {
                    _nameEnabled = false;
                    guild.Name = _name;
                    await GuildManager.DefaultManager.SaveGuildAsync(guild);
                    _btnName = "Editar";
                    MessagingCenter.Send<object, string>(this, ULFG.Forms.App.GuildChangedKey, "Edit " + guild.Id);
                    RaisePropertyChanged("NameEnabled");
                    RaisePropertyChanged("BtnName");
                    RaisePropertyChanged("Name");
                }
                else
                {
                    _nameEnabled = true;
                    _btnName = "Ok";
                    RaisePropertyChanged("NameEnabled");
                    RaisePropertyChanged("BtnName");
                }
                progress.Hide();
            }
            else
            {
                var config = new AlertConfig() { Title = "Error", Message = "El nombre ha de tener al menos 4 caracteres" };
                await UserDialogs.Instance.AlertAsync(config);
            }
        }

        /// <summary>
        /// Acción del comando <seealso cref="ChangeDesc"/>
        /// </summary>
        async Task ChangeDescCommand()
        {
            progress.Show();
            if (_btnDesc.Equals("Ok"))
            {
                _descEnabled = false;
                guild.Description = _desc;
                _descBackground = Color.FromHex("#E0E0E0");
                _btnDesc = "Editar";
                await GuildManager.DefaultManager.SaveGuildAsync(guild);
                MessagingCenter.Send<object, string>(this, ULFG.Forms.App.GuildChangedKey, "Edit " + guild.Id);
                RaisePropertyChanged("BtnDesc");
                RaisePropertyChanged("DescEnabled");
                RaisePropertyChanged("Desc");
                RaisePropertyChanged("DescBackground");
            }
            else
            {
                _descEnabled = true;
                _btnDesc = "Ok";
                _descBackground = Color.White;
                RaisePropertyChanged("BtnDesc");
                RaisePropertyChanged("DescEnabled");
                RaisePropertyChanged("DescBackground");
            }
            progress.Hide();
        }

        /// <summary>
        /// Acción del comando <seealso cref="ChangeMsg"/>
        /// </summary>
        async Task ChangeMsgCommand()
        {
            progress.Show();
            {
                if (_btnMsg.Equals("Ok"))
                {
                    _msgEnabled = false;
                    guild.Message = _msg;
                    _msgBackground = Color.FromHex("#E0E0E0");
                    await GuildManager.DefaultManager.SaveGuildAsync(guild);
                    _btnMsg = "Editar";
                    MessagingCenter.Send<object, string>(this, ULFG.Forms.App.GuildChangedKey, "Edit " + guild.Id);
                    RaisePropertyChanged("MsgEnabled");
                    RaisePropertyChanged("BtnMsg");
                    RaisePropertyChanged("Msg");
                    RaisePropertyChanged("MsgBackground");
                }
                else
                {
                    _msgEnabled = true;
                    _btnMsg = "Ok";
                    _msgBackground = Color.White;
                    RaisePropertyChanged("MsgEnabled");
                    RaisePropertyChanged("BtnMsg");
                    RaisePropertyChanged("MsgBackground");
                }
                progress.Hide();
            }
        }

        /// <summary>
        /// Acción del comando <seealso cref="ChangeVisibility"/>
        /// </summary>
        async Task ChangeVisibilityCommand()
        {
            progress.Show();
            guild.IsPublic = _visibility;
            await GuildManager.DefaultManager.SaveGuildAsync(guild);
            RaisePropertyChanged("visibility");
            progress.Hide();
        }
    }
}
