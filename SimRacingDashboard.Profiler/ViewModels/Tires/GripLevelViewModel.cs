using OxyPlot.Axes;
using SimRacingDashboard.Entities;
using System.Collections.Generic;

namespace SimRacingDashboard.Profiler.ViewModels.Tires
{
    class GripLevelViewModel : AbstractTireViewModel
    {
        public GripLevelViewModel(IList<CarState> datasets)
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
