using System.Text;
using System;
using System.IO;
using System.Net.Sockets;


namespace TCPEchoClient
{
    class TcpEchoClient
    {
        static void Main(string[] args)
        {
            if (args.Length < 2 || args.Length > 3)
            {
                throw new ArgumentException("Parameters: <Server> <Word> [<Port>]");
            }

            string server = args[0];

            // Convert the word to echo to a byte array
            byte[] byteBuffer = Encoding.ASCII.GetBytes(args[1]);

            // Use the supplied port name or default to 7
            int servPort = (args.Length == 3) ? Int32.Parse(args[2]) : 7;

            TcpClient client = null;
            NetworkStream netStream = null;

            try
            {
                client = new TcpClient(server, servPort);

                Console.WriteLine("Connected to server... sending echo string");

                // Get the network stream to which the data will be written and read from
                netStream = client.GetStream();

                // Send the encoded string to the server
                netStream.Write(byteBuffer, 0, byteBuffer.Length);

                int totlalBytesRcvd = 0; // total bytes recieved so far
                int bytesRcvd = 0; // total bytes recieved in the last read

                // Recieve the same string back from the server
                while (totlalBytesRcvd < byteBuffer.Length)
                {
                    if ((bytesRcvd = netStream.Read(byteBuffer, totlalBytesRcvd, 
                        byteBuffer.Length - totlalBytesRcvd)) == 0)
                    {
                        Console.WriteLine("Connection closed prematurely");
                        break;
                    }

                    totlalBytesRcvd += bytesRcvd;
                }
                Console.WriteLine("Received {0} bytes from server: {1}", totlalBytesRcvd,
                    Encoding.ASCII.GetString(byteBuffer, 0, totlalBytesRcvd));
            }
            catch (Exception e) { Console.WriteLine(e.Message); }
            finally
            {
                netStream.Close();
                client.Close();
            }
             
        }
    }
}
