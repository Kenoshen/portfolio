using System;

namespace Winger.Utils
{
    public class Log
    {
        public LogLevel Level { get; set; }
        public string Prefix { get; set; }

        public Log()
        {
            Level = LogLevel.INFO;
            Prefix = "";
        }

        public Log(string prefix)
        {
            Level = LogLevel.INFO;
            Prefix = prefix;
        }

        public Log(string prefix, LogLevel level)
        {
            Level = level;
            Prefix = prefix;
        }

        public Log(LogLevel level)
        {
            Level = level;
            Prefix = "";
        }

        public void Info(string s)
        {
            if (Level == LogLevel.DEBUG || Level == LogLevel.INFO)
            {
                LogInfo(Prefix + s);
            }
        }

        public void Debug(string s)
        {
            if (Level == LogLevel.DEBUG)
            {
                LogDebug(Prefix + s);
            }
        }

        public void Error(string s)
        {
            LogError(Prefix + s);
        }

        public static void LogInfo(string s)
        {
            Console.WriteLine(s);
        }

        public static void LogDebug(string s)
        {
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine(s);
            Console.ResetColor();
        }

        public static void LogError(string s)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(s);
            Console.ResetColor();
        }
    }

    public enum LogLevel
    {
        INFO,
        DEBUG,
        ERROR,
    }
}
