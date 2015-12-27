using SimRacingDashboard.Entities;
using System.Collections.ObjectModel;
using OxyPlot.Series;
using OxyPlot;
using OxyPlot.Axes;

namespace SimRacingDashboard.Profiler.ViewModels.Engine
{
    class RpmViewModel : AbstractViewModelWithHorizontalLineSeriesPlot
    {
        private LineSeries rpm;

        public RpmViewModel(ObservableCollection<TelemetryDataSet> datasets)
            : base(datasets)
        {
        }

        protected override void InitializeDataSeries()
        {
            this.rpm = new LineSeries { Color = OxyColors.Red, TrackerFormatString = "{0}\nLapTime: {2:F3}s\nRPM: {4:F0}/min" };
            DataPlot.Axes.Add(new LinearAxis { Unit = "1/min", Position = AxisPosition.Left, IsZoomEnabled = false });
            DataPlot.Series.Add(this.rpm);
        }

        protected override void Render(TelemetryDataSet data)
        {
            var time = data.Timings.CurrentLapTime.TotalSeconds;

            this.rpm.Points.Add(new DataPoint(time, data.Engine.Rpm));
        }

        protected override string Title
        {
            get
            {
                return "RPM";
            }
        }
    }
}
