using System.Collections.Generic;

namespace Winger.Network.Utils
{
    /// <summary>
    /// A class to help in the constructing of byte arrays with useful data
    /// </summary>
    public class ByteStream
    {
        private List<byte> bs = new List<byte>();

        public ByteStream() { }
        public ByteStream(byte[] byteArray)
        {
            if (byteArray != null)
                for (int i = 0; i < byteArray.Length; i++)
                    bs.Add(byteArray[i]);
        }

        #region Write
        /// <summary>
        /// Writes the given data to the end of the current byte stream
        /// </summary>
        /// <param name="data">the data to write to the stream</param>
        /// <returns>the number of bytes added to the stream</returns>
        public int Write(byte data)
        {
            bs.Add(data);
            return 1;
        }
        /// <summary>
        /// Writes the given data to the end of the current byte stream
        /// </summary>
        /// <param name="data">the data to write to the stream</param>
        /// <returns>the number of bytes added to the stream</returns>
        public int Write(short data)
        {
            byte[] temp = ByteUtils.shortToByteArray(data);
            for (int i = 0; i < temp.Length; i++)
                bs.Add(temp[i]);
            return temp.Length;
        }
        /// <summary>
        /// Writes the given data to the end of the current byte stream
        /// </summary>
        /// <param name="data">the data to write to the stream</param>
        /// <returns>the number of bytes added to the stream</returns>
        public int Write(int data)
        {
            byte[] temp = ByteUtils.intToByteArray(data);
            for (int i = 0; i < temp.Length; i++)
                bs.Add(temp[i]);
            return temp.Length;
        }
        /// <summary>
        /// Writes the given data to the end of the current byte stream
        /// </summary>
        /// <param name="data">the data to write to the stream</param>
        /// <returns>the number of bytes added to the stream</returns>
        public int Write(long data)
        {
            byte[] temp = ByteUtils.longToByteArray(data);
            for (int i = 0; i < temp.Length; i++)
                bs.Add(temp[i]);
            return temp.Length;
        }
        /// <summary>
        /// Writes the given data to the end of the current byte stream
        /// </summary>
        /// <param name="data">the data to write to the stream</param>
        /// <returns>the number of bytes added to the stream</returns>
        public int Write(float data)
        {
            byte[] temp = ByteUtils.floatToByteArray(data);
            for (int i = 0; i < temp.Length; i++)
                bs.Add(temp[i]);
            return temp.Length;
        }
        /// <summary>
        /// Writes the given data to the end of the current byte stream
        /// </summary>
        /// <param name="data">the data to write to the stream</param>
        /// <returns>the number of bytes added to the stream</returns>
        public int Write(double data)
        {
            byte[] temp = ByteUtils.doubleToByteArray(data);
            for (int i = 0; i < temp.Length; i++)
                bs.Add(temp[i]);
            return temp.Length;
        }
        /// <summary>
        /// Writes the given vector data to the end of the current byte
        /// stream in the order of (x, y)
        /// </summary>
        /// <param name="x">the x value of the vector</param>
        /// <param name="y">the y value of the vector</param>
        /// <returns>the number of bytes added to the stream</returns>
        public int Write(float x, float y)
        {
            byte[] tempx = ByteUtils.floatToByteArray(x);
            for (int i = 0; i < tempx.Length; i++)
                bs.Add(tempx[i]);

            byte[] tempy = ByteUtils.floatToByteArray(y);
            for (int i = 0; i < tempy.Length; i++)
                bs.Add(tempy[i]);

            return (tempx.Length + tempy.Length);
        }
        /// <summary>
        /// Writes the given vector data to the end of the current byte
        /// stream in the order of (x, y, z)
        /// </summary>
        /// <param name="x">the x value of the vector</param>
        /// <param name="y">the y value of the vector</param>
        /// <param name="z">the z value of the vector</param>
        /// <returns>the number of bytes added to the stream</returns>
        public int Write(float x, float y, float z)
        {
            byte[] tempx = ByteUtils.floatToByteArray(x);
            for (int i = 0; i < tempx.Length; i++)
                bs.Add(tempx[i]);

            byte[] tempy = ByteUtils.floatToByteArray(y);
            for (int i = 0; i < tempy.Length; i++)
                bs.Add(tempy[i]);

            byte[] tempz = ByteUtils.floatToByteArray(z);
            for (int i = 0; i < tempz.Length; i++)
                bs.Add(tempz[i]);

            return (tempx.Length + tempy.Length + tempz.Length);
        }
        /// <summary>
        /// Writes the given vector data to the end of the current byte
        /// stream in the order of (x, y, z, w)
        /// </summary>
        /// <param name="x">the x value of the vector</param>
        /// <param name="y">the y value of the vector</param>
        /// <param name="z">the z value of the vector</param>
        /// <param name="w">the w value of the vector</param>
        /// <returns>the number of bytes added to the stream</returns>
        public int Write(float x, float y, float z, float w)
        {
            byte[] tempx = ByteUtils.floatToByteArray(x);
            for (int i = 0; i < tempx.Length; i++)
                bs.Add(tempx[i]);

            byte[] tempy = ByteUtils.floatToByteArray(y);
            for (int i = 0; i < tempy.Length; i++)
                bs.Add(tempy[i]);

            byte[] tempz = ByteUtils.floatToByteArray(z);
            for (int i = 0; i < tempz.Length; i++)
                bs.Add(tempz[i]);

            byte[] tempw = ByteUtils.floatToByteArray(w);
            for (int i = 0; i < tempw.Length; i++)
                bs.Add(tempw[i]);

            return (tempx.Length + tempy.Length + tempz.Length + tempw.Length);
        }
        /// <summary>
        /// Writes the given data to the end of the current byte stream
        /// </summary>
        /// <param name="data">the data to write to the stream</param>
        /// <returns>the number of bytes added to the stream</returns>
        public int Write(string data)
        {
            byte[] temp = ByteUtils.stringToByteArray(data);
            for (int i = 0; i < temp.Length; i++)
                bs.Add(temp[i]);
            return temp.Length;
        }
        /// <summary>
        /// Writes the given object that implements the Bytable interface
        /// to the end of the byte stream in the order determined by the
        /// toByteArray() method of the Bytable interface
        /// </summary>
        /// <param name="obj">the byteable object to be added</param>
        /// <returns>the number of bytes added to the stream</returns>
        public int Write(Byteable obj)
        {
            byte[] temp = obj.ToByteArray();
            for (int i = 0; i < temp.Length; i++)
                bs.Add(temp[i]);
            return temp.Length;
        }
        #endregion

