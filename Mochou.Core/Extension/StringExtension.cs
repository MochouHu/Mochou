using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mochou.Core.Extension
{
    public static class StringExtension
    {
        public static String InTheMiddle(this String str, String s, String p, int beg) => SU.InTheMiddle(str, s, p, beg);
        public static String InTheMiddle(this String str, String s, String p) => SU.InTheMiddle(str, s, p);
        public static String MD5(this String str) => U.MD5(str);
        public static int ToInt(this String s) => T.ToInt(s);
        public static bool ToBoolean(this String s) => T.ToBoolean(s);
    }
}
