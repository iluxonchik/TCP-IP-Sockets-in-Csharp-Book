using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using ItemQuote;

namespace RecvTcp
{
    class RecvTcp
    {
        static void Main(string[] args)
        {
            if (args.Length != 1)
            {
                throw new ArgumentException("Parameters: <Port>");
            }

            int port = int.Parse(args[0]);

            // Create TCP listener to listen to TCP connections
            TcpListener listener = new TcpListener(IPAddress.Any, port);
            listener.Start();

            TcpClient client = listener.AcceptTcpClient();

            // Receive text-encoded quote
            ItemQuoteDecoder decoder = new ItemQuoteDecoderText();
            ItemQuote.ItemQuote quote = decoder.decode(client.GetStream());
            Console.WriteLine("Received Text-Encoded Quote:");
            Console.WriteLine(quote);

            // Repeat quote with binary-encoding adding 10 cents to the price
            ItemQuoteEncoder encoder = new ItemQuoteEncoderBin();
            quote.UnitPrice += 10;
            Console.WriteLine("Sending (binary)...");
            byte[] bytesToSend = encoder.encode(quote);
            client.GetStream().Write(bytesToSend, 0, bytesToSend.Length);

            client.Close();
            listener.Stop();
        }
    }
}
