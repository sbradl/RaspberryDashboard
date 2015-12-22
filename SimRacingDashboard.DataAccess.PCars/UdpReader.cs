using System;
using System.Net.Sockets;
using System.Net;

namespace SimRacingDashboard.DataAccess.PCars
{
    public class UdpReader : IDisposable
    {
        private UdpClient udpClient = new UdpClient();
        private UdpClient broadcaster = new UdpClient();

        private IPEndPoint broadcastreceiverAddress;
        private IPEndPoint broadcastAddress;

        public UdpReader(int port)
        {
            this.broadcastreceiverAddress = new IPEndPoint(IPAddress.Any, port);
            this.broadcastAddress = new IPEndPoint(IPAddress.Broadcast, port);

            this.udpClient.Client.Bind(this.broadcastreceiverAddress);
        }

        public byte[] ReadData()
        {
            return udpClient.Receive(ref broadcastreceiverAddress);
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

