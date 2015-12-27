using SimRacingDashboard.Entities;
using System;
using System.IO;

namespace SimRacingDashboard.DataAccess.DirtRally
{
    class PacketParser
    {
        public TelemetryDataSet? Parse(byte[] data)
        {
            using (var memoryStream = new MemoryStream(data))
            using (var binaryReader = new BinaryReader(memoryStream))
            {
                var helper = new Helper(binaryReader);

                return helper.ReadTelemetry();
            }
        }

        private class Helper
        {
            private BinaryReader reader;

            private TelemetryDataSet telemetry = new TelemetryDataSet();

            public Helper(BinaryReader reader)
            {
                this.reader = reader;
            }

            public TelemetryDataSet? ReadTelemetry()
            {
                var eventTime = this.reader.ReadSingle();
                var currentLapTime = this.reader.ReadSingle();

                if(currentLapTime == 0)
                {
                    return null;
                }

                var distanceOnCurrentLap = this.reader.ReadSingle();
                var totalDistance = this.reader.ReadSingle(); // fraction of completion (0 to 1)
                
                var position = new[]
                {
                    this.reader.ReadSingle(),
                    this.reader.ReadSingle(),
                    this.reader.ReadSingle()
                };

                var speed = this.reader.ReadSingle();

                var velocity = new[]
                {
                    this.reader.ReadSingle(),
                    this.reader.ReadSingle(),
                    this.reader.ReadSingle()
                };

                var pitchYawRoll = new[]
                {
                    this.reader.ReadSingle(),
                    this.reader.ReadSingle(),
                    this.reader.ReadSingle()
                };

                var suspensionPosition = new[]
                {
                    this.reader.ReadSingle(),
                    this.reader.ReadSingle(),
                    this.reader.ReadSingle(),
                    this.reader.ReadSingle()
                };

                var suspensionVelocity = new[]
                {
                    this.reader.ReadSingle(),
                    this.reader.ReadSingle(),
                    this.reader.ReadSingle(),
                    this.reader.ReadSingle()
                };

                var wheelVelocity = new[]
                {
                    this.reader.ReadSingle(),
                    this.reader.ReadSingle(),
                    this.reader.ReadSingle(),
                    this.reader.ReadSingle()
                };

                // unknown (no idea where)
                for (int i = 0; i < 3; i++)
                {
                    this.reader.ReadSingle();
                }

                var throttle = this.reader.ReadSingle();
                var steer = this.reader.ReadSingle();
                var brake = this.reader.ReadSingle();
                var clutch = this.reader.ReadSingle();

                var gear = this.reader.ReadSingle();

                var gforce = new[]
                {
                    this.reader.ReadSingle(),
                    this.reader.ReadSingle()
                };

                var currentLap = this.reader.ReadSingle();

                var engineSpeed = this.reader.ReadSingle();

                // unknown (38 - 50)
                for (int i = 0; i < 13; i++)
                {
                    this.reader.ReadSingle();
                }

                var brakeTemps = new[]
                {
                    this.reader.ReadSingle(),
                    this.reader.ReadSingle(),
                    this.reader.ReadSingle(),
                    this.reader.ReadSingle()
                };

                // unknown (55 - 59)
                for (int i = 0; i < 5; i++)
                {
                    this.reader.ReadSingle();
                }

                var totalLaps = this.reader.ReadSingle();
                var trackLength = this.reader.ReadSingle();

                // 62
                var unknown = this.reader.ReadSingle();

                var maxRpm = this.reader.ReadSingle();


                //////////////////////////
                this.telemetry.Event = new EventState
                {
                    LapsInEvent = (byte)totalLaps,
                    TrackLength = trackLength
                };

                this.telemetry.Timings = new TimingInfo
                {
                    CurrentLapTime = ToTimeSpan(currentLapTime)
                };

                this.telemetry.Speed = speed;

                this.telemetry.Engine = new EngineState
                {
                    IsRunning = true,
                    Rpm = (ushort)(engineSpeed * 10),
                    MaxRpm = (ushort)(maxRpm * 10)
                };

                this.telemetry.GearBox = new GearBoxState
                {
                    CurrentGear = (byte)gear
                };

                this.telemetry.Position = position;

                this.telemetry.CurrentTrackDistance = distanceOnCurrentLap;

                //this.telemetry.LapsCompleted = 
                this.telemetry.CurrentLap = (byte)currentLap;

                // TODO
                this.telemetry.Session = new SessionInfo
                {

                };

                this.telemetry.Oil = new OilState
                {

                };

                this.telemetry.Water = new WaterState
                {

                };

                this.telemetry.Fuel = new FuelState
                {

                };

                this.telemetry.Tires = new Tires
                {
                    FrontLeft = new Tire(),
                    FrontRight = new Tire(),
                    RearLeft = new Tire(),
                    RearRight = new Tire()
                };

                this.telemetry.ControlLights = new ControlLightsState
                {
                    DrivingAssists = new DrivingAssistsState
                    {

                    }
                };

                return this.telemetry;
            }

            private TimeSpan ToTimeSpan(float time)
            {
                return TimeSpan.FromSeconds(time);
            }
        }
    }
}
