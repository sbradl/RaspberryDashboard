using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using SimRacingDashboard.DataAccess;
using SimRacingDashboard.Entities;
using SimRacingDashboard.Profiler.ViewModels.Engine;
using SimRacingDashboard.Profiler.ViewModels.Tires;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace SimRacingDashboard.Profiler.ViewModels
{
    class MainViewModel : ViewModelBase
    {
        private ITelemetryGateway gateway;

        private LapViewModel selectedLap;
        private IOViewModel io = new IOViewModel();

        private bool autoScroll = false;

        private IList<TelemetryDataSet> allDataSets = new List<TelemetryDataSet>();
        private bool isDirty;
        private bool isRecording;

        public MainViewModel(ITelemetryGateway gateway)
        {
            this.gateway = gateway;
            this.Laps = new ObservableCollection<LapViewModel>();

            this.ToggleAutoScrollCommand = new RelayCommand(() =>
            {
                this.autoScroll = !this.autoScroll;
            });

            this.OpenCommand = new RelayCommand(Open);
            this.SaveCommand = new RelayCommand(Save, CanSave);
            this.SaveAsCommand = new RelayCommand(SaveAs);
        }

        public RelayCommand ToggleAutoScrollCommand { get; private set; }

        public RelayCommand OpenCommand { get; private set; }

        public RelayCommand SaveCommand { get; private set; }
        public RelayCommand SaveAsCommand { get; private set; }

        public bool IsRecording
        {
            get
            {
                return this.isRecording;
            }

            set
            {
                if(this.isRecording != value)
                {
                    this.isRecording = value;

                    if(this.IsRecording)
                    {
                        this.gateway.StartReading();
                    }
                    else
                    {
                        this.gateway.Shutdown();
                    }

                    RaisePropertyChanged(() => IsRecording);
                }
            }
        }

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

            this.isDirty = true;
            this.allDataSets.Add(data);
            this.LastAddedLap.Add(data);
        }

        private void Close()
        {
            this.Laps.Clear();
            this.LastAddedLap = null;
            this.SelectedLap = null;
            this.allDataSets.Clear();
            this.isDirty = false;

            BuildViewModelsForSelectedLap();
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
            if (this.SelectedLap == null)
            {
                this.TrackMap = null;
                this.RideHeight = null;
                this.GripLevel = null;
                this.Rpm = null;
                this.Torque = null;
                this.Speed = null;
            }
            else
            {
                this.TrackMap = new TrackMapViewModel(this.SelectedLap.Datasets);
                this.RideHeight = new RideHeightViewModel(this.SelectedLap.Datasets);
                this.GripLevel = new GripLevelViewModel(this.SelectedLap.Datasets);
                this.Rpm = new RpmViewModel(this.SelectedLap.Datasets);
                this.Torque = new TorqueViewModel(this.SelectedLap.Datasets);
                this.Speed = new SpeedViewModel(this.SelectedLap.Datasets);
            }

            RaisePropertyChanged(() => TrackMap);
            RaisePropertyChanged(() => RideHeight);
            RaisePropertyChanged(() => GripLevel);
            RaisePropertyChanged(() => Rpm);
            RaisePropertyChanged(() => Torque);
            RaisePropertyChanged(() => Speed);
        }

        private void Open()
        {
            var datasets = this.io.Open();

            if(datasets == null)
            {
                return;
            }

            Close();

            foreach (var dataset in datasets)
            {
                Add(dataset);
            }
        }

        private void Save()
        {
            this.isDirty = !this.io.Save(this.allDataSets);
        }

        private bool CanSave()
        {
            return true; // this.isDirty;
        }

        private void SaveAs()
        {
            this.isDirty = !this.io.SaveAs(this.allDataSets);
        }
    }
}
