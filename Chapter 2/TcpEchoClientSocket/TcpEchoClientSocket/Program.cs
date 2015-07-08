using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace TcpEchoClientSocket
{
    class TcoEchoClientSocket { 
        static void Main(string[] args)
        {
            if ((args.Length < 2) || (args.Length > 3))
            {
                throw new ArgumentException("Parameters: <Server> <Word> [<Port>]");
            }

            string server = args[0];

            byte[] byteBuffer = Encoding.ASCII.GetBytes(args[1]);

            int servPort = (args.Length == 3) ? int.Parse(args[2]) : 7;

            Socket sock = null;

            try
            {
                // Create a TCP socket instance
                sock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                IPEndPoint serverEndPoint = new IPEndPoint(Dns.GetHostEntry(server).AddressList[1], servPort);

                // Connect the socket to server on specified port
                sock.Connect(serverEndPoint);
                Console.WriteLine("Connecting to server... Sending echo string");

                // Send the encoded string to the server
                sock.Send(byteBuffer, byteBuffer.Length, SocketFlags.None);

                Console.WriteLine("Send {0} bytes to server", byteBuffer.Length);

                int totalBytesRcvd = 0;
                int bytesRcvd = 0;
                
                while (totalBytesRcvd < byteBuffer.Length)
                {
                    if ((bytesRcvd = sock.Receive(byteBuffer, totalBytesRcvd, byteBuffer.Length - totalBytesRcvd, SocketFlags.None)) == 0)
                    {
                        Console.WriteLine("Connection closed prematurely");
                        break;
                    }
                    totalBytesRcvd += bytesRcvd;
                }

                Console.WriteLine("Received {0} bytes from server: {1}", totalBytesRcvd, Encoding.ASCII.GetString(byteBuffer, 0, totalBytesRcvd));
            } catch(Exception e)
            {
                Console.WriteLine(e.Message);
            } finally
            {
                sock.Close();
            }
        }
    }
}
