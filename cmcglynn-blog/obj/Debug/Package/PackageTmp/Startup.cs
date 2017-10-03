using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(cmcglynn_blog.Startup))]
namespace cmcglynn_blog
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
