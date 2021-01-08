using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity;

namespace TcpIpClientServer
{
    public static class IoC
    {
        private static UnityContainer _container;

        static IoC()
        {
            _container = new UnityContainer();
            _container.RegisterType<IPacketHandler, DummyPacketHandler>();
        }

        public static UnityContainer Container 
        {
            get => _container;
        }
    }
}