        #region Read
        /// <summary>
        /// Reads the byte at the given index from the byte stream
        /// </summary>
        /// <param name="startIndex">the index of the byte to read</param>
        /// <returns>the byte at the given index</returns>
        public byte ReadByte(int startIndex)
        {
            return bs[startIndex];
        }
        /// <summary>
        /// Reads the first 2 bytes at the given index from the byte
        /// stream and converts it to a short
        /// </summary>
        /// <param name="startIndex">the starting index to read from</param>
        /// <returns>the 2 bytes at the index converted to a short</returns>
        public short ReadShort(int startIndex)
        {
            return ByteUtils.byteArrayToShort(extractByteArray(startIndex, 2));
        }
        /// <summary>
        /// Reads the first 4 bytes at the given index from the byte
        /// stream and converts it to an int
        /// </summary>
        /// <param name="startIndex">the starting index to read from</param>
        /// <returns>the 4 bytes at the index converted to an int</returns>
        public int ReadInt(int startIndex)
        {
            return ByteUtils.byteArrayToInt(extractByteArray(startIndex, 4));
        }
        /// <summary>
        /// Reads the first 8 bytes at the given index from the byte
        /// stream and converts it to a long
        /// </summary>
        /// <param name="startIndex">the starting index to read from</param>
        /// <returns>the 8 bytes at the index converted to a long</returns>
        public long ReadLong(int startIndex)
        {
            return ByteUtils.byteArrayToLong(extractByteArray(startIndex, 8));
        }
        /// <summary>
        /// Reads the first 4 bytes at the given index from the byte
        /// stream and converts it to a float
        /// </summary>
        /// <param name="startIndex">the starting index to read from</param>
        /// <returns>the 4 bytes at the index converted to a float</returns>
        public float ReadFloat(int startIndex)
        {
            return ByteUtils.byteArrayToFloat(extractByteArray(startIndex, 4));
        }
        /// <summary>
        /// Reads the first 8 bytes at the given index from the byte
        /// stream and converts it to a double
        /// </summary>
        /// <param name="startIndex">the starting index to read from</param>
        /// <returns>the 8 bytes at the index converted to a double</returns>
        public double ReadDouble(int startIndex)
        {
            return ByteUtils.byteArrayToDouble(extractByteArray(startIndex, 8));
        }
        /// <summary>
        /// Reads the first 8 bytes at the given index from the byte
        /// stream and converts it into 2 floats (x, y)
        /// </summary>
        /// <param name="startIndex">the starting index to read from</param>
        /// <returns>the float array containing the vector values</returns>
        public float[] ReadVector2(int startIndex)
        {
            byte[] data_x = extractByteArray(startIndex, 4);
            byte[] data_y = extractByteArray(startIndex + 4, 4);
            return new float[] { 
                ByteUtils.byteArrayToFloat(data_x), 
                ByteUtils.byteArrayToFloat(data_y) };
        }
        /// <summary>
        /// Reads the first 12 bytes at the given index from the byte
        /// stream and converts it into 3 floats (x, y, z)
        /// </summary>
        /// <param name="startIndex">the starting index to read from</param>
        /// <returns>the float array containing the vector values</returns>
        public float[] ReadVector3(int startIndex)
        {
            byte[] data_x = extractByteArray(startIndex, 4);
            byte[] data_y = extractByteArray(startIndex + 4, 4);
            byte[] data_z = extractByteArray(startIndex + 8, 4);
            return new float[] { 
                ByteUtils.byteArrayToFloat(data_x), 
                ByteUtils.byteArrayToFloat(data_y), 
                ByteUtils.byteArrayToFloat(data_z) };
        }
        /// <summary>
        /// Reads the first 16 bytes at the given index from the byte
        /// stream and converts it into 4 floats (x, y, z, w)
        /// </summary>
        /// <param name="startIndex">the starting index to read from</param>
        /// <returns>the float array containing the vector values</returns>
        public float[] ReadVector4(int startIndex)
        {
            byte[] data_x = extractByteArray(startIndex, 4);
            byte[] data_y = extractByteArray(startIndex + 4, 4);
            byte[] data_z = extractByteArray(startIndex + 8, 4);
            byte[] data_w = extractByteArray(startIndex + 12, 4);
            return new float[] { 
                ByteUtils.byteArrayToFloat(data_x), 
                ByteUtils.byteArrayToFloat(data_y), 
                ByteUtils.byteArrayToFloat(data_z),
                ByteUtils.byteArrayToFloat(data_w) };
        }
        /// <summary>
        /// Reads the given number of bytes from the byte stream 
        /// starting at the given index and converts it to an 
        /// ascii formatted string
        /// </summary>
        /// <param name="startIndex">the starting index to read from</param>
        /// <param name="length">the length of the string</param>
        /// <returns>the string read from the byte array</returns>
        public string ReadString(int startIndex, int length)
        {
            return ByteUtils.byteArrayToASCIIString(extractByteArray(startIndex, length));
        }
        /// <summary>
        /// The object implementing the bytable interface is essentially 
        /// populated using the parseByteArray() method from the bytable
        /// interface
        /// </summary>
        /// <param name="startIndex">the starting index to read from</param>
        /// <param name="obj">the byteable object to populate</param>
        /// <returns>the populated byteable object</returns>
        public Byteable ReadByteable(int startIndex, Byteable obj)
        {
            obj.ParseByteArray(bs.ToArray(), startIndex);
            return obj;
        }
        #endregion

        /// <summary>
        /// Returns a hex table representation of the byte stream
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return ByteUtils.byteArrayToHexTable(bs.ToArray());
        }

        /// <summary>
        /// Gets the current byte streams bytes
        /// </summary>
        /// <returns>the stream of bytes</returns>
        public byte[] GetBytes()
        {
            return bs.ToArray();
        }

        /// <summary>
        /// Helper method to break a byte array into smaller parts
        /// </summary>
        /// <param name="startIndex"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        private byte[] extractByteArray(int startIndex, int length)
        {
            byte[] ba = new byte[length];
            for (int i = 0; i < length; i++)
                ba[i] = bs[startIndex + i];
            return ba;
        }
    }
}
