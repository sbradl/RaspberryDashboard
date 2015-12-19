using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using SimRacingDashboard.Entities;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System;

namespace SimRacingDashboard.Profiler.ViewModels
{
    public class RideHeightViewModel : ViewModelBase
    {
        private LineSeries series = new LineSeries();

        private DateTime? firstDataPointTime;

        public RideHeightViewModel()
        {
            DataPlot = new PlotModel();
            DataPlot.Axes.Add(new LinearAxis { Unit = "mm", Position = AxisPosition.Left });
            DataPlot.Series.Add(this.series);
        }

        public PlotModel DataPlot { get; set; }

        public void Add(CarState data)
        {
            if(!this.firstDataPointTime.HasValue)
            {
                this.firstDataPointTime = data.DateTime;
            }

            lock (this.DataPlot.SyncRoot)
            {
                var offset = data.DateTime - this.firstDataPointTime.Value;
                
                this.series.Points.Add(new DataPoint(offset.TotalSeconds, data.Tires.MinRideHeight * 1000));

                if (this.series.Points.Count == 500)
                {
                    this.series.Points.RemoveAt(0);
                }

                RaisePropertyChanged(() => DataPlot);
            }

            this.DataPlot.InvalidatePlot(true);
        }
    }
}
