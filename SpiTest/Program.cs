using SimRacingDashboard.Output.Dashboard;
using SimRacingDashboard.Output.Dashboard.Gpio;
using SimRacingDashboard.Output.Dashboard.SPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SpiTest
{
    class Program
    {
        static void Main(string[] args)
        {
            byte[][] temperatures = new byte[][]
            {
                new byte[] { 0, 4, 2 },
                new byte[] { 0, 7, 7 },
                new byte[] { 1, 0, 1 },
                new byte[] { 1, 2, 9 }
            };

            using (var display = new Max7219(22, 27, 17))
            {
                //for (int j = 0; j < 1; j++)
                //{
                //    for (byte i = 0; i < temperatures.Length; i++)
                //    {
                //        var dataset = temperatures[i];

                //        Console.WriteLine("{0}{1}{2}", dataset[0], dataset[1], dataset[2]);
                        
                //        Display(display, dataset[0], dataset[1], dataset[2]);
                //        Thread.Sleep(1000);
                        
                //    }
                //}
                for(byte h = 0; h <= 9; h++)
                {
                    for(byte t = 0; t <= 9; t++)
                    {
                        for(byte o = 0; o <= 9; o++)
                        {
                            Display(display, h, t, o);
                            Thread.Sleep(10);
                        }
                    }
                }
            }

            //using (var display = new SevenSegmentDisplay())
            //{
            //    display.Write(7);
            //    Thread.Sleep(5000);
            //}

            //using (var pin = new OutputPin(5))
            //{
            //    pin.Enable();
            //    Thread.Sleep(2000);
            //}
        }

        static void Display(Max7219 display, byte hundred, byte ten, byte one)
        {
            display.Display(0x01, one);
            display.Display(0x02, ten);
            display.Display(0x03, hundred);
        }
    }
}
