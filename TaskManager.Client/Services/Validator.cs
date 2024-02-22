using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TaskManager.Client.Services
{
    public class Validator
    {
        public static bool ValidateEmail(string email)
        {
            string emailPattern = @"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$";
            return Regex.IsMatch(email, emailPattern);
        }

        public static bool ValidatePassword(string password)
        {
            return password.Length >= 6 && Regex.IsMatch(password, @"^[a-zA-Z0-9]+$");
        }
    }
}
