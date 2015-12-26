using System;
using SimRacingDashboard.Entities;
using GalaSoft.MvvmLight;

namespace SimRacingDashboard.Profiler.ViewModels
{
    class EngineViewModel : ViewModelBase
    {
        public ushort Rpm { get; private set; }
        public ushort MaxRpm { get; private set; }
        public float RpmLevel { get; private set; }

        internal void Add(TelemetryDataSet data)
        {
            this.Rpm = Math.Min(data.Engine.Rpm, data.Engine.MaxRpm);
            this.MaxRpm = data.Engine.MaxRpm;
            this.RpmLevel = Math.Min(data.Engine.RpmLevel, 1.0f);

            RaisePropertyChanged(() => Rpm);
            RaisePropertyChanged(() => MaxRpm);
            RaisePropertyChanged(() => RpmLevel);
        }
    }
}
