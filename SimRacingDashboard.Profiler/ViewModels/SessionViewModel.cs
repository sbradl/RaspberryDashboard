using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimRacingDashboard.Entities;

namespace SimRacingDashboard.Profiler.ViewModels
{
    class SessionViewModel : ViewModelBase
    {
        private string sessionState;
        private int participants;

        public string SessionState
        {
            get
            {
                return this.sessionState;
            }

            private set
            {
                this.sessionState = value;
                RaisePropertyChanged(() => this.SessionState);
            }
        }

        public int Participants
        {
            get
            {
                return this.participants;
            }

            private set
            {
                this.participants = value;
                RaisePropertyChanged(() => this.Participants);
            }
        }

        public void Add(TelemetryDataSet data)
        {
            this.SessionState = data.Session.State.ToString();
            this.Participants = data.Session.Participants;
        }
    }
}
