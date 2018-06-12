using Microsoft.Owin;
using Owin;
using RMDev_JwtExample;
using RMDev_JwtExample.IoCConfig;
using RMDev_JwtExample.JsonWebTokenConfig;

[assembly: OwinStartup(typeof(OwinStartup))]
namespace RMDev_JwtExample
{
    public class OwinStartup
    {
        /// <summary>
        /// PM> Install-Package Microsoft.Owin.Host.SystemWeb
        /// PM> Install-Package Microsoft.Owin.Security.Jwt
        /// </summary>
        public void Configuration(IAppBuilder app)
        {
            app.UseOAuthAuthorizationServer(SmObjectFactory.Container.GetInstance<AppOAuthOptions>());
            app.UseJwtBearerAuthentication(SmObjectFactory.Container.GetInstance<AppJwtOptions>());
        }
    }
}