
namespace Winger.Network.Utils
{
    /// <summary>
    /// An interface used for sending classes over connectionless UDP using the 
    /// UDPSender class, see the TestBytable class for an example
    /// </summary>
    public interface Byteable
    {
        /// <summary>
        /// Should be used to build byte arrays using the members of the class, 
        /// see the TestByteable for an example
        /// </summary>
        /// <returns>a byte array custom built for each class</returns>
        byte[] ToByteArray();

        /// <summary>
        /// Should be used to parse a given byte array into the members of the class,
        /// see the TestBytable for an example
        /// </summary>
        /// <param name="data">the byte array data to be parsed</param>
        /// <param name="startingIndex">the starting index within the data</param>
        int ParseByteArray(byte[] data, int startingIndex = 0);
    }
}
