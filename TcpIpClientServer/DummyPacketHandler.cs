using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TcpIpClientServer
{
    public class DummyPacketHandler : IPacketHandler
    {
        public void Display(byte[] data)
        {
            Console.WriteLine(Encoding.ASCII.GetString(data));
        }

        public byte[] GetResponse(byte[] data)
        {
            return Encoding.ASCII.GetBytes("OK");
        }

        public void Handle(byte[] data)
        {

        }
    }
}
