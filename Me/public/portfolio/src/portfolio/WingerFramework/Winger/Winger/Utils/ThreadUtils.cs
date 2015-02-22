using System.Threading;

namespace Winger.Utils
{
    public static class ThreadUtils
    {
        public static void Pause(int milliseconds)
        {
            System.Threading.Thread.Sleep(milliseconds);
        }


        public static Thread StartUpInThread(ThreadStart func)
        {
            Thread t = new Thread(func);
            t.Start();
            return t;
        }


        public static Thread StartUpInThread(ParameterizedThreadStart func, object arg)
        {
            Thread t = new Thread(func);
            t.Start(arg);
            return t;
        }
    }
}
