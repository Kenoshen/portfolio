using System;
using System.Text;

namespace Winger.Network.Utils
{
    /// <summary>
    /// Helper class for converting byte data in to different forms
    /// </summary>
    public static class ByteUtils
    {
        private static char[] hexArray = new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'a', 'b', 'c', 'd', 'e', 'f' };

        #region Helpers
        /// <summary>
        /// Concatenates two or more byte arrays
        /// </summary>
        /// <param name="arrays">the byte arrays to concatenate one after the other</param>
        /// <returns>the merged byte array</returns>
        public static byte[] mergeByteArrays(params object[] arrays)
        {
            if (arrays == null)
                return null;
            else if (arrays.Length == 0)
                return new byte[0];

            int currentLength = 0;
            int totalLength = 0;

            for (int i = 0; i < arrays.Length; i++)
            {
                if (!(arrays[i] is byte[]))
                    throw new ArgumentException("The object at index " + i +
                        " was type '" + arrays[i].GetType() + "' instead of type byte[].");
                else
                    totalLength += ((byte[])arrays[i]).Length;
            }

            if (arrays.Length == 1)
                return (byte[])arrays[0];

            byte[] merged = new byte[totalLength];

            for (int i = 0; i < arrays.Length; i++)
            {
                for (int k = 0; k < ((byte[])arrays[i]).Length; k++)
                    merged[currentLength + k] = ((byte[])arrays[i])[k];
                currentLength += ((byte[])arrays[i]).Length;
            }

            return merged;
        }

        /// <summary>
        /// Reverses the given byte array so that the endianness of the 
        /// data is the opposite of what it currently is (Big Endian or Little Endian)
        /// </summary>
        /// <param name="data">the byte array to invert</param>
        /// <returns>the inverted byte array</returns>
        public static byte[] reverseEndianness(byte[] data)
        {
            byte[] temp = new byte[data.Length];
            for (int i = 0; i < data.Length; i++)
                temp[i] = data[data.Length - (i + 1)];

            return temp;
        }

        /// <summary>
        /// Gets a sub array of a given array
        /// </summary>
        /// <param name="data">the original array</param>
        /// <param name="startIndex">the index to start the sub array (includes the byte at that index)</param>
        /// <param name="endIndex">the index to end the sub array (does not include the byte at that index)</param>
        /// <returns></returns>
        public static byte[] subArray(byte[] data, int startIndex, int endIndex)
        {
            byte[] sub = new byte[endIndex - startIndex];
            for (int i = startIndex; i < endIndex; i++)
                sub[i - startIndex] = data[i];
            return sub;
        }
        #endregion

        #region Display methods
        /// <summary>
        /// Converts a byte array into a table of hex numbers.  It makes for easier reading of byte array data.
        /// </summary>
        /// <param name="data">the byte array to convert</param>
        /// <param name="startIndex">the index to start at (includes the byte at that index)</param>
        /// <param name="endIndex">the index to end at (does not include the byte at that index)</param>
        /// <returns>the string of the converted byte array</returns>
        public static String byteArrayToHexTable(byte[] data, int startIndex = 0, int endIndex = -1)
        {
            if (data == null)
                return "null";
            try
            {
                StringBuilder sb = new StringBuilder();
                String hexString = byteArrayToHexString(data, startIndex, endIndex);
                hexString = hexString.Substring(2);
                if (hexString.Length % 2 != 0)
                    hexString = "0" + hexString;

                for (int i = 0; i < hexString.Length; i++)
                {
                    if (i / 2 % 16 == 0)
                    {
                        sb.Append("\n");
                    }
                    sb.Append(" ");
                    sb.Append(hexString.Substring(i, 2));
                    i++;
                }
                return sb.ToString();
            }
            catch (Exception e)
            {
                return e.ToString();
            }
        }

