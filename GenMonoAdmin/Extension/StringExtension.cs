using System;
using System.Text.RegularExpressions;

namespace Extension
{
    public static class StringExtension
    {
        public static bool isNullOrEmpty(this string value)
        {
            if(value == null)
            {
                return true;
            }

            if (value.Trim().Equals(""))
            {
                return true;
            }

            return false;
        }

        public static bool isNotNullOrEmpty(this string value)
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

        public static bool isRightPhone(this string value)
        {
            Regex regex = new Regex(@"^0[1-9][0-9]{8}$");

            return regex.IsMatch(value);
        }

        public static bool isRightEmail(this string value)
        {
            Regex regex = new Regex(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$");

            return regex.IsMatch(value);
        }
    }
}
