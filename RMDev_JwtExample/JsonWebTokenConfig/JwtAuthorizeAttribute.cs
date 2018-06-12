using System;
using System.Linq;
using System.Security.Claims;
using System.Web.Http;
using System.Web.Http.Controllers;
using RMDev_JwtExample.Services;

namespace RMDev_JwtExample.JsonWebTokenConfig
{
    public class JwtAuthorizeAttribute : AuthorizeAttribute
    {
        public Func<IUsersService> UsersService { set; get; }
        public Func<ITokenStoreService> TokenStoreService { set; get; }
        public override void OnAuthorization(HttpActionContext actionContext)
        {
            if (skipAuthorization(actionContext))
            {
                return;
            }

            var accessToken = actionContext.Request.Headers.Authorization.Parameter;
            if (string.IsNullOrWhiteSpace(accessToken) ||
                accessToken.Equals("undefined", StringComparison.OrdinalIgnoreCase))
            {
                // token nulo
                this.HandleUnauthorizedRequest(actionContext);
                return;
            }

            var claimsIdentity = actionContext.RequestContext.Principal.Identity as ClaimsIdentity;
            if (claimsIdentity?.Claims == null || !claimsIdentity.Claims.Any())
            {
                this.HandleUnauthorizedRequest(actionContext);
                return;
            }

            var userId = claimsIdentity.FindFirst(ClaimTypes.UserData).Value;

            var serialNumberClaim = claimsIdentity.FindFirst(ClaimTypes.SerialNumber);
            if (serialNumberClaim == null)
            {
                this.HandleUnauthorizedRequest(actionContext);
                return;
            }

            if (UsersService == null)
            {
                throw new NullReferenceException($"{nameof(UsersService)} is null. Make sure ioc.Policies.SetAllProperties is configured and also IFilterProvider is replaced with SmWebApiFilterProvider.");
            }

            var serialNumber = UsersService().GetSerialNumber(int.Parse(userId));
            if (serialNumber != serialNumberClaim.Value)
            {
                this.HandleUnauthorizedRequest(actionContext);
                return;
            }

            if (TokenStoreService == null)
            {
                throw new NullReferenceException($"{nameof(TokenStoreService)} is null. Make sure ioc.Policies.SetAllProperties is configured and also IFilterProvider is replaced with SmWebApiFilterProvider.");
            }

            if (!TokenStoreService().IsValidToken(accessToken, int.Parse(userId)))
            {
                // this is not our issued token
                this.HandleUnauthorizedRequest(actionContext);
                return;
            }

            base.OnAuthorization(actionContext);
        }

        private static bool skipAuthorization(HttpActionContext actionContext)
        {
            return actionContext.ActionDescriptor.GetCustomAttributes<AllowAnonymousAttribute>().Any()
                   || actionContext.ControllerContext.ControllerDescriptor.GetCustomAttributes<AllowAnonymousAttribute>().Any();
        }
    }
}