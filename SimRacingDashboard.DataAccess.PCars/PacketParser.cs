using System;
using SimRacingDashboard.Entities;
using System.IO;

namespace SimRacingDashboard.DataAccess.PCars
{
    public class PacketParser
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

            private CarFlags carFlags;

            private TelemetryDataSet telemetry = new TelemetryDataSet
            {
                DateTime = DateTime.Now
            };

            public Helper(BinaryReader reader)
            {
                this.reader = reader;  
            }

            public GameState GameState { get; private set; }
            public PCarsSessionState SessionState { get; private set; }

            public TelemetryDataSet? ReadTelemetry()
            {
                this.telemetry.Version = this.reader.ReadUInt16();

                if(this.telemetry.Version == 0)
                {
                    return null;
                }
                
                int packetTypeAndSequence = this.reader.ReadByte();
                int packetType = packetTypeAndSequence & 3;
                int frameSequence = packetTypeAndSequence >> 2;

                if (packetType != 0)
                {
                    return null;
                }

                //Goto(3);
                var gameAndSessionState = this.reader.ReadByte();
                byte gameState;
                byte sessionState;
                SplitByte(gameAndSessionState, out gameState, out sessionState);
                this.GameState = (GameState)gameState;

                if(this.GameState != GameState.Playing)
                {
                    return null;
                }

                this.SessionState = (PCarsSessionState)sessionState;

                //// participant info
                //Goto(4);
                var viewedParticipant = this.reader.ReadSByte();
                var numParticipants = this.reader.ReadSByte();

                this.telemetry.Session = new SessionInfo
                {
                    State = ToGeneralSessionState(this.SessionState),
                    Participants = numParticipants
                };

                //// unfiltered input
                //Goto(6);
                //var unfilteredThrottle = this.reader.ReadByte();
                //var unfilteredBrake = this.reader.ReadByte();
                //var unfilteredSteering = this.reader.ReadSByte();
                //var unfilteredClutch = this.reader.ReadByte();
                //var raceStateFlags = this.reader.ReadByte();

                //// event info
                Goto(11);
                var lapsInEvent = this.reader.ReadByte();

                //// Timings
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

                this.telemetry.Timings = new TimingInfo
                {
                    BestLapTime = ToTimeSpan(bestLapTime),
                    LastLapTime = ToTimeSpan(lastLapTime),
                    CurrentLapTime = ToTimeSpan(currentTime),
                    SplitTime = ToTimeSpan(splitTime),
                    SplitTimeDifference = ToTimeSpan(splitTimeAhead), // TODO: decide which one to use
                    CurrentSectorTimes = new[]
                    {
                        ToTimeSpan(currentSector1Time),
                        ToTimeSpan(currentSector2Time),
                        ToTimeSpan(currentSector3Time)
                    },
                    FastestSectorTimes = new[]
                    {
                        ToTimeSpan(fastestSector1Time),
                        ToTimeSpan(fastestSector2Time),
                        ToTimeSpan(fastestSector3Time)
                    }
                };

                // flags
                Goto(98);
                var highestFlagState = this.reader.ReadByte();
                byte highestFlagColor;
                byte flagReason;
                SplitByte(highestFlagState, out highestFlagColor, out flagReason);

                Goto(1360);
                var trackLength = this.reader.ReadSingle();

                this.telemetry.Event = new EventState
                {
                    LapsInEvent = lapsInEvent,
                    TimeRemaining = ToTimeSpan(eventTimeRemaining),
                    Flag = (Flag)highestFlagColor,
                    TrackLength = trackLength
                };

                //// pit info
                //Goto(99);
                //var pitModeSchedule = this.reader.ReadByte();

                Goto(110);
                this.carFlags = (CarFlags)this.reader.ReadByte();

                ReadOilState();
                ReadWaterState();
                ReadFuelState();

                Goto(120);
                this.telemetry.Speed = this.reader.ReadSingle();

                ReadEngineState();
                ReadGearBoxState();
                ReadTireStates();
                ReadControlLightStates();

                //// participant info
                Goto(464);
                this.telemetry.Position = new[]
                {
                    this.reader.ReadInt16(),
                    this.reader.ReadInt16(),
                    this.reader.ReadInt16()
                };

                this.telemetry.CurrentTrackDistance = this.reader.ReadUInt16();

                Goto(473);
                this.telemetry.LapsCompleted = this.reader.ReadByte();
                this.telemetry.CurrentLap = this.reader.ReadByte();

                return this.telemetry;
            }
            
