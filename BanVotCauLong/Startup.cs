using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(BanVotCauLong.Startup))]
namespace BanVotCauLong
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
