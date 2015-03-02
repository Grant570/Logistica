using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Logistica_Review.Startup))]
namespace Logistica_Review
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
