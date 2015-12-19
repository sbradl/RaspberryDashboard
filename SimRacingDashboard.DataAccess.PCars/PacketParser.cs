using System;
using SimRacingDashboard.Entities;
using System.IO;

namespace SimRacingDashboard.DataAccess.PCars
{
    public class PacketParser
    {
        public CarState Parse(byte[] data)
        {
            using (var memoryStream = new MemoryStream(data))
            using (var binaryReader = new BinaryReader(memoryStream))
            {
                var buildVersionNumber = binaryReader.ReadUInt16();
                var sequencePacketType = binaryReader.ReadByte();

                return new Helper(binaryReader).ReadCarState();
            }
        }

        private class Helper
        {
            private BinaryReader reader;
            private CarState carState = new CarState
            {
                DateTime = DateTime.Now
            };

            public Helper(BinaryReader reader)
            {
                this.reader = reader;  
            }

            public CarState ReadCarState()
            {
                Goto(3);
                var gameAndSessionState = this.reader.ReadByte();

                // participant info
                Goto(4);
                var viewedParticipant = this.reader.ReadSByte();
                var numParticipants = this.reader.ReadSByte();

                // unfiltered input
                Goto(6);
                var unfilteredThrottle = this.reader.ReadByte();
                var unfilteredBrake = this.reader.ReadByte();
                var unfilteredSteering = this.reader.ReadSByte();
                var unfilteredClutch = this.reader.ReadByte();
                var raceStateFlags = this.reader.ReadByte();

                // event info
                Goto(11);
                var lapsInEvent = this.reader.ReadByte();

                // timings
                // Timings
                Goto(12);
                var bestLapTime = this.reader.ReadSingle();                 
                var lastLapTime = this.reader.ReadSingle();                 
                var currentTime = this.reader.ReadSingle();                 
                var splitTimeAhead = this.reader.ReadSingle();             
                var splitTimeBehind = this.reader.ReadSingle();             
                var splitTime = this.reader.ReadSingle();                   
                var eventTimeRemaining = this.reader.ReadSingle();          
                var personalFastestLapTime = this.reader.ReadSingle();      
                var worldFastestLapTime = this.reader.ReadSingle();         
                var currentSector1Time = this.reader.ReadSingle();         
                var currentSector2Time = this.reader.ReadSingle();          
                var currentSector3Time = this.reader.ReadSingle();          
                var fastestSector1Time = this.reader.ReadSingle();          
                var fastestSector2Time = this.reader.ReadSingle();          
                var fastestSector3Time = this.reader.ReadSingle();          
                var personalFastestSector1Time = this.reader.ReadSingle();  
                var personalFastestSector2Time = this.reader.ReadSingle();  
                var personalFastestSector3Time = this.reader.ReadSingle();
                var worldFastestSector1Time = this.reader.ReadSingle();    
                var worldFastestSector2Time = this.reader.ReadSingle();     
                var worldFastestSector3Time = this.reader.ReadSingle();

                // flags
                Goto(98);
                var highestFlag = this.reader.ReadByte();

                // pit info
                Goto(99);
                var pitModeSchedule = this.reader.ReadByte();
                
                ReadOilState();
                ReadWaterState();
                ReadFuelState();
               
                Goto(120);
                this.carState.Speed = this.reader.ReadSingle();

                ReadEngineState();
                ReadGearBoxState();
                ReadTireStates();
                ReadControlLightStates();

                return this.carState;
            }
            
            private void ReadOilState()
            {
                Goto(100);
                this.carState.Oil = new OilState
                {
                    Temperature = ReadTemperature(),
                    Pressure = ReadPressure()
                };
            }

            private void ReadWaterState()
            {
                Goto(104);
                this.carState.Water = new WaterState
                {
                    Temperature = ReadTemperature(),
                    Pressure = ReadPressure()
                };
            }

            private void ReadFuelState()
            {
                Goto(108);
                var pressure = ReadPressure();

                Goto(111);
                var capacityInLiters = this.reader.ReadByte();

                Goto(116);
                var level = this.reader.ReadSingle();

                this.carState.Fuel = new FuelState
                {
                    Pressure = pressure,
                    CapacityInLiters = capacityInLiters,
                    Level = level
                };
            }

            private void ReadEngineState()
            {
                Goto(124);
                var rpm = this.reader.ReadUInt16();
                var maxRpm = this.reader.ReadUInt16();

                Goto(129);
                var boostAmount = this.reader.ReadByte();

                Goto(448);
                var speed = this.reader.ReadSingle();
                var torque = this.reader.ReadSingle();

                this.carState.Engine = new EngineState
                {
                    Rpm = rpm,
                    MaxRpm = maxRpm,
                    BoostAmount = boostAmount,
                    Speed = speed,
                    Torque = torque,
                    // TODO: IsRunning
                };
            }

            private void ReadGearBoxState()
            {
                Goto(128);
                var gearAndNumGears = this.reader.ReadByte();
                var gear = ((byte)(gearAndNumGears << 4)) >> 4;
                var numGears = gearAndNumGears >> 4;

                this.carState.GearBox = new GearBoxState
                {
                    CurrentGear = (byte)gear,
                    NumGears = (byte)numGears
                };
            }

