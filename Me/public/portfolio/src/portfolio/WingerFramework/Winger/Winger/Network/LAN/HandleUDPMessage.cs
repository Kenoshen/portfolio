using Winger.Network.Utils;
namespace Winger.Network.LAN
{
    /// <summary>
    /// Used with the LANServer to notify the subscriber of the 
    /// delegate that a message was received and needs to be handled
    /// </summary>
    /// <param name="packet">the packet just received</param>
    public delegate void HandleUDPMessage(DatagramPacket packet);
}
