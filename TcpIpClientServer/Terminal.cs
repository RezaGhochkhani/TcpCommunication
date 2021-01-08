using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace TcpIpClientServer
{
    public class Terminal
    {
        public event EventHandler Disconnected;
        public static int DataBufferSize { get; set; } = 4096;
        private TcpClient _socket;
        private NetworkStream _inputStream;
        private byte[] _receiveBuffer;

        public Terminal(TcpClient socket, IPacketHandler handler)
        {
            _receiveBuffer = new byte[DataBufferSize];
            _socket = socket;
            PacketHandler = handler;
        }

        public IPacketHandler PacketHandler { get; set; }

        public void Connect()
        {
            try
            {
                _socket.SendBufferSize = DataBufferSize;
                _socket.ReceiveBufferSize = DataBufferSize;
                _inputStream = _socket.GetStream();
                _inputStream.BeginRead(_receiveBuffer, 0, DataBufferSize, receiveCallback, null);
            }
            catch (Exception ex)
            {
                onDisconnected();
                Console.WriteLine($"Client disconnected-Line 39: {ex.Message}");
            }
        }

        private void receiveCallback(IAsyncResult ar)
        {
            try
            {
                var contentLength = _inputStream.EndRead(ar);
                if (contentLength <= 0)
                {
                    onDisconnected();
                    return;
                }
                var data = new byte[contentLength];
                Buffer.BlockCopy(_receiveBuffer, 0, data, 0, contentLength);
                PacketHandler.Display(data);
                var response = PacketHandler.GetResponse(data);
                _inputStream.BeginWrite(response, 0, response.Length, null, null);
                PacketHandler.Handle(data);

                // TODO: there might be a problem here! because untill the server processes the response it cannot accept messages
                _inputStream.BeginRead(_receiveBuffer, 0, DataBufferSize, receiveCallback, null);
            }
            catch (Exception ex)
            {
                onDisconnected();
                Console.WriteLine($"Client disconnected-Line 39: {ex.Message}");
            }
        }

        private void onDisconnected()
        {
            _socket.Client.Close();
            Console.WriteLine("Client disconnected");
            Disconnected?.Invoke(this, EventArgs.Empty);
        }
    }
}
