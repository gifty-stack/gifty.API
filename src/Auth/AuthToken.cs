using System;

namespace gifty.Api.Auth
{
    internal sealed class AuthToken
    {
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public string UserLogin { get; set; }
        public DateTime ExpirationDateTime { get; set; }
    }
}