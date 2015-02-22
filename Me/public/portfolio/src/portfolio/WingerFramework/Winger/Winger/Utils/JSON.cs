using System;
using System.Collections.Generic;
using System.Linq;
using Winger.Network.Utils;
using System.Collections;

namespace Winger.Utils
{
    /// <summary>
    /// JSON stands for Java Script Object Notation.  It is a useful data type for mapping objects.  It is also a 'live' object.  In other words, it is mutable.
    /// </summary>
    public class JSON : Byteable, IEnumerator<object>, IEnumerable<object>
    {
        private bool isArray = false;
        private Dictionary<string, object> map;
        private List<object> array;

        private int enumeratorPosition = 0;
        private List<object> enumeratorList = new List<object>();


        /// <summary>
        /// Initialize a JSON object with a json-formatted string
        /// </summary>
        /// <param name="jsonString">a json-formatted string</param>
        public JSON(string jsonString)
        {
            ParseString(jsonString, 0);
        }

        /// <summary>
        /// Get and set the object at the value of the xpath
        /// </summary>
        /// <param name="xpath">the property to set or get</param>
        /// <returns>the object being set or gotten</returns>
        public object this[string xpath]
        {
            get { return Get(xpath); }
            set { Put(xpath, value); }
        }

        /// <summary>
        /// Gets an object at a given xpath.
        /// 
        /// Ex:
        /// books // gets the object under the property name "books"
        /// books.0.price // gets the price of the first book
        /// books.# // gets a list of book objects
        /// books.#.price // gets a list of price objects
        /// 
        /// </summary>
        /// <param name="xpath">xpath (not necessarily fully implemented)</param>
        /// <returns>the object that matches the path, or null if nothing matches</returns>
        public object Get(string xpath)
        {
            try
            {
                string[] parts = xpath.Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries);
                return Get(parts, 0);
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pathParts"></param>
        /// <param name="depth"></param>
        /// <returns></returns>
        private object Get(string[] pathParts, int depth)
        {
            string curPath = pathParts[depth];
            int n = 0;
            if (isArray)
            {
                if (curPath.Equals("#"))
                {
                    if (depth + 1 >= pathParts.Length)
                    {
                        return array;
                    }
                    else
                    {
                        List<object> objs = new List<object>();
                        foreach (object o in array)
                        {
                            if (o is JSON)
                            {
                                objs.Add(((JSON)o).Get(pathParts, depth + 1));
                            }
                        }
                        return objs;
                    }
                }
                else if (Int32.TryParse(curPath, out n))
                {
                    object o = array[n];
                    if (depth + 1 >= pathParts.Length)
                    {
                        return o;
                    }
                    else if (o is JSON)
                    {
                        return ((JSON)array[n]).Get(pathParts, depth + 1);
                    }
                }
            }
            else
            {
                if (map.ContainsKey(pathParts[depth]))
                {
                    object o = map[pathParts[depth]];
                    if (depth + 1 >= pathParts.Length)
                    {
                        return o;
                    }
                    else if (o is JSON)
                    {
                        return ((JSON)o).Get(pathParts, depth + 1);
                    }
                }
            }
            return null;
        }
        
        /// <summary>
        /// Puts an object at a given xpath. 
        /// 
        /// Ex:
        /// books // sets the books object array to a given object
        /// books.0 // sets the first object in the books object array to a given object
        /// books.0.price // sets the price of the first object in the books array
        /// books.# // adds an object to the end of the books array
        /// </summary>
        /// <param name="xpath">xpath (not necessarily fully implemented)</param>
        /// <param name="val">the object to set</param>
        /// <returns>true if the object was succesfully put, false if not</returns>
        public bool Put(string xpath, object val)
        {
            if (xpath == null || xpath.Length == 0)
            {
                return false;
            }
            string key = null;
            object parent = null;
            string[] parts = xpath.Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length > 1)
            {
                string[] newParts = new string[parts.Length - 1];
                for (int i = 0; i < newParts.Length; i++)
                {
                    newParts[i] = parts[i];
                }
                key = parts[parts.Length - 1];
                if (newParts.Length == 0)
                {
                    parent = this;
                }
                else
                {
                    parent = Get(newParts, 0);
                }
            }
            else
            {
                key = parts[0];
                parent = this;
            }

