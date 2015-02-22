namespace Winger.Network.LAN
{
    /// <summary>
    /// Used with the LANServer to notify the subscriber of the 
    /// delegate that a message was received and needs to be handled
    /// </summary>
    /// <param name="data">the data just received</param>
    public delegate void HandleTCPMessage(byte[] data);
}
