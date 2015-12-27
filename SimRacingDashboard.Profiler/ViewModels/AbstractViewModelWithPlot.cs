using GalaSoft.MvvmLight;
using OxyPlot;
using SimRacingDashboard.Entities;
using System;
using System.Collections.ObjectModel;
using System.Threading;

namespace SimRacingDashboard.Profiler.ViewModels
{
    abstract class AbstractViewModelWithPlot : ViewModelBase
    {
        private readonly Timer timer;

        private const int RenderUpdatePeriodInMs = 32;

        protected static EventHandler<int> SelectedDatasetChanged;
        
        protected AbstractViewModelWithPlot(ObservableCollection<TelemetryDataSet> datasets)
        {
            this.DataPlot = new PlotModel
            {
                Title = Title
            };

            this.DataPlot.MouseDown += OnDataPlotClicked;

            this.timer = new Timer(OnTimerElapsed);
            this.timer.Change(0, RenderUpdatePeriodInMs);

            InitializeDataSeries();

            Do(() =>
            {
                foreach (var data in datasets)
                {
                    Render(data);
                }
            });

            datasets.CollectionChanged += Datasets_CollectionChanged;
        }

        public PlotModel DataPlot { get; private set; }

        protected abstract string Title
        {
            get;
        }

        private void Datasets_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            Do(() =>
            {
                foreach (TelemetryDataSet data in e.NewItems)
                {
                    Render(data);
                }
            });
        }

        protected abstract void Render(TelemetryDataSet data);

        protected abstract void InitializeDataSeries();

        private void Do(Action action)
        {
            lock (this.DataPlot.SyncRoot)
            {
                action();

                RaisePropertyChanged(() => DataPlot);
            }
        }

        private void OnTimerElapsed(object state)
        {
            this.DataPlot.InvalidatePlot(true);
        }

        private void OnDataPlotClicked(object sender, OxyMouseDownEventArgs e)
        {
            if (e.ChangedButton != OxyMouseButton.Left || e.HitTestResult == null)
            {
                return;
            }

            if(SelectedDatasetChanged != null)
            {
                var index = (int)Math.Round(e.HitTestResult.Index);
                
                SelectedDatasetChanged(this, index);
            }
        }
    }
}
