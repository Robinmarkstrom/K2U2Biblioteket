using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biblioteket.LibraryClient.Core
{
    public static class AuthenticatorService
    {
        public static AdminUser? Login(string personnummer, string pin)
        {
            if (personnummer == "19980223-7256" && pin == "1234")
            {
                return new AdminUser
                {
                    Name = "Robin Markström",
                    PersonNumber = personnummer,
                    IsAdmin = true
                };
            }

            return null;
        }
    }
}
