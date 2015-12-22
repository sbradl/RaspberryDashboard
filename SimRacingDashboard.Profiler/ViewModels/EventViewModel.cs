using GalaSoft.MvvmLight;
using System;
using SimRacingDashboard.Entities;

namespace SimRacingDashboard.Profiler.ViewModels
{
    class EventViewModel : ViewModelBase
    {
        public int Laps { get; private set; }

        public TimeSpan Remaining { get; private set; }

        public string Flag { get; private set; }

        public float TrackLength { get; private set; }

        public void Add(CarState data)
        {
            this.Laps = data.Event.LapsInEvent;
            RaisePropertyChanged(() => this.Laps);

            this.Remaining = data.Event.TimeRemaining;
            RaisePropertyChanged(() => this.Remaining);

            this.Flag = data.Event.Flag.ToString();
            RaisePropertyChanged(() => this.Flag);

            this.TrackLength = data.Event.TrackLength;
            RaisePropertyChanged(() => TrackLength);
        }
    }
}
