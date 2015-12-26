using GalaSoft.MvvmLight;
using SimRacingDashboard.Entities;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace SimRacingDashboard.Profiler.ViewModels
{
    class LapViewModel : ViewModelBase
    {
        public LapViewModel()
        {
            this.Datasets = new ObservableCollection<TelemetryDataSet>();
        }

        public int Lap
        {
            get; private set;
        }

        public TimeSpan LapTime
        {
            get; private set;
        }

        public bool Pitted
        {
            get
            {
                return this.Datasets.Any(dataset => dataset.ControlLights.SpeedLimiter);
            }
        }

        public ObservableCollection<TelemetryDataSet> Datasets
        {
            get; private set;
        }

        public float LatestTrackDistance
        {
            get
            {
                return this.Datasets.Last().CurrentTrackDistance;
            }
        }

        public bool IsFinished
        {
            get; private set;
        }

        public void Finish()
        {
            this.IsFinished = true;
            RaisePropertyChanged(() => IsFinished);

            RaisePropertyChanged(() => LatestTrackDistance);
            RaisePropertyChanged(() => Lap);
            RaisePropertyChanged(() => LapTime);
            RaisePropertyChanged(() => Pitted);
        }

        public void Add(TelemetryDataSet state)
        {
            this.Datasets.Add(state);
            this.Lap = state.CurrentLap;
            this.LapTime = state.Timings.CurrentLapTime;

            //RaisePropertyChanged(() => LatestTrackDistance);
            //RaisePropertyChanged(() => Lap);
            //RaisePropertyChanged(() => LapTime);
            //RaisePropertyChanged(() => Pitted);
        }
    }
}
