using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ProjectX1._5.Startup))]
namespace ProjectX1._5
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
