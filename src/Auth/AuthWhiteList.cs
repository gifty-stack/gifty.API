using System.Collections.Generic;

namespace gifty.Api.Auth
{
    internal static class AuthWhiteList
    {
        internal static IEnumerable<string> WhiteList { get; } = new List<string>
        {
            "/",
            "/users/register",
            "/users/login"
        };
    }
}