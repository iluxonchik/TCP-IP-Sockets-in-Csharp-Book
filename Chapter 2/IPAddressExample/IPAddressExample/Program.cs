using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace IPAddressExample
{
    class Program
    {
        static void Main(string[] args)
        {
            // Get and print local host info
            try
            {
                Console.WriteLine("Local Host:");
                string localHostName = Dns.GetHostName();
                Console.WriteLine("\tHost Name: " + localHostName);

                IPAddressExample.PrintHostInfo(localHostName);
            } catch (Exception)
            {
                Console.WriteLine("Unable to resolve local host");
            }

            // Get and print hosts given on command line
            foreach(String arg in args)
            {
                Console.WriteLine(arg + ":");
                IPAddressExample.PrintHostInfo(arg);
            }
        }
    }
}
