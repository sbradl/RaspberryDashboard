using SimRacingDashboard.DataAccess;
using SimRacingDashboard.Profiler.Gateways;
using SimRacingDashboard.Profiler.ViewModels;
using System.ComponentModel;
using System.Windows;

namespace SimRacingDashboard.Profiler
{
    public partial class MainWindow : Window
    {
        private ICarStateGateway gateway;
        
        public MainWindow()
        {
            InitializeComponent();

            this.gateway = new DataAccess.PCars.CarStateGateway();
            //this.gateway = new FileGateway();
            var viewModel = new RideHeightViewModel();
            this.DataContext = viewModel;

            this.gateway.CarStateChanged += (sender, data) =>
            {
                viewModel.Add(data);
            };

            this.gateway.StartReading();
        }

        protected override void OnClosing(CancelEventArgs args)
        {
            this.gateway.Shutdown();
        }
    }
}
