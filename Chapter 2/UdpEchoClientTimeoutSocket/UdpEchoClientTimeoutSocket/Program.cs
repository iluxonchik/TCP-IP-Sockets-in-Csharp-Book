using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace UdpEchoClientTimeoutSocket
{

    /*
        Since we want to access the timeout limit option, we either have
        to use the Socket class or inherit from UdpClient (since UdpClient.Client is 
        a protected property).
    */
    class UdpEchoClientTimeoutSocket
    {
        private const int TIMEOUT = 3000;
        private const int MAXTRIES = 5;

        static void Main(string[] args)
        {
            if ((args.Length < 2) || (args.Length > 3))
            {
                throw new ArgumentException("Parameters: <Server> <Word> [<Port>]");
            }

            string server = args[0];

            int servPort = (args.Length == 3) ? int.Parse(args[2]) : 7;

            // Create a socket that is connected to server on specified ports
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

            // Set the receive timeout for this socket
            socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReceiveTimeout, TIMEOUT);

            IPEndPoint remoteIPEndPoint = new IPEndPoint(Dns.GetHostEntry(server).AddressList[1], servPort);
            EndPoint remoteEndPoint = (EndPoint)remoteIPEndPoint;

            byte[] sendPacket = Encoding.ASCII.GetBytes(args[1]);
            byte[] rcvPacket = new byte[sendPacket.Length];

            int tries = 0; // packets may be lost, so we have to keep trying
            bool receivedResponse = false;

            do
            {
                socket.SendTo(sendPacket, remoteEndPoint);
                Console.WriteLine("Sent {0} bytes to server...", sendPacket.Length);

                try
                {
                    // Attemp echo reply receive
                    socket.ReceiveFrom(rcvPacket, ref remoteEndPoint);
                    receivedResponse = true;
                }
                catch (SocketException e)
                {
                    tries++;
                    if (e.ErrorCode == 10060)
                        Console.WriteLine("Timed out, {0} more tries left", MAXTRIES - tries);
                    else
                        Console.WriteLine(e.ErrorCode + ": " + e.Message);
                }
            } while ((!receivedResponse) && (tries < MAXTRIES));

            if (receivedResponse)
            {
                Console.WriteLine("Received {0} bytes from {1} : {2}", rcvPacket.Length, remoteEndPoint,
                    Encoding.ASCII.GetString(rcvPacket, 0, rcvPacket.Length));
            } else
            {
                Console.WriteLine("No response... :(");
            }

            socket.Close();
        }
    }
}
