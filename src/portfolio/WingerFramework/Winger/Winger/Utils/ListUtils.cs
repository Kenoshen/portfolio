using System.Collections.Generic;
using System.Text;

namespace Winger.Utils
{
    public static class ListUtils
    {
        public static List<T> ToList<T>(params T[] args)
        {
            List<T> list = new List<T>();
            if (args != null)
            {
                for (int i = 0; i < args.Length; i++)
                {
                    list.Add(args[i]);
                }
            }
            return list;
        }

        public static string ListToString<T>(List<T> list)
        {
            StringBuilder sb = new StringBuilder("[");
            if (list.Count > 0)
            {
                sb.Append(list[0]);
                for(int i = 1; i < list.Count; i++)
                {
                    sb.Append(", " + list[i]);
                }
            }
            sb.Append("]");
            return sb.ToString();
        }

        public static List<OUTPUT> CastToList<INPUT, OUTPUT>(List<INPUT> list)
        {
            List<OUTPUT> newList = new List<OUTPUT>();
            foreach (INPUT i in list)
            {
                // TODO: figure out how to cast generic to generic 
                // newList.Add((OUTPUT)i);
            }
            return newList;
        }
    }
}
