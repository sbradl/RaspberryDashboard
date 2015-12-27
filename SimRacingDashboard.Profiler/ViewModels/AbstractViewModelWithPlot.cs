using GalaSoft.MvvmLight;
using OxyPlot;
using System;
using System.Threading;

namespace SimRacingDashboard.Profiler.ViewModels
{
    abstract class AbstractViewModelWithPlot : ViewModelBase
    {
        private readonly Timer timer;

        private const int RenderUpdatePeriodInMs = 32;

        protected static EventHandler<int> SelectedDatasetChanged;
        
        protected AbstractViewModelWithPlot()
        {
            this.DataPlot = new PlotModel();

            this.DataPlot.MouseDown += OnDataPlotClicked;

            this.timer = new Timer(OnTimerElapsed);
            this.timer.Change(0, RenderUpdatePeriodInMs);
        }

        public PlotModel DataPlot { get; private set; }

        protected void Do(Action action)
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
