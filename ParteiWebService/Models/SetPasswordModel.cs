using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Aufgabe_2.Models
{
    public class SetPasswordModel
    {
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string PasswordCode { get; set; }
        public string EmailConfirmationCode { get; set; }
        public string UserId { get; set; }
        public string Username { get; set; }
    }
}
