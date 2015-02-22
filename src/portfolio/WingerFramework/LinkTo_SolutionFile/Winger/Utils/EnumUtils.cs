using System;
using System.Collections.Generic;
using System.Linq;

namespace Winger.Utils
{
    /// <summary>
    /// Helper class for enums
    /// </summary>
    public static class EnumUtils
    {
        /// <summary>
        /// Gives you a var of enum values
        /// </summary>
        /// <typeparam name="T">the enum class</typeparam>
        /// <returns>the enum values</returns>
        public static IEnumerable<T> GetValues<T>()
        {
            return Enum.GetValues(typeof(T)).Cast<T>();
        }
    }
}
