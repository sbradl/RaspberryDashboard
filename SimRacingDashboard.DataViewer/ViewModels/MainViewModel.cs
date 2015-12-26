using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimRacingDashboard.Entities;
using System.Collections.ObjectModel;
using System.Reflection;

namespace SimRacingDashboard.DataViewer.ViewModels
{
    class MainViewModel : ViewModelBase
    {
        private IEnumerable<string> selectedProperties = new List<string>();

        public MainViewModel()
        {
            this.AvailableProperties = new ObservableCollection<string>(PrintProperties(typeof(TelemetryDataSet), string.Empty));
            this.Datasets = new ObservableCollection<TelemetryDataSet>();

            BuildColumnConfig();
        }

        public ObservableCollection<string> AvailableProperties { get; private set; }

        public IEnumerable<string> SelectedProperties
        {
            get
            {
                return this.selectedProperties;
            }

            set
            {
                this.selectedProperties = value;
                BuildColumnConfig();
            }
        }

        public ObservableCollection<TelemetryDataSet> Datasets { get; private set; }

        public ColumnConfig ColumnConfig { get; set; }

        public TelemetryDataSet SelectedDataset
        {
            get
            {
                return this.Datasets.FirstOrDefault(); ;
            }

            set
            {
                ///var x = value;
            }
        }
        
        internal void Add(TelemetryDataSet data)
        {
            this.Datasets.Add(data);

            if(data.Engine.Shift)
            {

            }
        }

        private void BuildColumnConfig()
        {
            this.ColumnConfig = new ColumnConfig
            {
                Columns = this.SelectedProperties.Select(property => new Column { Header = property, DataField = property })
            };

            RaisePropertyChanged(() => ColumnConfig);
        }

        private IEnumerable<string> PrintProperties(Type objType, string baseName)
        {
            PropertyInfo[] properties = objType.GetProperties();
            foreach (PropertyInfo property in properties)
            {
                var newBaseName = string.IsNullOrWhiteSpace(baseName) ? property.Name : baseName + "." + property.Name;
                var propertyType = property.PropertyType;

                if( propertyType.IsPrimitive || 
                    propertyType == typeof(TimeSpan) ||
                    propertyType.IsEnum)
                {
                    yield return newBaseName;
                }
                else if(propertyType.Namespace.StartsWith("SimRacingDashboard"))
                {
                    foreach (var p in PrintProperties(propertyType, newBaseName))
                    {
                        yield return p;
                    }
                }
            }
        }
    }
}
