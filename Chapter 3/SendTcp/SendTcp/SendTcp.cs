using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ItemQuote
{
    class SendTcp
    {
        static void Main(string[] args)
        {
            if (args.Length != 2)
            {
                throw new ArgumentException("Parameters: <Destination> <Port>");
            }
            string server = args[0];
            int servPort = Int32.Parse(args[1]);

            // Create a socket that's coonnected to the server on the specified port
            TcpClient client = new TcpClient(server, servPort);
            NetworkStream netStream = client.GetStream();

            ItemQuote quote = new ItemQuote(1234567890987654L, "Super Widgets", 1000, 12999, true, false);

            // Send text-encoded quote
            ItemQuoteEncoderText coder = new ItemQuoteEncoderText();
            byte[] codedQuote = coder.encode(quote);
            Console.WriteLine("Sending Text-Encoded Quote (" + codedQuote.Length + " bytes):");
            Console.WriteLine(quote);

            netStream.Write(codedQuote, 0, codedQuote.Length);

            // Receive binary-encoded quote
            ItemQuoteDecoder decoder = new ItemQuoteDecoderBin();
            ItemQuote receivedQuote = decoder.decode(client.GetStream());
            Console.WriteLine("Received Binary-Encode Quote:");
            Console.WriteLine(receivedQuote);

            netStream.Close();
            client.Close();
        }
    }
}
