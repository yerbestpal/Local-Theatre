using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(AssessmentLocalTheatre.Startup))]
namespace AssessmentLocalTheatre
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
