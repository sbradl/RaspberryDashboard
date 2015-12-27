using SimRacingDashboard.Entities;
using OxyPlot.Series;
using OxyPlot;
using OxyPlot.Axes;
using System.Collections.ObjectModel;

namespace SimRacingDashboard.Profiler.ViewModels.Engine
{
    class TorqueViewModel : AbstractViewModelWithHorizontalLineSeriesPlot
    {
        private LineSeries torque;

        public TorqueViewModel(ObservableCollection<TelemetryDataSet> datasets)
            : base(datasets)
        {
        }

        protected override string Title
        {
            get
            {
                return "Torque";
            }
        }

        protected override void InitializeDataSeries()
        {
            this.torque = new LineSeries { Color = OxyColors.Blue, TrackerFormatString = "{0}\nLapTime: {2:F3}s\nTorque: {4:F0}Nm" };
            DataPlot.Axes.Add(new LinearAxis { Unit = "Nm", Position = AxisPosition.Left, IsZoomEnabled = false });
            DataPlot.Series.Add(this.torque);
        }

        protected override void Render(TelemetryDataSet data)
        {
            var time = data.Timings.CurrentLapTime.TotalSeconds;

            this.torque.Points.Add(new DataPoint(time, data.Engine.Torque));
        }
    }
}
