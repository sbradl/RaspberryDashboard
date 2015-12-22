using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimRacingDashboard.Entities;

namespace SimRacingDashboard.Profiler.ViewModels
{
    class ControlLightsViewModel : ViewModelBase
    {
        public bool Abs { get; private set; }
        public bool TractionControl { get; private set; }
        public bool StabilityControl { get; private set; }
        public bool Lights { get; private set; }
        public bool EngineActive { get; private set; }
        public bool EngineWarning { get; private set; }
        public bool SpeedLimiter { get; private set; }

        internal void Add(CarState data)
        {
            this.Abs = data.ControlLights.DrivingAssists.ABS;
            this.TractionControl = data.ControlLights.DrivingAssists.TractionControl;
            this.StabilityControl = data.ControlLights.DrivingAssists.StabilityControl;
            this.Lights = data.ControlLights.LightsAreOn;
            this.EngineActive = data.Engine.IsRunning;
            this.EngineWarning = data.ControlLights.EngineWarning;
            this.SpeedLimiter = data.ControlLights.SpeedLimiter;

            RaisePropertyChanged(() => Abs);
            RaisePropertyChanged(() => TractionControl);
            RaisePropertyChanged(() => StabilityControl);
            RaisePropertyChanged(() => Lights);
            RaisePropertyChanged(() => EngineActive);
            RaisePropertyChanged(() => EngineWarning);
            RaisePropertyChanged(() => SpeedLimiter);
        }
    }
}