            if (parent != null && parent is JSON)
            {
                JSON jParent = parent as JSON;
                if (jParent.isArray)
                {
                    int index = 0;
                    if (key == "#")
                    {
                        jParent.array.Add(val);
                        return true;
                    }
                    else if (Int32.TryParse(key, out index))
                    {
                        jParent.array[index] = val;
                        return true;
                    }
                }
                else
                {
                    jParent.map[key] = val;
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Gets the keys/indexes within this object
        /// </summary>
        /// <returns>a list of properties</returns>
        public List<string> Properties()
        {
            if (!isArray)
            {
                return map.Keys.ToList<string>();
            }
            else
            {
                List<string> indexes = new List<string>();
                for (int i = 0; i < array.Count; i++)
                {
                    indexes.Add("" + i);
                }
                return indexes;
            }
        }

        /// <summary>
        /// Checks if a property exists
        /// </summary>
        /// <param name="xpath">the xpath to the property</param>
        /// <returns>true if the property exists</returns>
        public bool HasProperty(string xpath)
        {
            if (xpath == null || xpath == "")
            {
                return false;
            }
            string[] pathParts = xpath.Split('.');
            return HasProperty(pathParts, 0, this);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pathParts"></param>
        /// <param name="depth"></param>
        /// <param name="cur"></param>
        /// <returns></returns>
        private bool HasProperty(string[] pathParts, int depth, object cur)
        {
            if (depth >= pathParts.Length)
            {
                return true;
            }
            if (cur == null || !(cur is JSON))
            {
                return false;
            }
            JSON curJ = cur as JSON;
            string property = pathParts[depth];
            if (curJ.Properties().Contains(property))
            {
                return HasProperty(pathParts, depth + 1, curJ.Get(property));
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Gets the count of the number of properties
        /// </summary>
        /// <returns>the count of the number of properties</returns>
        public int Length()
        {
            return Properties().Count;
        }

        #region Parse
        public static JSON Parse(string jsonString)
        {
            return new JSON(jsonString);
        }

        private JSON()
        {

        }

        private int ParseString(string jsonString, int index)
        {
            if (jsonString == null)
            {
                throw new Exception("Cannot parse null JSON string.");
            }
            if (jsonString == "")
            {
                throw new Exception("Cannot parse empty JSON string.");
            }
            index = StreamToNotWhiteSpace(jsonString, index);
            if (jsonString[index].Equals('{'))
            {
                isArray = false;
                map = new Dictionary<string, object>();

                int i = index + 1;
                int k = 0;
                while (true)
                {
                    i = StreamToNotWhiteSpace(jsonString, i);
                    char startOfFind = jsonString[i];
                    if (startOfFind.Equals('"'))
                    {
                        // this is a key
                        i++;
                        k = StreamToChar(jsonString, '"', i, false, true);
                        string key = jsonString.Substring(i, k - i);
                        // finding :
                        k++;
                        i = StreamToChar(jsonString, ':', k, true, false);
                        i++;
                        // finding obj/str/num
                        i = StreamToNotWhiteSpace(jsonString, i);
                        char startOfObj = jsonString[i];
                        if (startOfObj.Equals('{') || startOfObj.Equals('['))
                        {
                            // its an object/array (basically another JSON object)
                            JSON j = new JSON();
                            i = j.ParseString(jsonString, i);
                            map.Add(key, j);
                            i = StreamToNotWhiteSpace(jsonString, i);
                            char maybeEndChar = jsonString[i];
                            i++;
                            if (maybeEndChar.Equals(','))
                            {
                                // this is means there is another key:value
                            }
                            else if (maybeEndChar.Equals('}'))
                            {
                                // this is the end of the object
                                return i;
                            }
                            else
                            {
                                throw new Exception("Couldn't find end ',' or '}'. index:" + i + "  " + GetHelperString(jsonString, i, 100));
                            }
                        }
                        else if (startOfObj.Equals('"'))
                        {
                            // its a string
                            i++;
                            k = StreamToChar(jsonString, '"', i, true, true);
                            string val = jsonString.Substring(i, k - i);
                            map.Add(key, val);
                            k++;
                            i = StreamToNotWhiteSpace(jsonString, k);
                            char maybeEndChar = jsonString[i];
                            i++;
                            if (maybeEndChar.Equals(','))
                            {
                                // this is means there is another key:value
                            }
                            else if (maybeEndChar.Equals('}'))
                            {
                                // this is the end of the object
                                return i;
                            }
                            else
                            {
                                throw new Exception("Couldn't find end ',' or '}'. index:" + i + "  " + GetHelperString(jsonString, i, 100));
                            }
                        }
                        else if (startOfObj.Equals(','))
                        {
                            // its null
                            map.Add(key, null);
                            i++;
                        }
                        else if (startOfObj.Equals('}'))
                        {
                            // its null and its the end of the obj
                            map.Add(key, null);
                            i++;
                            return i;
                        }
                        else if (startOfObj.Equals('t') || startOfObj.Equals('f') ||
                            startOfObj.Equals('T') || startOfObj.Equals('F'))
                        {
                            // its a boolean
                            if (jsonString.Substring(i, 4).ToLower().Equals("true"))
                            {
                                i += 4;
                                map.Add(key, true);
                            }
                            else if (jsonString.Substring(i, 5).ToLower().Equals("false"))
                            {
                                i += 5;
                                map.Add(key, false);
                            }
                            else
                            {
                                throw new Exception("Unrecognized characters at index: " + i + "  " + GetHelperString(jsonString, i, 100));
                            }

                            i = StreamToNotWhiteSpace(jsonString, i);
                            char maybeEndChar = jsonString[i];
                            i++;
                            if (maybeEndChar.Equals(','))
                            {
                                // this is means there is another key:value
                            }
                            else if (maybeEndChar.Equals('}'))
                            {
                                // this is the end of the object
                                return i;
                            }
                            else
                            {
                                throw new Exception("Couldn't find end ',' or '}'. index:" + i + "  " + GetHelperString(jsonString, i, 100));
                            }
                        }
                        else
                        {
                            bool isNegative = false;
                            if (startOfObj.Equals('-'))
                            {
                                i++;
                                startOfObj = jsonString[i];
                                isNegative = true;
                            }
                            double d = 0;
                            if (Double.TryParse(startOfObj.ToString(), out d))
                            {
                                // its a number
                                k = StreamToChars(jsonString, i, new char[] { ' ', '\t', 'n', ',', '}' });
                                string valStr = jsonString.Substring(i, k - i);
                                double val = Double.Parse(valStr);
                                if (isNegative)
                                {
                                    val *= -1;
                                }
                                map.Add(key, val);
                                i = StreamToNotWhiteSpace(jsonString, k);
                                char maybeEndChar = jsonString[i];
                                i++;
                                if (maybeEndChar.Equals(','))
                                {
                                    // this is means there is another key:value
                                }
                                else if (maybeEndChar.Equals('}'))
                                {
                                    // this is the end of the object
                                    return i;
                                }
                                else
                                {
                                    throw new Exception("Couldn't find end ',' or '}'. index:" + i + "  " + GetHelperString(jsonString, i, 100));
                                }
                            }
                            else
                            {
                                // its an error
                                throw new Exception("Could not determine the type at index:" + i + "  " + GetHelperString(jsonString, i, 100));
                            }
                        }
                    }
                    else if (startOfFind.Equals('}'))
                    {
                        // this is the end
                        i++;
                        return i;
                    }
                }
            }
            else if (jsonString[index].Equals('['))
            {
                isArray = true;
                array = new List<object>();

                int i = index + 1;
                int k = 0;
                while (true)
                {
                    // finding obj/str/num
                    i = StreamToNotWhiteSpace(jsonString, i);
                    char startOfObj = jsonString[i];
                    if (startOfObj.Equals('{') || startOfObj.Equals('['))
                    {
                        // its an object/array (basically another JSON object)
                        JSON j = new JSON();
                        i = j.ParseString(jsonString, i);
                        array.Add(j);
                        i = StreamToNotWhiteSpace(jsonString, i);
                        char maybeEndChar = jsonString[i];
                        i++;
                        if (maybeEndChar.Equals(','))
                        {
                            // this is means there is another key:value
                        }
                        else if (maybeEndChar.Equals(']'))
                        {
                            // this is the end of the object
                            return i;
                        }
                        else
                        {
                            throw new Exception("Couldn't find end ',' or ']'. index:" + i + "  " + GetHelperString(jsonString, i, 100));
                        }
                    }
                    else if (startOfObj.Equals('"'))
                    {
                        // its a string
                        i++;
                        k = StreamToChar(jsonString, '"', i, true, true);
                        string val = jsonString.Substring(i, k - i);
                        array.Add(val);
                        k++;
                        i = StreamToNotWhiteSpace(jsonString, k);
                        char maybeEndChar = jsonString[i];
                        i++;
                        if (maybeEndChar.Equals(','))
                        {
                            // this is means there is another key:value
                        }
                        else if (maybeEndChar.Equals(']'))
                        {
                            // this is the end of the object
                            return i;
                        }
                        else
                        {
                            throw new Exception("Couldn't find end ',' or ']'. index:" + i + "  " + GetHelperString(jsonString, i, 100));
                        }
                    }
                    else if (startOfObj.Equals(','))
                    {
                        // its null
                        array.Add(null);
                        i++;
                    }
                    else if (startOfObj.Equals(']'))
                    {
                        // its null and its the end of the obj
                        i++;
                        return i;
                    }
                    else
                    {
                        double d = 0;
                        if (Double.TryParse(startOfObj.ToString(), out d))
                        {
                            // its a number
                            k = StreamToChars(jsonString, i, new char[] { ' ', '\t', 'n', ',', ']' });
                            string valStr = jsonString.Substring(i, k - i);
                            double val = Double.Parse(valStr);
                            array.Add(val);
                            i = StreamToNotWhiteSpace(jsonString, k);
                            char maybeEndChar = jsonString[i];
                            i++;
                            if (maybeEndChar.Equals(','))
                            {
                                // this is means there is another key:value
                            }
                            else if (maybeEndChar.Equals(']'))
                            {
                                // this is the end of the object
                                return i;
                            }
                            else
                            {
                                throw new Exception("Couldn't find end ',' or ']'. index:" + i + "  " + GetHelperString(jsonString, i, 100));
                            }
                        }
                        else
                        {
                            // its an error
                            throw new Exception("Could not determine the type at index:" + i + "  " + GetHelperString(jsonString, i, 100));
                        }
                    }
                }
            }
            throw new Exception("Expected '{' or '[' to start string=" + jsonString.Substring(0, Math.Min(jsonString.Length, 25)));
        }

        private static int StreamToChar(string s, char c, int i, bool allowWhiteSpace, bool allowOtherChar)
        {
            if (i >= s.Length)
            {
                throw new Exception("Starting index was greater or equal to string length. index:" + i);
            }
            while (i < s.Length)
            {
                char n = s[i];
                if (n.Equals(c))
                {
                    return i;
                }
                else if (n.Equals(' ') || n.Equals('\n') || n.Equals('\t'))
                {
                    if (!allowWhiteSpace)
                    {
                        throw new Exception("No whitespace allowed. index:" + i);
                    }
                }
                else
                {
                    if (!allowOtherChar)
                    {
                        throw new Exception("Expecting a '" + c + "' but got a '" + n + "'. index:" + i);
                    }
                }
                i++;
            }
            throw new Exception("Char did not exist in string");
        }

        private static int StreamToNotWhiteSpace(string s, int i)
        {
            if (i >= s.Length)
            {
                throw new Exception("Starting index was greater or equal to string length. index:" + i);
            }
            while (i < s.Length)
            {
                char n = s[i];
                if (!n.Equals(' ') && !n.Equals('\n') && !n.Equals('\t') && !n.Equals('\r'))
                {
                    return i;
                }
                i++;
            }
            throw new Exception("Found the end of the string before a non-whitespace char");
        }

        private static int StreamToChars(string s, int i, char[] chars)
        {
            if (i >= s.Length)
            {
                throw new Exception("Starting index was greater or equal to string length. index:" + i);
            }
            while (i < s.Length)
            {
                char n = s[i];
                foreach (char c in chars)
                {
                    if (n.Equals(c))
                    {
                        return i;
                    }
                }
                i++;
            }
            throw new Exception("Found the end of the string before any of the given chars");
        }

        private static string GetHelperString(string s, int i, int len)
        {
            int endHelperStr = Math.Min(i + len, s.Length - 1) - i;
            string helperString = "--[" + s.Substring(Math.Max(i - (len / 2), 0), endHelperStr) + "]--";
            return helperString.Replace("\n", "").Replace("\t", "");
        }
        #endregion

        #region ToString
        public override string ToString()
        {
            return ToString(0);
        }

        public string ToString(int tab)
        {
            string t = "";
            for (int i = 0; i < tab; i++)
            {
                t += " ";
            }
            string n = "";
            if (tab > 0)
                n = "\n";
            string s = " ";
            return ToString(t, n, s, 0);
        }

        private string ToString(string t, string n, string s, int depth)
        {
            string tab = Tab(t, depth + 1);
            if (isArray)
            {
                if (array.Count == 0)
                {
                    return "[]";
                }
                string str = "[" + n;
                for (int i = 0; i < array.Count; i++)
                {
                    str += tab;
                    object o = array[i];
                    if (o == null)
                    {
                        str += "";
                    }
                    else if (o is JSON)
                    {
                        str += ((JSON)o).ToString(t, n, s, depth + 1);
                    }
                    else if (o is string)
                    {
                        str += "\"" + o.ToString() + "\"";
                    }
                    else
                    {
                        str += o.ToString();
                    }

                    if (i + 1 < array.Count)
                    {
                        str += "," + s + n;
                    }
                }
                str += n + Tab(t, depth) + "]";
                return str;
            }
            else
            {
                string[] keys = map.Keys.ToArray();
                if (keys.Length == 0)
                {
                    return "{}";
                }
                string str = "{" + n;
                for (int i = 0; i < keys.Length; i++)
                {
                    str += tab + "\"" + keys[i] + "\"" + s + ":" + s;
                    object o = map[keys[i]];
                    if (o == null)
                    {
                        str += "";
                    }
                    else if (o is JSON)
                    {
                        str += ((JSON)o).ToString(t, n, s, depth + 1);
                    }
                    else if (o is string)
                    {
                        str += "\"" + o.ToString() + "\"";
                    }
                    else
                    {
                        str += o.ToString();
                    }

                    if (i + 1 < keys.Length)
                    {
                        str += "," + s + n;
                    }
                }
                str += n + Tab(t, depth) + "}";
                return str;
            }
        }

        private string Tab(string t, int depth)
        {
            string tab = "";
            for (int i = 0; i < depth; i++)
            {
                tab += t;
            }
            return tab;
        }
        #endregion

        #region Bytable
        /// <summary>
        /// Takes the JSON object and puts it into a byte array with an integer length at the begining
        /// </summary>
        /// <returns>byte array with integer length at the begining</returns>
        public byte[] ToByteArray()
        {
            byte[] data = ByteUtils.stringToByteArray(this.ToString());
            byte[] len = ByteUtils.intToByteArray(data.Length);
            byte[] byteArray = ByteUtils.mergeByteArrays(len, data);
            return byteArray;
        }

        /// <summary>
        /// Parses a byte array assuming there is an integer length at the begining
        /// </summary>
        /// <param name="data">raw data</param>
        /// <param name="startingIndex">the starting index of the JSON</param>
        /// <returns>the ending index of the JSON</returns>
        public int ParseByteArray(byte[] data, int startingIndex = 0)
        {
            int len = ByteUtils.byteArrayToInt(data, startingIndex);
            startingIndex += 4;
            byte[] rawData = ByteUtils.subArray(data, startingIndex, startingIndex + len);
            string strData = ByteUtils.byteArrayToASCIIString(rawData);
            try
            {
                ParseString(strData, 0);
                return startingIndex + len;
            }
            catch
            {
                return startingIndex + len;
            }
        }
        #endregion


        #region Enumerator and Enumerable
        public List<object> GetJsonAsList()
        {
            if (isArray)
            {
                return array;
            }
            else
            {
                List<object> objs = new List<object>();
                foreach (string key in map.Keys)
                {
                    objs.Add(map[key]);
                }
                return objs;
            }
        }

        object IEnumerator<object>.Current
        {
            get { return enumeratorList[enumeratorPosition]; }
        }

        void IDisposable.Dispose()
        {
        }

        object IEnumerator.Current
        {
            get { return enumeratorList[enumeratorPosition]; }
        }

        bool IEnumerator.MoveNext()
        {
            enumeratorPosition++;
            return (enumeratorPosition < enumeratorList.Count);
        }

        void IEnumerator.Reset()
        {
            enumeratorPosition = 0;
        }

        IEnumerator<object> IEnumerable<object>.GetEnumerator()
        {
            enumeratorPosition = -1;
            enumeratorList = GetJsonAsList();
            return (IEnumerator<object>)this;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            enumeratorPosition = -1;
            enumeratorList = GetJsonAsList();
            return (IEnumerator)this;
        }
        #endregion

        #region Factory
        public static JSON EmptyObject()
        {
            return new JSON("{}");
        }

        public static JSON EmptyArray()
        {
            return new JSON("[]");
        }
        #endregion

    }
}
