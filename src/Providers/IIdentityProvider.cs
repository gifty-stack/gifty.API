using System.Security.Claims;
using Nancy;

namespace gifty.Api.Providers
{
    public interface IIdentityProvider
    {
         ClaimsPrincipal GetUserIdentity(NancyContext context);
    }
}