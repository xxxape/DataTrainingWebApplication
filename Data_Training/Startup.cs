using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Data_Training.Startup))]
namespace Data_Training
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.MapSignalR();
            ConfigureAuth(app);
        }
    }
}
