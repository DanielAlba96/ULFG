using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(ULFGService.Startup))]

namespace ULFGService
{
    /// <summary>
    /// Inicializador por defecto de la aplicaci�n
    /// </summary>
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureMobileApp(app);
        }
    }
}