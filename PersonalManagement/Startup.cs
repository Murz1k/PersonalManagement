using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(PersonalManagement.Startup))]
namespace PersonalManagement
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
