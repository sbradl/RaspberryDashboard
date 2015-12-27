using OxyPlot.Axes;
using SimRacingDashboard.Entities;
using System.Collections.ObjectModel;

namespace SimRacingDashboard.Profiler.ViewModels.Tires
{
    class GripLevelViewModel : AbstractTireViewModel
    {
        public GripLevelViewModel(ObservableCollection<TelemetryDataSet> datasets)
            : base(datasets)
        {
        }

        protected override string TrackerFormatString
        {
            get
            {
                return "{0}\nLapTime: {2:F3}s\nGrip-Level: {4:F0}%";
            }
        }

        protected override string Title
        {
            get
            {
                return "Grip-Level";
            }
        }

        protected override Axis GetVerticalAxis()
        {
            return new LinearAxis { Unit = "%", Position = AxisPosition.Left, IsZoomEnabled = false };
        }

        protected override double GetValueFor(Tire tire)
        {
            return tire.GripLevel * 100;
        }
    }
}
