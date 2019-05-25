using System.Collections.Generic;

namespace Utils
{
    public class LogHelper
    {
        public static string List2Str<T>(IEnumerable<T> list)
        {
            string str = "";
            string suffix = "-";
            foreach (var item in list)
            {
                str += item + suffix;
            }
            //移除掉最后末尾那个"-"
            str = str.Remove(str.Length-1, 1);

            return str;
        }
    }
}
