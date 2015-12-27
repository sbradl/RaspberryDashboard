using OxyPlot;
using OxyPlot.Annotations;
using OxyPlot.Axes;
using SimRacingDashboard.Entities;
using System.Collections.ObjectModel;

namespace SimRacingDashboard.Profiler.ViewModels
{
    abstract class AbstractViewModelWithHorizontalLineSeriesPlot : AbstractViewModelWithPlot
    {
        private LineAnnotation verticalLine = new LineAnnotation { Type = LineAnnotationType.Vertical, Color = OxyColors.Red };

        private Axis xAxis = new LinearAxis { Unit = "s", Position = AxisPosition.Bottom, IsZoomEnabled = true };
        
        public AbstractViewModelWithHorizontalLineSeriesPlot(ObservableCollection<TelemetryDataSet> datasets)
            : base(datasets)
        {
            DataPlot.Annotations.Add(this.verticalLine);
            DataPlot.Axes.Add(this.xAxis);
            DataPlot.LegendPlacement = LegendPlacement.Inside;
            DataPlot.LegendPosition = LegendPosition.TopRight;

            SelectedDatasetChanged += (sender, datasetIndex) =>
            {
                var selectedDataset = datasets[datasetIndex];
                this.verticalLine.X = selectedDataset.Timings.CurrentLapTime.TotalSeconds;
            };
        }
    }
}
