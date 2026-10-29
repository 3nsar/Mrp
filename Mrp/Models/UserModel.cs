using System;
using System.Collections.Generic;
using System.Text;

namespace Mrp.Models
{
    public class UserModel
    {
        public Guid ID { get; set; } = Guid.Empty;
        public string Username { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string Salt { get; set; } = string.Empty;
        public string? Token { get; set; }
    }
}
