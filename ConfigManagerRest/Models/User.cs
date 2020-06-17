using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ConfigManagerRest.Models
{
    public class SessionUser
    {
        public Guid SessionId { get; set; }
        public string Token { get; set; }
    }
    public class User : SessionUser
    {
        public Guid Guid { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }

    internal class TokenUser : User
    {
        public DateTime ExpirationDateTime { get; set; }
    }
}