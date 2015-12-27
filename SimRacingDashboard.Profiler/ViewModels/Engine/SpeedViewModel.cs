using SimRacingDashboard.Entities;
using OxyPlot.Series;
using OxyPlot;
using OxyPlot.Axes;
using System.Collections.ObjectModel;

namespace SimRacingDashboard.Profiler.ViewModels.Engine
{
    class SpeedViewModel : AbstractViewModelWithHorizontalLineSeriesPlot
    {
        private LineSeries speed;

        public SpeedViewModel(ObservableCollection<TelemetryDataSet> datasets)
            : base(datasets)
        {
        }

        protected override string Title
        {
            get
            {
                return "Speed";
            }
        }

        protected override void InitializeDataSeries()
        {
            this.speed = new LineSeries { Color = OxyColors.Green, TrackerFormatString = "{0}\nLapTime: {2:F3}s\nSpeed: {4:F0}km/h" };
            DataPlot.Axes.Add(new LinearAxis { Unit = "km/h", Position = AxisPosition.Left, IsZoomEnabled = false });
            DataPlot.Series.Add(this.speed);
        }

        protected override void Render(TelemetryDataSet data)
        {
            var time = data.Timings.CurrentLapTime.TotalSeconds;

            this.speed.Points.Add(new DataPoint(time, data.Speed * 3.6));
        }
    }
}
