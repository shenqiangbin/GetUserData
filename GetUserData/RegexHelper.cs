using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace GetUserData
{
    public class RegexHelper
    {
        public static bool IsTel(string tel)
        {
            if (string.IsNullOrEmpty(tel))
                return false;

            return Regex.IsMatch(tel, "^((1[3,5,8][0-9])|(14[5,7])|(17[0,1,3,6,7,8]))\\d{8}$");
        }
    }
}
