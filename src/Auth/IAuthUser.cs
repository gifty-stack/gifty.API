using System;
using System.Security.Principal;

namespace gifty.Api.Auth
{
    internal interface IAuthUser : IIdentity
    {
        string Login { get; }
        Guid Id { get; }
    }
}