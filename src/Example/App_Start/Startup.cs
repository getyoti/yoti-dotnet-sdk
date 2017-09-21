using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(Example.Startup))]

namespace Example
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            this.ConfigureAuth(app);
        }
    }
}