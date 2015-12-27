using System;
using System.Net.Sockets;
using System.Net;

namespace SimRacingDashboard.DataAccess.Udp
{
    public class UdpReader : IDisposable
    {
        private UdpClient udpClient = new UdpClient();
        private UdpClient broadcaster = new UdpClient();

        private IPEndPoint broadcastReceiverAddress;
        private IPEndPoint broadcastAddress;

        public UdpReader(int port, IPAddress ip)
        {
            this.broadcastReceiverAddress = new IPEndPoint(ip, port);
            this.broadcastAddress = new IPEndPoint(IPAddress.Broadcast, port);

            this.udpClient.Client.Bind(this.broadcastReceiverAddress);
        }

        public byte[] ReadData()
        {
            return udpClient.Receive(ref broadcastReceiverAddress);
        }

        public void Dispose()
        {
            byte[] bytes = new byte[] { };
            this.broadcaster.Send(bytes, bytes.Length, this.broadcastAddress);
            this.broadcaster.Close();
            
            this.udpClient.Close();
        }
    }
}

