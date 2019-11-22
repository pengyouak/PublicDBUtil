using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using NLog;

namespace PublicDBUtil
{
    public class LogHelper
    {
        public static ILogger Logger
        {
            get
            {
                return LogManager.GetLogger(GetCurrentMethodFullName());
            }
        }

        private static string GetCurrentMethodFullName()
        {
            StackFrame frame;
            string str;
            string str1;
            bool flag;
            try
            {
                int num = 2;
                StackTrace stackTrace = new StackTrace();
                int length = stackTrace.GetFrames().Length;
                do
                {
                    int num1 = num;
                    num = num1 + 1;
                    frame = stackTrace.GetFrame(num1);
                    str = frame.GetMethod().DeclaringType.ToString();
                    flag = (!str.EndsWith("Exception") ? false : num < length);
                }
                while (flag);
                string name = frame.GetMethod().Name;
                str1 = string.Concat(str, ".", name);
            }
            catch
            {
                str1 = null;
            }

            Console.WriteLine(str1);
            return str1;
        }
    }
}
