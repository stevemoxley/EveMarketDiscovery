using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(EveAccountantWeb.Startup))]
namespace EveAccountantWeb
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
