using PROJECT_PRN221.Models;
using System.Text.RegularExpressions;

namespace PROJECT_PRN221.Utils
{
    public class Validation
    {
        public static bool IsUsernameUnique(string username, ProjectPrn221Context context)
        {
            return !context.Customers.Any(c => c.Username == username);
        }

        public static bool IsUsernameAdmin(string username, ProjectPrn221Context context)
        {
            return !context.Admins.Any(c => c.Username == username);
        }

        public static bool IsEmailValid(string email)
        {
            string emailPattern = @"^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,4}$";
            Regex regex = new Regex(emailPattern);
            return regex.IsMatch(email);
        }

        public static bool IsPasswordValid(string password)
        {
            return Regex.IsMatch(password, @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{6,}$");
        }

        public static bool IsPrice(double? price)
        {
            if (price > 0)
            {
                return true;
            }
            return false;
        }

        public static bool IsDiscount(double? discount)
        {
            if (discount >= 0 && discount <= 1)
            {
                return true;
            }
            return false;
        }
    }
}
