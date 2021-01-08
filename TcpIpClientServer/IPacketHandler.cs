using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TcpIpClientServer
{
    public interface IPacketHandler
    {
        void Handle(byte[] data);
        void Display(byte[] data);
        byte[] GetResponse(byte[] data);
    }
}
