using System;
using System.Net.Sockets;
using System.Net;

namespace RaspberryDashboard.DataAccess.PCars
{
    public class UdpReader : IDisposable
    {
        private UdpClient udpClient = new UdpClient();
        private IPEndPoint broadcastAddress;

        public UdpReader(int port)
        {
            this.broadcastAddress = new IPEndPoint(IPAddress.Any, port);
            this.udpClient.Client.Bind(this.broadcastAddress);
        }

        public byte[] ReadData()
        {
            return udpClient.Receive(ref broadcastAddress);
        }

        public void Dispose()
        {
            this.udpClient.Close();
        }
    }
}

