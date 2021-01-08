using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Unity;

namespace TcpIpClientServer
{
    class Program
    {
        static void Main(string[] args)
        {
            var server = new Server(IPAddress.Any, 5023);
            server.Start();
            Console.WriteLine("Press any key to terminate the server");
            Console.ReadLine();
        }
    }
}
