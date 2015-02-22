using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Winger.Utils;

namespace Winger.Network.LAN
{
    /// <summary>
    /// A wrapper around TcpListener that listens for Tcp connections
    /// </summary>
    public class TCPReceiver
    {
        private bool running = false;
        private int maxMessageSize = 256;
        private int port = 8080;
        private TcpListener listener;
        private HandleTCPMessage messageDel;


        /// <summary>
        /// The delegate called when a message is received
        /// </summary>
        public HandleTCPMessage MessageDelegate
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
            listener = new TcpListener(IPAddress.Any, Port);
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
            listener.Start();
            running = true;
            while (running)
            {
                Socket client = listener.AcceptSocket();
                ThreadUtils.StartUpInThread(HandleMessage, client);
            }
            listener.Stop();
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
        /// Handle the message in a separate thread so the listener can continue listening
        /// </summary>
        /// <param name="clientObj">the TcpClient object</param>
        private void HandleMessage(object clientObj)
        {
            Console.WriteLine("Handle Message: ");
            Socket s = clientObj as Socket;
            byte[] buffer = new byte[MaxMessageSize];
            // NOLONGERTODO: breaking here?
            int length = s.Receive(buffer);

            byte[] data = new byte[length];
            for (int i = 0; i < length; i++)
                data[i] = buffer[i];

            MessageDelegate(data);
            Console.WriteLine("Message handled!");
        }
    }
}
