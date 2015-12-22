using OxyPlot;
using OxyPlot.Annotations;
using OxyPlot.Axes;
using SimRacingDashboard.Entities;
using System.Collections.Generic;

namespace SimRacingDashboard.Profiler.ViewModels
{
    abstract class AbstractViewModelWithHorizontalLineSeriesPlot : AbstractViewModelWithPlot
    {
        private LineAnnotation verticalLine = new LineAnnotation { Type = LineAnnotationType.Vertical, Color = OxyColors.Red };

        private Axis xAxis = new LinearAxis { Unit = "s", Position = AxisPosition.Bottom, IsZoomEnabled = true };

        private IList<CarState> datasets;

        public AbstractViewModelWithHorizontalLineSeriesPlot(IList<CarState> datasets)
        {
            this.datasets = datasets;

            DataPlot.Annotations.Add(this.verticalLine);
            DataPlot.Axes.Add(this.xAxis);
            DataPlot.LegendPlacement = LegendPlacement.Inside;
            DataPlot.LegendPosition = LegendPosition.TopRight;

            SelectedDatasetChanged += (sender, datasetIndex) =>
            {
                var selectedDataset = this.datasets[datasetIndex];
                this.verticalLine.X = selectedDataset.Timings.CurrentLapTime.TotalSeconds;
            };
        }
    }
}
