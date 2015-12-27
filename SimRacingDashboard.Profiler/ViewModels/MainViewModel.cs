using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using SimRacingDashboard.Entities;
using SimRacingDashboard.Profiler.ViewModels.Engine;
using SimRacingDashboard.Profiler.ViewModels.Tires;
using System.Collections.ObjectModel;

namespace SimRacingDashboard.Profiler.ViewModels
{
    class MainViewModel : ViewModelBase
    {
        private LapViewModel selectedLap;
        private bool autoScroll = false;

        public MainViewModel()
        {
            this.Laps = new ObservableCollection<LapViewModel>();

            this.ToggleAutoScrollCommand = new RelayCommand(() =>
            {
                this.autoScroll = !this.autoScroll;
            });
        }

        public RelayCommand ToggleAutoScrollCommand { get; private set; }

        public ObservableCollection<LapViewModel> Laps { get; private set; }

        public LapViewModel LastAddedLap { get; private set; }

        public LapViewModel SelectedLap
        {
            get
            {
                return this.selectedLap;
            }

            set
            {
                if(this.selectedLap != value)
                {
                    this.selectedLap = value;
                    RaisePropertyChanged(() => SelectedLap);

                    BuildViewModelsForSelectedLap();
                }
            }
        }

        public TrackMapViewModel TrackMap
        {
            get; private set;
        }

        public RideHeightViewModel RideHeight
        {
            get; private set;
        }

        public GripLevelViewModel GripLevel
        {
            get; private set;
        }

        //public SessionViewModel Session { get; private set; }

        //public EventViewModel Event { get; private set; }

        public SpeedViewModel Speed { get; private set; }

        public RpmViewModel Rpm { get; private set; }

        public TorqueViewModel Torque { get; private set; }

        //public ControlLightsViewModel ControlLights { get; private set; }

        //public CarPositionViewModel Position { get; private set; }

        //public TimingsViewModel Timings { get; private set; }

        public void Add(TelemetryDataSet data)
        {
            if(this.LastAddedLap == null)
            {
                StartNewLap();
            }
            else
            {
                var lapChanged = data.CurrentLap != this.LastAddedLap.Lap;
                var lapFinished = data.CurrentTrackDistance < this.LastAddedLap.LatestTrackDistance &&
                    this.LastAddedLap.LatestTrackDistance - data.CurrentTrackDistance > 100;

                if (lapChanged)
                {
                    this.LastAddedLap.Finish();
                    StartNewLap();
                }
            }

            this.LastAddedLap.Add(data);
        }

        private void StartNewLap()
        {
            this.LastAddedLap = new LapViewModel();

            this.Laps.Add(this.LastAddedLap);

            if(this.autoScroll)
            {
                this.SelectedLap = this.LastAddedLap;
            }
        }

        private void BuildViewModelsForSelectedLap()
        {
            this.TrackMap = new TrackMapViewModel(this.SelectedLap.Datasets);
            this.RideHeight = new RideHeightViewModel(this.SelectedLap.Datasets);
            this.GripLevel = new GripLevelViewModel(this.SelectedLap.Datasets);
            this.Rpm = new RpmViewModel(this.SelectedLap.Datasets);
            this.Torque = new TorqueViewModel(this.SelectedLap.Datasets);
            this.Speed = new SpeedViewModel(this.SelectedLap.Datasets);

            RaisePropertyChanged(() => TrackMap);
            RaisePropertyChanged(() => RideHeight);
            RaisePropertyChanged(() => GripLevel);
            RaisePropertyChanged(() => Rpm);
            RaisePropertyChanged(() => Torque);
            RaisePropertyChanged(() => Speed);
        }
    }
}
