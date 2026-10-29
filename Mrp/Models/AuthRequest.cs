using System;
using System.Collections.Generic;
using System.Text;

namespace Mrp.Models
{
    public class AuthRequest
    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
