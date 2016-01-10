using SimRacingDashboard.DataAccess;
using SimRacingDashboard.Profiler.ViewModels;
using System;
using System.ComponentModel;
using System.IO;
using System.Windows;

namespace SimRacingDashboard.Profiler
{
    public partial class MainWindow : Window
    {
        private ITelemetryGateway gateway;
        
        public MainWindow()
        {
            AppDomain.CurrentDomain.UnhandledException += HandleException;

            InitializeComponent();

            this.WindowState = WindowState.Maximized;

            this.gateway = GatewayFactory.CreateGateway();
            var viewModel = new MainViewModel(this.gateway);
            this.DataContext = viewModel;

            this.gateway.TelemetryChanged += (sender, data) =>
            {
                Dispatcher.Invoke(() => viewModel.Add(data));
            };
        }

        private void HandleException(object sender, UnhandledExceptionEventArgs e)
        {
            var message = string.Format("{1}{0}{0}{2}{0}{0}", Environment.NewLine, DateTime.Now, e.ExceptionObject.ToString());
            File.AppendAllText("error.log", message);
            MessageBox.Show(message);
        }

        protected override void OnClosing(CancelEventArgs args)
        {
            this.gateway.Shutdown();
        }
    }
}
