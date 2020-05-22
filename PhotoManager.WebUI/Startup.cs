using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(PhotoManager.WebUI.Startup))]
namespace PhotoManager.WebUI
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
