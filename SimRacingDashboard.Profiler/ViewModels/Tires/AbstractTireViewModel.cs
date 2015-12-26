using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using SimRacingDashboard.Entities;
using System.Collections.Generic;

namespace SimRacingDashboard.Profiler.ViewModels.Tires
{
    abstract class AbstractTireViewModel : AbstractViewModelWithHorizontalLineSeriesPlot
    {
        private LineSeries frontLeft;
        private LineSeries frontRight;
        private LineSeries rearLeft;
        private LineSeries rearRight;
        
        public AbstractTireViewModel(IList<TelemetryDataSet> datasets)
            : base(datasets)
        {
            frontLeft = new LineSeries { Title = "Front Left", Color = OxyColors.LightBlue, TrackerFormatString = TrackerFormatString };
            frontRight = new LineSeries { Title = "Front Right", Color = OxyColors.DarkBlue, TrackerFormatString = TrackerFormatString };
            rearLeft = new LineSeries { Title = "Rear Left", Color = OxyColors.LightGreen, TrackerFormatString = TrackerFormatString };
            rearRight = new LineSeries { Title = "Rear Right", Color = OxyColors.DarkGreen, TrackerFormatString = TrackerFormatString };

            DataPlot.Axes.Add(GetVerticalAxis());

            DataPlot.Series.Add(this.frontLeft);
            DataPlot.Series.Add(this.frontRight);
            DataPlot.Series.Add(this.rearLeft);
            DataPlot.Series.Add(this.rearRight);

            foreach (var dataset in datasets)
            {
                Add(dataset);
            }
        }

        protected abstract string TrackerFormatString
        {
            get;
        }

        protected abstract Axis GetVerticalAxis();

        protected abstract double GetValueFor(Tire tire);

        private void Add(TelemetryDataSet data)
        {
            Do(() =>
            {
                var time = data.Timings.CurrentLapTime.TotalSeconds;

                this.frontLeft.Points.Add(new DataPoint(time, GetValueFor(data.Tires.FrontLeft)));
                this.frontRight.Points.Add(new DataPoint(time, GetValueFor(data.Tires.FrontRight)));
                this.rearLeft.Points.Add(new DataPoint(time, GetValueFor(data.Tires.RearLeft)));
                this.rearRight.Points.Add(new DataPoint(time, GetValueFor(data.Tires.RearRight)));
            });
        }
    }
}
