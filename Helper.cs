using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppMini
{
    public static class Helper
    {
        public static bool StartsWithCapitalLetter(this string str)
            => !string.IsNullOrEmpty(str) && Char.IsUpper(str[0]);

        public static bool Valid(this string s)
        => !string.IsNullOrEmpty(s) && !s.Contains(" ") && s.Length >= 3;

        public static bool ValidClassroomName(this string s)
        => !string.IsNullOrEmpty(s) &&
            s.Length == 5 &&
            char.IsUpper(s[0]) &&
            char.IsUpper(s[1]) &&
            char.IsDigit(s[2]) &&
            char.IsDigit(s[3]) &&
            char.IsDigit(s[4]);
    }
}
