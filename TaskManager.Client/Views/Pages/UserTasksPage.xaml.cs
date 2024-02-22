using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TaskManager.Client.ViewModels;

namespace TaskManager.Client.Views.Pages
{
    /// <summary>
    /// Логика взаимодействия для UserTasksPage.xaml
    /// </summary>
    public partial class UserTasksPage : Page
    {
        public UserTasksPage()
        {
            InitializeComponent();
            Loaded += UserTasksPage_Loaded;
        }

        private void UserTasksPage_Loaded(object sender, RoutedEventArgs e)
        {
            (DataContext as UserTasksPageViewModel)?.LoadAllTasks();
        }
        
    }
}
