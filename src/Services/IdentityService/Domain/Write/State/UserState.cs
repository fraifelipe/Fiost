using System;
using System.Collections.Generic;

namespace IdentityService.Domain.Write.State
{
    public class UserState
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName => FirstName + " " + LastName;
        public string Username { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public string Role { get; set; }
        public string Token { get; set; }
        public IList<UserState> Following { get; set; } = new List<UserState>();
        //public IList<UserState> Following { get; set; } = new List<UserState>();
    }
}