using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(LeaveYourCouch.Mvc.Startup))]
namespace LeaveYourCouch.Mvc
{
    public partial class Startup
    {

        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);

            
        }
    }
}
