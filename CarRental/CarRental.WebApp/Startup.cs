using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(CarRental.WebApp.Startup))]
namespace CarRental.WebApp
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
