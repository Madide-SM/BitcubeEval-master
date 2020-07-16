using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Bitcubeeval.Startup))]
namespace Bitcubeeval
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
