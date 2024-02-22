using Newtonsoft.Json.Linq;
using System.Windows;
using System.Windows.Input;
using TaskManager.Client.Services;
using TaskManager.Client.ViewModels;

namespace TaskManager.Client.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            DragMove();
        }

        private void exitApp(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