            private void ReadTireStates()
            {
                Goto(220);
                var flags1 = this.reader.ReadByte();
                var flags2 = this.reader.ReadByte();
                var flags3 = this.reader.ReadByte();
                var flags4 = this.reader.ReadByte();

                var terrain1 = this.reader.ReadByte();
                var terrain2 = this.reader.ReadByte();
                var terrain3 = this.reader.ReadByte();
                var terrain4 = this.reader.ReadByte();

                var tyreY1 = this.reader.ReadSingle();
                var tyreY2 = this.reader.ReadSingle();
                var tyreY3 = this.reader.ReadSingle();
                var tyreY4 = this.reader.ReadSingle();

                var tyreRps1 = this.reader.ReadSingle();
                var tyreRps2 = this.reader.ReadSingle();
                var tyreRps3 = this.reader.ReadSingle();
                var tyreRps4 = this.reader.ReadSingle();

                var tyreSlipSpeed1 = this.reader.ReadSingle();
                var tyreSlipSpeed2 = this.reader.ReadSingle();
                var tyreSlipSpeed3 = this.reader.ReadSingle();
                var tyreSlipSpeed4 = this.reader.ReadSingle();

                var temp1 = this.reader.ReadByte();
                var temp2 = this.reader.ReadByte();
                var temp3 = this.reader.ReadByte();
                var temp4 = this.reader.ReadByte();

                var grip1 = this.reader.ReadByte();
                var grip2 = this.reader.ReadByte();
                var grip3 = this.reader.ReadByte();
                var grip4 = this.reader.ReadByte();

                var heightAboveGround1 = this.reader.ReadSingle();
                var heightAboveGround2 = this.reader.ReadSingle();
                var heightAboveGround3 = this.reader.ReadSingle();
                var heightAboveGround4 = this.reader.ReadSingle();

                var lateralStiffness1 = this.reader.ReadSingle();
                var lateralStiffness2 = this.reader.ReadSingle();
                var lateralStiffness3 = this.reader.ReadSingle();
                var lateralStiffness4 = this.reader.ReadSingle();

                var wear1 = this.reader.ReadByte();
                var wear2 = this.reader.ReadByte();
                var wear3 = this.reader.ReadByte();
                var wear4 = this.reader.ReadByte();

                Goto(336);
                var treadTemp1 = this.reader.ReadUInt16();
                var treadTemp2 = this.reader.ReadUInt16();
                var treadTemp3 = this.reader.ReadUInt16();
                var treadTemp4 = this.reader.ReadUInt16();

                var layerTemp1 = this.reader.ReadUInt16();
                var layerTemp2 = this.reader.ReadUInt16();
                var layerTemp3 = this.reader.ReadUInt16();
                var layerTemp4 = this.reader.ReadUInt16();

                var carcassTemp1 = this.reader.ReadUInt16();
                var carcassTemp2 = this.reader.ReadUInt16();
                var carcassTemp3 = this.reader.ReadUInt16();
                var carcassTemp4 = this.reader.ReadUInt16();

                var rimTemp1 = this.reader.ReadUInt16();
                var rimTemp2 = this.reader.ReadUInt16();
                var rimTemp3 = this.reader.ReadUInt16();
                var rimTemp4 = this.reader.ReadUInt16();

                var internalAirTemp1 = this.reader.ReadUInt16();
                var internalAirTemp2 = this.reader.ReadUInt16();
                var internalAirTemp3 = this.reader.ReadUInt16();
                var internalAirTemp4 = this.reader.ReadUInt16();

                Goto(392);
                var rideHeight1 = this.reader.ReadSingle();
                var rideHeight2 = this.reader.ReadSingle();
                var rideHeight3 = this.reader.ReadSingle();
                var rideHeight4 = this.reader.ReadSingle();

                Goto(440);
                var pressure1 = this.reader.ReadUInt16();
                var pressure2 = this.reader.ReadUInt16();
                var pressure3 = this.reader.ReadUInt16();
                var pressure4 = this.reader.ReadUInt16();

                this.carState.Tires = new Tires
                {
                    FrontLeft = new Tire { RideHeight = rideHeight1 },
                    FrontRight = new Tire { RideHeight = rideHeight2 },
                    RearLeft = new Tire { RideHeight = rideHeight3 },
                    RearRight = new Tire { RideHeight = rideHeight4 },
                };
            }

            private void ReadControlLightStates()
            {
                Goto(110);
                CarFlags carFlags = (CarFlags)this.reader.ReadByte();

                this.carState.ControlLights = new ControlLightsState
                {
                    DrivingAssists = new DrivingAssistsState
                    {
                        ABS = carFlags.HasFlag(CarFlags.ABS),
                        StabilityControl = carFlags.HasFlag(CarFlags.StabilityControl),
                        TractionControl = carFlags.HasFlag(CarFlags.TractionControl)
                    },
                    EngineWarning = carFlags.HasFlag(CarFlags.EngineWarning),
                    Handbrake = carFlags.HasFlag(CarFlags.Handbrake),
                    LightsAreOn = carFlags.HasFlag(CarFlags.Headlight),
                    SpeedLimiter = carFlags.HasFlag(CarFlags.SpeedLimiter),
                };
            }

            private void Goto(long offset)
            {
                this.reader.BaseStream.Seek(offset, SeekOrigin.Begin);
            }

            private Temperature ReadTemperature()            
            {
                return new Temperature
                {
                    Value = (float) this.reader.ReadInt16(),
                    Unit = TemperatureUnit.Celsius
                };
            }

            private Pressure ReadPressure()
            {
                return new Pressure
                {
                    Value = this.reader.ReadUInt16(),
                    Unit = PressureUnit.KPa
                };
            }
        }
    }
}
