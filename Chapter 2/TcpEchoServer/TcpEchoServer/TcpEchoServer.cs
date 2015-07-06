using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace TcpEchoServer
{
    class TcpEchoServer
    {
        private const int BUFFERSIZE = 32; // Size of recieve buffer
        static void Main(string[] args)
        {
            if (args.Length > 1)
            {
                throw new ArgumentException("Parameters: [<Port>]");
            }

            int servPort = (args.Length == 1) ? Int32.Parse(args[0]) : 7;

            TcpListener listener = null;

            try
            {
                // Create a TCPListener to accept client connection
                listener = new TcpListener(IPAddress.Any, servPort);
                listener.Start();
            } catch (SocketException se)
            {
                Console.WriteLine(se.ErrorCode + ": " + se.Message);
                Environment.Exit(se.ErrorCode);
            }

            /* 
                The sole purpose of TcpListener is to supply a new, connected
                TcpClient instance for each new TcpConnection.
            */

            byte[] rcvBuffer = new byte[BUFFERSIZE];
            int bytesRcvd;

            for(; ;)
            {
                TcpClient client = null;
                NetworkStream netSteram = null;

                try
                {
                    client = listener.AcceptTcpClient(); // Get client connection
                    netSteram = client.GetStream();
                    Console.WriteLine("Handling Client -" );

                    // Recieve until client closes connection, indicated by 0 return value
                    int totalBytesEchoed = 0;
                    while ((bytesRcvd = netSteram.Read(rcvBuffer, 0, rcvBuffer.Length)) > 0)
                    {
                        Console.WriteLine("Recieved from client {0}: {1}", client.GetHashCode(), Encoding.ASCII.GetString(rcvBuffer, 0, bytesRcvd));
                        netSteram.Write(rcvBuffer, 0, bytesRcvd);
                        totalBytesEchoed += bytesRcvd;
                    }
                    Console.WriteLine("echoed {0} bytes.", totalBytesEchoed);

                    // Close the stream and socket. We are done with this client!
                    client.Close();
                    netSteram.Close();
                } catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    client.Close();
                    netSteram.Close();
                }
            }
        }
    }
}
