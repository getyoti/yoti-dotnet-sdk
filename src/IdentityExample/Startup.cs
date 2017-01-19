using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(IdentityExample.Startup))]
namespace IdentityExample
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
