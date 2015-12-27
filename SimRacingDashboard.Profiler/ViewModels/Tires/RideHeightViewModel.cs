using OxyPlot.Axes;
using SimRacingDashboard.Entities;
using System.Collections.ObjectModel;
using System;

namespace SimRacingDashboard.Profiler.ViewModels.Tires
{
    class RideHeightViewModel : AbstractTireViewModel
    {
        public RideHeightViewModel(ObservableCollection<TelemetryDataSet> datasets)
            : base(datasets)
        {
        }

        protected override string TrackerFormatString
        {
            get
            {
                return "{0}\nLapTime: {2:F3}s\nHeight: {4:F1}mm";
            }
        }

        protected override string Title
        {
            get
            {
                return "Ride height";
            }
        }

        protected override Axis GetVerticalAxis()
        {
            return new LinearAxis { Unit = "mm", Position = AxisPosition.Left, IsZoomEnabled = false };
        }

        protected override double GetValueFor(Tire tire)
        {
            return ToMillimeter(tire.RideHeightInMeter);
        }

        private float ToMillimeter(float rideHeight)
        {
            return rideHeight * 1000;
        }
    }
}
