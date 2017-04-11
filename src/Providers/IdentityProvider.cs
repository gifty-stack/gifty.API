using System;
using System.Security.Claims;
using gifty.Api.Auth;
using gifty.Api.Settings;
using Jose;
using Microsoft.Extensions.Options;
using Nancy;

namespace gifty.Api.Providers
{
    internal sealed class IdentityProvider : IIdentityProvider
    {
        private readonly AuthSettings _authSettings;
        private const string _bearerDeclaration = "Bearer ";

        public IdentityProvider(IOptions<AuthSettings> authOptions)
        {
            _authSettings = authOptions.Value;
        }

        public ClaimsPrincipal GetUserIdentity(NancyContext context)
        {
            try
            {
                var authorizationHeader = context.Request.Headers.Authorization;
                var jwt = authorizationHeader.Substring(0, _bearerDeclaration.Length);

                var authToken = Jose.JWT.Decode<AuthToken>(jwt, _authSettings.SecretKey, JwsAlgorithm.HS256);

                if(authToken.ExpirationDateTime < DateTime.UtcNow)
                    return null;
                
                var authUser = new AuthUser(authToken.UserName, authToken.UserLogin, authToken.UserId);   
                return new ClaimsPrincipal(authUser);             
            }
            catch(Exception)
            {
                return null;
            }
        }
    }
}