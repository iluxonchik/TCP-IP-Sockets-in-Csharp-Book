using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace UDPEchoServer
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length > 1)
            {
                throw new ArgumentException("Parameters: <Port>");
            }

            int servPort = (args.Length == 1) ? int.Parse(args[0]) : 7;

            UdpClient client = null;

            try
            {
                // Create an instance of UdpClient on the port to listen on
                client = new UdpClient(servPort);
            }
            catch (SocketException e)
            {
                Console.WriteLine(e.ErrorCode + ": " + e.Message);
                Environment.Exit(e.ErrorCode);
            }

            // Create and IPEndpoint instance that will be passed as a referrence
            // to the .Receive() call and be populated with the remote client info
            IPEndPoint remoteIPEndPoint = new IPEndPoint(IPAddress.Any, 0);
            Console.WriteLine("remoteIPEndpoint before for: {0} : {1}", remoteIPEndPoint.Address, remoteIPEndPoint.Port);

            for (; ;)
            {
                try
                {
                    // Receive byte array with echo datagram packet contents
                    byte[] byteBuffer = client.Receive(ref remoteIPEndPoint);
                    Console.WriteLine("Handling client at " + remoteIPEndPoint + " -- ");
                    Console.WriteLine("remoteIPEndpoint in for: {0} : {1}", remoteIPEndPoint.Address, remoteIPEndPoint.Port);

                    // Send an echo back to the client
                    client.Send(byteBuffer, byteBuffer.Length, remoteIPEndPoint);
                    Console.WriteLine("echoed {0} bytes", byteBuffer.Length);
                }
                catch (SocketException e)
                {

                    Console.WriteLine("{0} : {1}", e.ErrorCode, e.Message);
                }
            }
        }

    }
}
