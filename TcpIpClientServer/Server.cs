using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using Unity;

namespace TcpIpClientServer
{
    public class Server
    {
        private TcpListener _listener;
        private List<Terminal> _terminals;

        public Server(string ip, int port) : this(IPAddress.Parse(ip), port) { }

        public Server(IPAddress ipAddress, int port)
        {
            _listener = new TcpListener(ipAddress, port);
            _terminals = new List<Terminal>();
        }

        public void Start()
        {
            try
            {
                _listener.Start();
                _listener.BeginAcceptTcpClient(clientConnectCallback, null);
                Console.WriteLine($"Successfully started server");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }

        private void clientConnectCallback(IAsyncResult ar)
        {
            _listener.BeginAcceptTcpClient(clientConnectCallback, null);
            var client = new Terminal(_listener.EndAcceptTcpClient(ar), IoC.Container.Resolve<IPacketHandler>());
            lock (_terminals)
            {
                _terminals.Add(client);
            }
            client.Connect();
            client.Disconnected += Client_Disconnected;
        }

        private void Client_Disconnected(object sender, EventArgs e)
        {
            var terminal = sender as Terminal;
            if (terminal == null) return;
            lock (_terminals)
            {
                if (_terminals.Contains(terminal))
                    _terminals.Remove(terminal);
            }
        }
    }
}
