using Microsoft.Owin.Security.Infrastructure;
using RMDev_JwtExample.DomainClasses;
using RMDev_JwtExample.Services;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace RMDev_JwtExample.JsonWebTokenConfig
{
    public class RefreshTokenProvider : IAuthenticationTokenProvider
    {
        private readonly Func<ISecurityService> _securityService;
        private readonly IAppJwtConfiguration _configuration;
        private readonly Func<ITokenStoreService> _tokenStoreService;

        public RefreshTokenProvider(
            IAppJwtConfiguration configuration,
            Func<ITokenStoreService> tokenStoreService,
            Func<ISecurityService> securityService)
        {
            _configuration = configuration;
            _configuration.CheckArgumentNull(nameof(_configuration));

            _tokenStoreService = tokenStoreService;
            _tokenStoreService.CheckArgumentNull(nameof(_tokenStoreService));

            _securityService = securityService;
            _securityService.CheckArgumentNull(nameof(_securityService));
        }

        public void Create(AuthenticationTokenCreateContext context)
        {
            CreateAsync(context).RunSynchronously();
        }

        public async Task CreateAsync(AuthenticationTokenCreateContext context)
        {
            var refreshTokenId = Guid.NewGuid().ToString("n");

            var now = DateTime.UtcNow;
            var ownerUserId = context.Ticket.Identity.FindFirst(ClaimTypes.UserData).Value;
            var token = new UserToken
            {
                OwnerUserId = int.Parse(ownerUserId),
                // Refresh token handles should be treated as secrets and should be stored hashed
                RefreshTokenIdHash = _securityService().GetSha256Hash(refreshTokenId),
                Subject = context.Ticket.Identity.Name,
                RefreshTokenExpiresUtc = now.AddMinutes(Convert.ToDouble(_configuration.RefreshTokenExpirationMinutes)),
                AccessTokenExpirationDateTime = now.AddMinutes(Convert.ToDouble(_configuration.ExpirationMinutes))
            };

            context.Ticket.Properties.IssuedUtc = now;
            context.Ticket.Properties.ExpiresUtc = token.RefreshTokenExpiresUtc;

            token.RefreshToken = context.SerializeTicket();

            _tokenStoreService().CreateUserToken(token);
            _tokenStoreService().DeleteExpiredTokens();

            context.SetToken(refreshTokenId);
        }

        public void Receive(AuthenticationTokenReceiveContext context)
        {
            ReceiveAsync(context).RunSynchronously();
        }

        public async Task ReceiveAsync(AuthenticationTokenReceiveContext context)
        {
            var hashedTokenId = _securityService().GetSha256Hash(context.Token);
            var refreshToken = _tokenStoreService().FindToken(hashedTokenId);
            if (refreshToken != null)
            {
                context.DeserializeTicket(refreshToken.RefreshToken);
                _tokenStoreService().DeleteToken(hashedTokenId);
            }
        }
    }
}