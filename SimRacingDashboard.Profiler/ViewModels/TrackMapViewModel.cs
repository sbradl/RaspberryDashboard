using OxyPlot;
using OxyPlot.Annotations;
using OxyPlot.Series;
using SimRacingDashboard.Entities;
using System.Collections.Generic;
using System.Linq;

namespace SimRacingDashboard.Profiler.ViewModels
{
    class TrackMapViewModel : AbstractViewModelWithPlot
    {
        private LineSeries position = new LineSeries { Color = OxyColors.Black };

        //private LapViewModel lap;
        private EllipseAnnotation annotation = new EllipseAnnotation { Width = 25, Height = 25, Stroke = OxyColors.Red, StrokeThickness = 2, Fill = OxyColors.Transparent };

        public TrackMapViewModel(IEnumerable<CarState> datasets)
        {
            this.DataPlot.PlotType = PlotType.Cartesian;
            this.DataPlot.Series.Add(this.position);
            DataPlot.Annotations.Add(this.annotation);

            foreach (var data in datasets)
            {
                this.position.Points.Add(new DataPoint(data.Position[0], data.Position[2]));
            }

            this.position.Points.Add(this.position.Points.First());

            SelectedDatasetChanged += (sender, datasetIndex) =>
            {
                var selectedDataset = datasets.Skip(datasetIndex - 1).Take(1).Single();

                this.annotation.X = selectedDataset.Position[0];
                this.annotation.Y = selectedDataset.Position[2];
            };
        }

        //public TrackMapViewModel(LapViewModel lap)
        //{
        //    this.lap = lap;
        //    this.DataPlot.Series.Add(this.position);

        //    foreach (var data in lap.Datasets)
        //    {
        //        this.position.Points.Add(new DataPoint(data.Position[0], data.Position[2]));
        //    }

        //    lap.Datasets.CollectionChanged += Datasets_CollectionChanged;
        //}

        //private void Datasets_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        //{
        //    Do(() =>
        //    {
        //        foreach (CarState data in e.NewItems)
        //        {
        //            this.position.Points.Add(new DataPoint(data.Position[0], data.Position[2]));
        //        }
        //    });
        //}
    }
}