            private void ReadOilState()
            {
                Goto(100);
                this.telemetry.Oil = new OilState
                {
                    Temperature = ReadTemperature(),
                    Pressure = ReadPressure()
                };
            }

            private void ReadWaterState()
            {
                Goto(104);
                this.telemetry.Water = new WaterState
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

                this.telemetry.Fuel = new FuelState
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

                this.telemetry.Engine = new EngineState
                {
                    Rpm = rpm,
                    MaxRpm = maxRpm,
                    BoostAmount = boostAmount,
                    Speed = speed,
                    Torque = torque,
                    IsRunning = this.carFlags.HasFlag(CarFlags.EngineActive)
                };
            }

            private void ReadGearBoxState()
            {
                Goto(128);
                var gearAndNumGears = this.reader.ReadByte();
                byte gear;
                byte numGears;
                SplitByte(gearAndNumGears, out gear, out numGears);

                this.telemetry.GearBox = new GearBoxState
                {
                    CurrentGear = gear,
                    NumGears = numGears
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

                var grip1 = Normalize(this.reader.ReadByte());
                var grip2 = Normalize(this.reader.ReadByte());
                var grip3 = Normalize(this.reader.ReadByte());
                var grip4 = Normalize(this.reader.ReadByte());

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

                this.telemetry.Tires = new Tires
                {
                    FrontLeft = new Tire
                    {
                        RideHeightInMeter = rideHeight1,
                        HeightAboveGround = heightAboveGround1,
                        SlipSpeed = tyreSlipSpeed1,
                        GripLevel = grip1,
                        WearLevel = wear1,
                        Pressure = new Pressure { Value = pressure1, Unit = PressureUnit.KPa }
                    },
                    FrontRight = new Tire
                    {
                        RideHeightInMeter = rideHeight2,
                        HeightAboveGround = heightAboveGround2,
                        SlipSpeed = tyreSlipSpeed2,
                        GripLevel = grip2,
                        WearLevel = wear2,
                        Pressure = new Pressure { Value = pressure2, Unit = PressureUnit.KPa }
                    },
                    RearLeft = new Tire
                    {
                        RideHeightInMeter = rideHeight3,
                        HeightAboveGround = heightAboveGround3,
                        SlipSpeed = tyreSlipSpeed3,
                        GripLevel = grip3,
                        WearLevel = wear3,
                        Pressure = new Pressure { Value = pressure3, Unit = PressureUnit.KPa }
                    },
                    RearRight = new Tire
                    {
                        RideHeightInMeter = rideHeight4,
                        HeightAboveGround = heightAboveGround4,
                        SlipSpeed = tyreSlipSpeed4,
                        GripLevel = grip4,
                        WearLevel = wear4,
                        Pressure = new Pressure { Value = pressure4, Unit = PressureUnit.KPa }
                    },
                };
            }

            private void ReadControlLightStates()
            {
                this.telemetry.ControlLights = new ControlLightsState
                {
                    DrivingAssists = new DrivingAssistsState
                    {
                        ABS = !carFlags.HasFlag(CarFlags.ABS),
                        StabilityControl = !carFlags.HasFlag(CarFlags.StabilityControl),
                        TractionControl = !carFlags.HasFlag(CarFlags.TractionControl)
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

            private void SplitByte(byte input, out byte low, out byte high)
            {
                low = (byte)(((byte)(input << 4)) >> 4);
                high = (byte)(input >> 4);
            }

            private SessionState ToGeneralSessionState(PCarsSessionState sessionState)
            {
                switch(sessionState)
                {
                    case PCarsSessionState.Practice:
                        return Entities.SessionState.Practice;

                    case PCarsSessionState.Test:
                        return Entities.SessionState.Warmup;

                    case PCarsSessionState.Qualifying:
                        return Entities.SessionState.Qualify;

                    case PCarsSessionState.FormationLap:
                    case PCarsSessionState.Race:
                        return Entities.SessionState.Race;

                    case PCarsSessionState.TimeAttack:
                        return Entities.SessionState.TimeAttack;
                }

                return Entities.SessionState.Race;
            }

            private TimeSpan ToTimeSpan(float time)
            {
                //if(time > TimeSpan.MaxValue.TotalSeconds)
                //{
                //    return TimeSpan.MaxValue;
                //}

                //if(time < TimeSpan.MinValue.TotalSeconds)
                //{
                //    return TimeSpan.MinValue;
                //}

                return TimeSpan.FromSeconds(time);
            }

            private float Normalize(byte value)
            {
                float fValue = value;

                return fValue / byte.MaxValue;
            }
        }
    }
}
