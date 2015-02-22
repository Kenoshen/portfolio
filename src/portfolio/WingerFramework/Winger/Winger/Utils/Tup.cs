using System.Collections.Generic;

namespace Winger.Utils
{
    public class Tup<T>
    {
        private List<T> collection = new List<T>();
        public Tup(params T[] args)
        {
            if (args != null)
            {
                for (int i = 0; i < args.Length; i++)
                {
                    collection.Add(args[i]);
                }
            }
        }

        public T this[int i]
        {
            get { return Get(i); }
        }

        public T Get(int i)
        {
            if (collection.Count > 0)
            {
                i = i % collection.Count;
                if (i < 0)
                {
                    i = collection.Count + i;
                }
                return collection[i];
            }
            else
            {
                return default(T);
            }
        }
    }


    public class Tup<T1, T2>
    {
        private const int count2 = 2;
        public T1 Item1 { get; private set; }
        public T2 Item2 { get; private set; }

        public Tup(T1 item1, T2 item2)
        {
            Item1 = item1;
            Item2 = item2;
        }

        public object this[int i]
        {
            get { return Get(i); }
        }

        public object Get(int i)
        {
            i = i % count2;
            if (i < 0)
            {
                i = count2 + i;
            }
            switch (i)
            {
                case 0: return Item1;
                case 1: return Item2;
                default: return Item1;
            }
        }
    }


    public class Tup<T1, T2, T3>
    {
        private const int count3 = 3;
        public T1 Item1 { get; private set; }
        public T2 Item2 { get; private set; }
        public T3 Item3 { get; private set; }

        public Tup(T1 item1, T2 item2, T3 item3)
        {
            Item1 = item1;
            Item2 = item2;
            Item3 = item3;
        }

        public object this[int i]
        {
            get { return Get(i); }
        }

        public object Get(int i)
        {
            i = i % count3;
            if (i < 0)
            {
                i = count3 + i;
            }
            switch (i)
            {
                case 0: return Item1;
                case 1: return Item2;
                case 2: return Item3;
                default: return Item1;
            }
        }
    }


    public class Tup<T1, T2, T3, T4>
    {
        private const int count4 = 4;
        public T1 Item1 { get; private set; }
        public T2 Item2 { get; private set; }
        public T3 Item3 { get; private set; }
        public T4 Item4 { get; private set; }

        public Tup(T1 item1, T2 item2, T3 item3, T4 item4)
        {
            Item1 = item1;
            Item2 = item2;
            Item3 = item3;
            Item4 = item4;
        }

        public object this[int i]
        {
            get { return Get(i); }
        }

        public object Get(int i)
        {
            i = i % count4;
            if (i < 0)
            {
                i = count4 + i;
            }
            switch (i)
            {
                case 0: return Item1;
                case 1: return Item2;
                case 2: return Item3;
                case 3: return Item4;
                default: return Item1;
            }
        }
    }
}
