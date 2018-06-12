using Microsoft.Owin;
using Microsoft.Owin.Security.Infrastructure;
using Microsoft.Owin.Security.OAuth;
using RMDev_JwtExample.IoCConfig;
using System;

namespace RMDev_JwtExample.JsonWebTokenConfig
{
    public class AppOAuthOptions : OAuthAuthorizationServerOptions
    {
        public AppOAuthOptions(IAppJwtConfiguration configuration)
        {
            this.AllowInsecureHttp = true; // TODO: Alterar ao comprar um certificado SSL.
            this.TokenEndpointPath = new PathString(configuration.TokenPath);
            this.AccessTokenExpireTimeSpan = TimeSpan.FromMinutes(configuration.ExpirationMinutes);
            this.AccessTokenFormat = new AppJwtWriterFormat(this, configuration);
            this.Provider = SmObjectFactory.Container.GetInstance<IOAuthAuthorizationServerProvider>();
            this.RefreshTokenProvider = SmObjectFactory.Container.GetInstance<IAuthenticationTokenProvider>();
        }
    }
}