using SimRacingDashboard.DataAccess;
using SimRacingDashboard.DataViewer.ViewModels;
using System.ComponentModel;
using System.Linq;
using System.Windows;

namespace SimRacingDashboard.DataViewer
{
    public partial class MainWindow : Window
    {
        private ICarStateGateway gateway;
        private MainViewModel viewModel;

        public MainWindow()
        {
            InitializeComponent();

            this.gateway = new DataAccess.PCars.CarStateGateway();

            this.viewModel = new MainViewModel();
            this.DataContext = this.viewModel;

            this.gateway.CarStateChanged += (sender, data) =>
            {
                Dispatcher.Invoke(() => this.viewModel.Add(data));
            };

            this.gateway.StartReading();
        }

        protected override void OnClosing(CancelEventArgs args)
        {
            this.gateway.Shutdown();
        }

        private void ListView_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            this.viewModel.SelectedProperties = this.dataList.SelectedItems.Cast<string>();
        }
    }
}
