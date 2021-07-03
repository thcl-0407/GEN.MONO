using System.Text.RegularExpressions;

namespace Extension
{
    public static class StringExtension
    {
        public static bool IsNullOrEmpty(this string value)
        {
            if (value == null)
            {
                return true;
            }

            if (value.Trim().Equals(""))
            {
                return true;
            }

            return false;
        }

        public static bool IsNotNullOrEmpty(this string value)
        {
            if (value == null)
            {
                return false;
            }

            if (value.Trim().Equals(""))
            {
                return false;
            }

            return true;
        }

        public static bool IsRightPhoneNo(this string value)
        {
            Regex regex = new Regex(@"^0[1-9][0-9]{8}$");

            return regex.IsMatch(value);
        }

        public static bool IsRightEmail(this string value)
        {
            Regex regex = new Regex(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$");

            return regex.IsMatch(value);
        }
    }
}
