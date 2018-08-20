using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ProIntegracao.UI.Startup))]
namespace ProIntegracao.UI
{
    /// <summary>
    /// StartUp
    /// </summary>
    public partial class Startup
    {
        /// <summary>
        /// Configuration
        /// </summary>
        /// <param name="app"></param>
        public void Configuration(IAppBuilder app)
        {
            //ConfigureAuth(app);
        }
    }
}
