using System.Security.Claims;
using System.Web.Http;
using RMDev_JwtExample.JsonWebTokenConfig;
using RMDev_JwtExample.Services;

namespace RMDev_JwtExample.Controllers
{
    [RoutePrefix("api/user")]
    public class UserController : ApiController
    {
        private readonly ITokenStoreService _tokenStoreService;

        public UserController(ITokenStoreService tokenStoreService)
        {
            _tokenStoreService = tokenStoreService;
        }

        [JwtAuthorize]
        [Route("logout")]
        [HttpGet, HttpPost]
        public IHttpActionResult Logout()
        {
            var claimsIdentity = this.User.Identity as ClaimsIdentity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.UserData).Value;
            _tokenStoreService.InvalidateUserTokens(int.Parse(userId));
            _tokenStoreService.DeleteExpiredTokens();

            return this.Ok(new { message = "Logout successful. =)" });
        }
    }
}