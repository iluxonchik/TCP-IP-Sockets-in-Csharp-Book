using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace UDPEchoClient
{
    class Program
    {
        /*
             A UDP socket, on the other hand, is not required to establish a connection before communication, 
             and each datagram can be sent and received from a different destination. The Connect() method of 
             UdpClient does allow the speciﬁcation of the remote address and port, but its use is optional. 
             Unlike the TCP version of Connect(), the UDP version merely sets the default destination and does 
             not actually cause any connection-setup messages to be transmitted through the network. 
        */
        static void Main(string[] args)
        {
            if ((args.Length < 2) || (args.Length > 3))
            {
                throw new System.ArgumentException("Parameters: <Server> <Word> [<Port>]");
            }

            string server = args[0]; // Server name or IP address

            // Use supplied port or default to 7
            int servPort = (args.Length == 3) ? Int32.Parse(args[2]) : 7;

            // Convert string to an array of bytes
            byte[] sendPacket = Encoding.ASCII.GetBytes(args[1]);

            // Create UDPClient instance

            UdpClient client = new UdpClient();

            try
            {
                // Send the echo string to the specified host and port
                client.Send(sendPacket, sendPacket.Length, server, servPort);

                Console.WriteLine("Sent {0} bytes to the server", sendPacket.Length);

                // This IPEndPoint instance will be populated with the remote sendre's
                // endpoint information after the Receive() call
                IPEndPoint remoteIPEndpoint = new IPEndPoint(IPAddress.Any, 0);

                // Attemp echo reply receive
                byte[] rcvPacket = client.Receive(ref remoteIPEndpoint);

                Console.WriteLine("Received {0} bytes from {1}: {2}",
                    rcvPacket.Length, remoteIPEndpoint, Encoding.ASCII.GetString(rcvPacket));
            }
            catch (SocketException e) 
            {

                Console.WriteLine(e.ErrorCode + ": " + e.Message);
            }

            client.Close();
        }
    }
}
