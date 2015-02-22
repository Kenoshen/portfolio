using System;
using System.Threading;
using Winger.Network.Utils;
using Winger.Utils;

namespace Winger.Network.LAN
{
    /// <summary>
    /// A wrapper around the DatagramSocket that is used for receiving connectionless UDP messages while running in a thread
    /// </summary>
    public class UDPReceiver
    {
        private bool running = false;
        private int maxMessageSize = 1024;
        private int port = 8080;
        private HandleUDPMessage messageDel;
        private DatagramSocket socket = new DatagramSocket();

        private int errorCounter = 0;

        /// <summary>
        /// The delegate called when a message is received and parsed into a DatagramPacket
        /// </summary>
        public HandleUDPMessage MessageDelegate
        {
            set { messageDel = value; }
            get { return messageDel; }
        }

        /// <summary>
        /// The maximum byte array size of a message to be received. Defaults to 256
        /// </summary>
        public int MaxMessageSize
        {
            set { maxMessageSize = value; }
            get { return maxMessageSize; }
        }

        /// <summary>
        /// The port to listen to. Defaults to 8080
        /// </summary>
        public int Port
        {
            set { port = value; }
            get { return port; }
        }

        /// <summary>
        /// Calls the Run() method inside of a thread and returns the thread object
        /// </summary>
        /// <returns>the thread object running the Run() method</returns>
        public Thread StartInThread()
        {
            Thread caller = new Thread(new ThreadStart(this.Run));
            caller.Start();
            return caller;
        }

        /// <summary>
        /// Calling this method will block while listening for messages on the given port 
        /// and on receiving a message will call the UDPHandleMessage delegate
        /// </summary>
        public void Run()
        {
            running = true;
            while (running)
            {
                DatagramPacket p = new DatagramPacket();
                try
                {
                    p = socket.Listen(maxMessageSize, port);
                    ThreadUtils.StartUpInThread(HandleMessage, p);
                }
                catch (Exception e)
                {
                    // ignore exceptions
                    errorCounter++;
                    Console.WriteLine("\n\nError #" + errorCounter + "  " + e);
                }
            }
        }

        /// <summary>
        /// Marks the Run() method to stop looping and finish after the next received message
        /// </summary>
        public void Stop()
        {
            if (running == true)
                running = false;
        }

        /// <summary>
        /// Handle the message in a different thread so the receiver can keep receiving
        /// </summary>
        /// <param name="dgPacketObj">the datagram packet</param>
        private void HandleMessage(object dgPacketObj)
        {
            if (MessageDelegate != null)
            {
                MessageDelegate((DatagramPacket)dgPacketObj);
            }
        }


        /// <summary>
        /// A static version of the UDPReceiver.StartInThread() method that takes all the required arguments
        /// in one method call.
        /// </summary>
        /// <param name="maxMessageSize">the maximum byte array size of a message to be received</param>
        /// <param name="port">the port to listen to</param>
        /// <param name="messageDel">the delegate called when a message is received and parsed into a DatagramPacket</param>
        /// <returns>the thread that the server is now running in</returns>
        public static Thread StartInThread(int maxMessageSize, int port, HandleUDPMessage messageDel)
        {
            UDPReceiver server = new UDPReceiver();
            server.MaxMessageSize = maxMessageSize;
            server.Port = port;
            server.MessageDelegate = messageDel;
            Thread caller = new Thread(new ThreadStart(server.Run));
            caller.Start();
            return caller;
        }

        /// <summary>
        /// A static version of the UDPReceiver.Run() method that takes all the required arguments 
        /// in one method call.  Does not handle starting a thread however.
        /// </summary>
        /// <param name="maxMessageSize">the maximum byte array size of a message to be received</param>
        /// <param name="port">the port to listen to</param>
        /// <param name="messageDel">the delegate called when a message is received and parsed into a DatagramPacket</param>
        public static void Run(int maxMessageSize, int port, HandleUDPMessage messageDel)
        {
            UDPReceiver server = new UDPReceiver();
            server.MaxMessageSize = maxMessageSize;
            server.Port = port;
            server.MessageDelegate = messageDel;
            server.Run();
        }
    }
}
