using System;

namespace gifty.Api.Auth
{
    internal class AuthUser : IAuthUser
    {
        public string AuthenticationType => "Test";
        public bool IsAuthenticated { get; }
        public string Name { get; }
        public string Login { get; }
        public Guid Id { get; }

        public AuthUser(string name, string login, Guid id)
        {
            Name = name;
            Login = login;
            Id = id;
            IsAuthenticated = true;
        }
    }
}