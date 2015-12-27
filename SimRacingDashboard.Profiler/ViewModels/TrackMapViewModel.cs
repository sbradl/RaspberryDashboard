using OxyPlot;
using OxyPlot.Annotations;
using OxyPlot.Series;
using SimRacingDashboard.Entities;
using System.Collections.ObjectModel;
using System.Linq;
using System;

namespace SimRacingDashboard.Profiler.ViewModels
{
    class TrackMapViewModel : AbstractViewModelWithPlot
    {
        private LineSeries position = new LineSeries { Color = OxyColors.Black };
        
        private EllipseAnnotation annotation = new EllipseAnnotation { Width = 25, Height = 25, Stroke = OxyColors.Red, StrokeThickness = 2, Fill = OxyColors.Transparent };

        protected override string Title
        {
            get
            {
                return "Trackmap";
            }
        }

        public TrackMapViewModel(ObservableCollection<TelemetryDataSet> datasets)
            : base(datasets)
        {
            SelectedDatasetChanged += (sender, datasetIndex) =>
            {
                var selectedDataset = datasets.Skip(datasetIndex - 1).Take(1).Single();

                this.annotation.X = selectedDataset.Position[0];
                this.annotation.Y = selectedDataset.Position[2];
            };
        }

        protected override void InitializeDataSeries()
        {
            this.DataPlot.PlotType = PlotType.Cartesian;
            this.DataPlot.Series.Add(this.position);
            DataPlot.Annotations.Add(this.annotation);
        }

        protected override void Render(TelemetryDataSet data)
        {
            this.position.Points.Add(new DataPoint(data.Position[0], data.Position[2]));
        }
    }
}
