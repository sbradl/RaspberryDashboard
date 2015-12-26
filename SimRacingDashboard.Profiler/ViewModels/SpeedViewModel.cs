using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimRacingDashboard.Entities;

namespace SimRacingDashboard.Profiler.ViewModels
{
    class SpeedViewModel : ViewModelBase
    {
        public float Value { get; private set; }

        internal void Add(TelemetryDataSet data)
        {
            this.Value = data.Speed;
            RaisePropertyChanged(() => Value);
        }
    }
}
