using GalaSoft.MvvmLight;
using Microsoft.Win32;
using SimRacingDashboard.Entities;
using SimRacingDashboard.Profiler.Interactors.IO;
using System.Collections.Generic;

namespace SimRacingDashboard.Profiler.ViewModels
{
    class IOViewModel : ViewModelBase
    {
        private string currentFilename;

        public IList<TelemetryDataSet> Open()
        {
            if (!SelectFileToOpen())
            {
                return null;
            }

            return new OpenInteractor().Execute(this.currentFilename);
        }

        public bool SaveAs(IList<TelemetryDataSet> data)
        {
            if(!AskForFilename())
            {
                return false;
            }

            return Save(data);
        }

        public bool Save(IList<TelemetryDataSet> data)
        {
            if(!CurrentFilenameIsSet)
            {
                if(!AskForFilename())
                {
                    return false;
                }
            }

            Write(data);

            return true;
        }

        private bool CurrentFilenameIsSet
        {
            get
            {
                return !string.IsNullOrWhiteSpace(this.currentFilename);
            }
        }

        private bool AskForFilename()
        {
            var dialog = new SaveFileDialog();
            dialog.AddExtension = true;
            dialog.ValidateNames = true;
            dialog.DefaultExt = "telemetry";
            dialog.Filter = "Telemetry File|*.telemetry";

            if(dialog.ShowDialog() == true)
            {
                this.currentFilename = dialog.FileName;
                return true;
            }

            return false;
        }

        private bool SelectFileToOpen()
        {
            var dialog = new OpenFileDialog();
            dialog.Filter = "Telemetry File|*.telemetry";

            if (dialog.ShowDialog() == true)
            {
                this.currentFilename = dialog.FileName;
                return true;
            }

            return false;
        }

        private void Write(IList<TelemetryDataSet> data)
        {
            new SaveInteractor().Execute(this.currentFilename, data);
        }
    }
}
