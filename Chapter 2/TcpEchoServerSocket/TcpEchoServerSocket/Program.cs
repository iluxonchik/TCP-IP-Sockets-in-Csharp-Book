using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace TcpEchoServerSocket
{
    class TcpEchoServerSocket
    {
        private const int BUFFSIZE = 32; // receive bufer size
        private const int BACKLOG = 5; // outstanding connection queue max size

        static void Main(string[] args)
        {
            if (args.Length > 1)
            {
                throw new ArgumentException("Parameters: [<Port>]");
            }

            int servPort = (args.Length == 1) ? int.Parse(args[0]) : 7;

            Socket server = null;

            try
            {
                // Create a socket to accept client connections
                server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                // Associate the socket with a local address and port number
                server.Bind(new IPEndPoint(IPAddress.Any, servPort));
                server.Listen(BACKLOG);
            }
            catch (SocketException e) 
            {
                Console.WriteLine(e.ErrorCode + ": " + e.Message);
                Environment.Exit(e.ErrorCode);
            }

            byte[] rcvBuffr = new byte[BUFFSIZE];
            int bytesRcvd;

            for (; ;)
            {
                Socket client = null;

                try
                {
                    client = server.Accept(); // get client connection
                    Console.WriteLine("Handling client at " + client.RemoteEndPoint + " - ");

                    int totalBytesEchoed = 0;
                    while ((bytesRcvd = client.Receive(rcvBuffr,0, rcvBuffr.Length, SocketFlags.None)) > 0)
                    {
                        client.Send(rcvBuffr, 0, bytesRcvd, SocketFlags.None);
                        totalBytesEchoed += bytesRcvd;
                    }
                    Console.WriteLine("echoed {0} bytes", totalBytesEchoed);
                    client.Close();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    client.Close();
                }
            }
         
        }
    }
}

