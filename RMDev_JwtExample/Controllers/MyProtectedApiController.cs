using System.Web.Http;
using RMDev_JwtExample.JsonWebTokenConfig;

namespace RMDev_JwtExample.Controllers
{
    [JwtAuthorize]
    public class MyProtectedApiController : ApiController
    {
        public IHttpActionResult Get()
        {
            return Ok(new
            {
                Id = 1,
                Title = "Message from 'My Protected Controller'!",
                Username = this.User.Identity.Name
            });
        }
    }
}