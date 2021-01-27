using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Anyware.Startup))]
namespace Anyware
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            app.MapSignalR();
        }
    }
}
