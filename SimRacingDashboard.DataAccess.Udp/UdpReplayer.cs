using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;

namespace SimRacingDashboard.DataAccess.Udp
{
    public class UdpReplayer : IDisposable
    {
        private IList<byte[]> datasets = new List<byte[]>();
        private int current = 0;
        private bool isReading = true;

        public UdpReplayer()
        {
            using (var stream = new FileStream("telemetry.udp", FileMode.Open, FileAccess.Read, FileShare.Read))
            using(var reader = new BinaryReader(stream))
            {
                while (reader.BaseStream.Position < reader.BaseStream.Length)
                {
                    byte[] buffer = new byte[1367];
                    reader.Read(buffer, 0, buffer.Length);
                
                    this.datasets.Add(buffer);
                }
            }

           this.datasets = this.datasets.Skip(2000).ToList();
        }

        public byte[] ReadData()
        {
            if(this.current == this.datasets.Count - 1)
            {
                while(this.isReading)
                {
                    Thread.Sleep(100);
                }

                return this.datasets[this.current];
            }

            //Thread.Sleep(1);

            return this.datasets[this.current++];
        }

        public void Dispose()
        {
            this.isReading = false;
        }
    }
}
