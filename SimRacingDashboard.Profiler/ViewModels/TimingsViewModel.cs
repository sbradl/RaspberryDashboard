using GalaSoft.MvvmLight;
using System;
using SimRacingDashboard.Entities;

namespace SimRacingDashboard.Profiler.ViewModels
{
    class TimingsViewModel : ViewModelBase
    {
        public TimeSpan CurrentLapTime { get; private set; }
        public TimeSpan SplitTime { get; private set; }
        public TimeSpan SplitTimeDifference { get; private set; }

        internal void Add(CarState data)
        {
            this.CurrentLapTime = data.Timings.CurrentLapTime;
            this.SplitTime = data.Timings.SplitTime;
            this.SplitTimeDifference = data.Timings.SplitTimeDifference;

            RaisePropertyChanged(() => CurrentLapTime);
            RaisePropertyChanged(() => SplitTime);
            RaisePropertyChanged(() => SplitTimeDifference);
        }
    }
}
