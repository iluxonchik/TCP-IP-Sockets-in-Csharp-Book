using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace IPAddressExample
{
    public static class IPAddressExample
    {
        public static void PrintHostInfo(String host)
        {
            try
            {
                IPHostEntry hostInfo;

                // Attemp to resolve DNS for a given host or address
                hostInfo = Dns.GetHostEntry(host);

                // Display the primary host name
                Console.WriteLine("\tCanonical Name: " + hostInfo.HostName);

                // Display the list of IP addresses for this host
                Console.Write("\tIP Addresses: ");
                foreach(IPAddress ipaddr in hostInfo.AddressList)
                {
                    Console.Write(ipaddr.ToString() + " ");
                }
                Console.WriteLine();

                // Display list of alias names for this host
                Console.Write("\tAlisases: ");

                foreach(String alias in hostInfo.Aliases)
                {
                    Console.Write(alias +  " ");
                }
                Console.WriteLine();
            } catch (Exception)
            {
                Console.WriteLine("\t Unable to resolve host: " + host);
            }
        }
    }
}
