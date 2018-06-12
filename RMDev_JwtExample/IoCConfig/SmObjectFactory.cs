using System;
using StructureMap;
using System.Threading;
using RMDev_JwtExample.JsonWebTokenConfig;
using RMDev_JwtExample.Services;
using Microsoft.Owin.Security.Infrastructure;
using Microsoft.Owin.Security.OAuth;
using StructureMap.Web;

namespace RMDev_JwtExample.IoCConfig
{
    public static class SmObjectFactory
    {
        private static readonly Lazy<Container> _containerBuilder =
            new Lazy<Container>(defaultContainer, LazyThreadSafetyMode.ExecutionAndPublication);

        public static IContainer Container { get; } = _containerBuilder.Value;

        private static Container defaultContainer()
        {
            return new Container(ioc =>
            {
                ioc.For<IAppJwtConfiguration>().Singleton().Use(() => AppJwtConfiguration.Config);
                ioc.For<IUsersService>().HybridHttpOrThreadLocalScoped().Use<UsersService>();
                ioc.For<ITokenStoreService>().HybridHttpOrThreadLocalScoped().Use<TokenStoreService>();
                ioc.For<ISecurityService>().HybridHttpOrThreadLocalScoped().Use<SecurityService>();

                ioc.Policies.SetAllProperties(setterConvention =>
                {
                    setterConvention.OfType<Func<IUsersService>>();
                    setterConvention.OfType<Func<ITokenStoreService>>();
                });

                ioc.For<IOAuthAuthorizationServerProvider>().Singleton().Use<AppOAuthProvider>();
                ioc.For<IAuthenticationTokenProvider>().Singleton().Use<RefreshTokenProvider>();
            });
        }
    }
}