        /// <summary>
        /// Converts a byte array to a string of hex numbers.  It makes for slightly easier reading of smaller byte arrays.
        /// </summary>
        /// <param name="data">the byte array to convert</param>
        /// <param name="startIndex">the index to start at (includes the byte at that index)</param>
        /// <param name="endIndex">the index to end at (does not include the byte at that index)</param>
        /// <returns>the string of the converted byte array</returns>
        public static String byteArrayToHexString(byte[] data, int startIndex = 0, int endIndex = -1)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("0x");
            if (endIndex == -1)
                endIndex = data.Length;
            for (int i = startIndex; i < endIndex; i++)
            {
                int bD = (int)data[i];
                int first = bD / 16;
                int second = bD - (first * 16);
                sb.Append(hexArray[first]);
                sb.Append(hexArray[second]);
            }
            return sb.ToString();
        }

        /// <summary>
        /// Converts a byte array into a string of ascii characters.  If the byte array contained a word or sentence then 
        /// using this method would decode the byte array for you.
        /// </summary>
        /// <param name="data">the byte array to convert</param>
        /// <param name="startIndex">the index to start at (includes the byte at that index)</param>
        /// <param name="endIndex">the index to end at (does not include the byte at that index)</param>
        /// <returns>the string of the converted byte array</returns>
        public static String byteArrayToASCIIString(byte[] data, int startIndex = 0, int endIndex = -1)
        {
            return convertHexToASCII(byteArrayToHexString(data, startIndex, endIndex));
        }

        /// <summary>
        /// Converts a given string in a hex format to ascii format.
        /// </summary>
        /// <param name="inputHex">the hex to format</param>
        /// <returns>the formatted hex </returns>
        public static String convertHexToASCII(String inputHex)
        {
            StringBuilder sb = new StringBuilder();
            StringBuilder temp = new StringBuilder();

            String hex = inputHex;
            if (inputHex.Length >= 2)
                if (inputHex.Substring(0, 2).Equals("0x"))
                    hex = inputHex.Substring(2);

            //49204c6f7665204a617661 split into two characters 49, 20, 4c...
            for (int i = 0; i < hex.Length - 1; i += 2)
            {
                //grab the hex in pairs
                String output = hex.Substring(i, 2);
                //convert hex to decimal
                int deci = Int32.Parse(output, System.Globalization.NumberStyles.HexNumber);
                //convert the decimal to character
                sb.Append((char)deci);

                temp.Append(deci);
            }

            return sb.ToString();
        }
        #endregion

        #region To byte array methods
        /// <summary>
        /// Converts a string into an array of bytes
        /// </summary>
        /// <param name="data">the string to convert</param>
        /// <returns>the byte data of the string (byteDataLength == inputStringLength)</returns>
        public static byte[] stringToByteArray(String data)
        {
            return System.Text.Encoding.ASCII.GetBytes(data);
        }

        /// <summary>
        /// Puts a byte into an array of length 1
        /// </summary>
        /// <param name="data">the byte</param>
        /// <returns>the array of the single byte</returns>
        public static byte[] byteToByteArray(byte data)
        {
            return new byte[] { data };
        }

        /// <summary>
        /// Converts a short into an array of bytes
        /// </summary>
        /// <param name="data">the short to convert</param>
        /// <returns>the byte data of the short (2 bytes)</returns>
        public static byte[] shortToByteArray(short data)
        {
            byte[] shortByte = new byte[2];
            shortByte[0] = (byte)(data >> 8);
            shortByte[1] = (byte)(data >> 0);
            return shortByte;
        }

        /// <summary>
        /// Converts an int into an array of bytes
        /// </summary>
        /// <param name="data">the int to convert</param>
        /// <returns>the byte data of the int (4 bytes)</returns>
        public static byte[] intToByteArray(int data)
        {
            byte[] intByte = new byte[4];
            intByte[0] = (byte)(data >> 24);
            intByte[1] = (byte)(data >> 16);
            intByte[2] = (byte)(data >> 8);
            intByte[3] = (byte)(data >> 0);
            return intByte;
        }

        /// <summary>
        /// Converts a long into an array of bytes
        /// </summary>
        /// <param name="data">the long to convert</param>
        /// <returns>the byte data of the long (8 bytes)</returns>
        public static byte[] longToByteArray(long data)
        {
            byte[] longByte = new byte[8];
            longByte[0] = (byte)(data >> 56);
            longByte[1] = (byte)(data >> 48);
            longByte[2] = (byte)(data >> 40);
            longByte[3] = (byte)(data >> 32);
            longByte[4] = (byte)(data >> 24);
            longByte[5] = (byte)(data >> 16);
            longByte[6] = (byte)(data >> 8);
            longByte[7] = (byte)(data >> 0);
            return longByte;
        }

        /// <summary>
        /// Converts a float into an array of bytes
        /// </summary>
        /// <param name="data">the float to convert</param>
        /// <returns>the byte data of the float (4 bytes)</returns>
        public static byte[] floatToByteArray(float data)
        {
            return reverseEndianness(BitConverter.GetBytes(data));
        }

        /// <summary>
        /// Converts a double into an array of bytes
        /// </summary>
        /// <param name="data">the double to convert</param>
        /// <returns>the byte data of the double (8 bytes)</returns>
        public static byte[] doubleToByteArray(double data)
        {
            return reverseEndianness(BitConverter.GetBytes(data));
        }
        #endregion

        #region From byte array methods
        /// <summary>
        /// Extracts the first byte in a byte array
        /// </summary>
        /// <param name="data">the byte array to extract from</param>
        /// <param name="startingIndex">the index to start at within the byte array</param>
        /// <returns>the first byte in a given byte array</returns>
        public static byte byteArrayToByte(byte[] data, int startingIndex = 0)
        {
            return data[startingIndex];
        }

        /// <summary>
        /// Converts the first 2 bytes into a short
        /// </summary>
        /// <param name="data">byte array with at least 2 bytes</param>
        /// <param name="startingIndex">the index to start at within the byte array</param>
        /// <returns>short using first 2 bytes in the given byte array</returns>
        public static short byteArrayToShort(byte[] data, int startingIndex = 0)
        {
            return BitConverter.ToInt16(reverseEndianness(subArray(data, startingIndex, startingIndex + 2)), 0);
        }

        /// <summary>
        /// Converts the first 4 bytes into an int
        /// </summary>
        /// <param name="data">byte array with at least 4 bytes</param>
        /// <param name="startingIndex">the index to start at within the byte array</param>
        /// <returns>int using first 4 bytes in the given byte array</returns>
        public static int byteArrayToInt(byte[] data, int startingIndex = 0)
        {
            return BitConverter.ToInt32(reverseEndianness(subArray(data, startingIndex, startingIndex + 4)), 0);
        }

        /// <summary>
        /// Converts the first 8 bytes into a long
        /// </summary>
        /// <param name="data">byte array with at least 8 bytes</param>
        /// <param name="startingIndex">the index to start at within the byte array</param>
        /// <returns>long using first 8 bytes in the given byte array</returns>
        public static long byteArrayToLong(byte[] data, int startingIndex = 0)
        {
            return BitConverter.ToInt64(reverseEndianness(subArray(data, startingIndex, startingIndex + 8)), 0);
        }

        /// <summary>
        /// Converts the first 4 bytes into a float
        /// </summary>
        /// <param name="data">byte array with at least 4 bytes</param>
        /// <param name="startingIndex">the index to start at within the byte array</param>
        /// <returns>float using first 4 bytes in the given byte array</returns>
        public static float byteArrayToFloat(byte[] data, int startingIndex = 0)
        {
            return BitConverter.ToSingle(reverseEndianness(subArray(data, startingIndex, startingIndex + 4)), 0);
        }

        /// <summary>
        /// Converts the first 8 bytes into a double
        /// </summary>
        /// <param name="data">byte array with at least 8 bytes</param>
        /// <param name="startingIndex">the index to start at within the byte array</param>
        /// <returns>double using first 8 bytes in the given byte array</returns>
        public static double byteArrayToDouble(byte[] data, int startingIndex = 0)
        {
            return BitConverter.ToDouble(reverseEndianness(subArray(data, startingIndex, startingIndex + 8)), 0);
        }
        #endregion
    }
}
