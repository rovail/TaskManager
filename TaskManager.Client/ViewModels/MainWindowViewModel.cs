using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using TaskManager.Client.Models;
using TaskManager.Client.Services;
using TaskManager.Client.Views;
using TaskManager.Client.Views.Pages;
using TaskManager.Common.Models;
using static System.Net.Mime.MediaTypeNames;

#nullable disable
namespace TaskManager.Client.ViewModels
{
    namespace TaskManager.Client.ViewModels
    {
        public class MainWindowViewModel : BindableBase
        {
            UsersRequestService _userService;

            public DelegateCommand CreateTaskCommand { get; set; }
            //public DelegateCommand UpdatePageCommand { get; set; }
            //public DelegateCommand CreatePageCommand { get; set; }
            public DelegateCommand MyTasksPageCommand { get; set; }
            public DelegateCommand EditCommand { get; set; }

            private Window _currentWindow;

            private AuthToken _token;
            public AuthToken Token
            {
                get => _token;
                set
                {
                    _token = value;
                    RaisePropertyChanged(nameof(AuthToken));
                }
            }
            private UserModel _currentUser;
            public UserModel CurrentUser 
            { 
                get => _currentUser; 
                set
                {
                    _currentUser = value;
                    RaisePropertyChanged(nameof(CurrentUser));
                }
            }

            private UserStatus _userStatus;
            public UserStatus UserStatus 
            {
                get => _userStatus;
                set
                {
                    _userStatus = value;
                    RaisePropertyChanged(nameof(UserStatus));
                }
            }

            private string _userFullName;
            public string UserFullName 
            { 
                get => _userFullName;
                set
                {
                    _userFullName = value;
                    RaisePropertyChanged(nameof(UserFullName));
                }
            }

            private Page _selectedPage;
            public Page SelectedPage
            {
                get { return _selectedPage; }
                set { SetProperty(ref _selectedPage, value); }
            }

            private BitmapImage _avatar;
            public BitmapImage Avatar
            {
                get { return _avatar; }
                set { SetProperty(ref _avatar, value); }
            }

            public MainWindowViewModel(AuthToken authToken, UserModel currentUser, Window currentWindow)
            {
                _userService = new UsersRequestService();
                _currentWindow = currentWindow;
                _token = authToken;
                _currentUser = currentUser;
                LoadImage();
                _userStatus = _currentUser.Status;
                _userFullName = _currentUser.FirstName + " " + _currentUser.LastName;

                var userTasksPageViewModel = new UserTasksPageViewModel(_token, CurrentUser);

                MyTasksPageCommand = new DelegateCommand(() => OpenPage(new UserTasksPage { DataContext = userTasksPageViewModel }));
                CreateTaskCommand = new DelegateCommand(() => CreateTask());
                EditCommand = new DelegateCommand(() => Edit());

                OpenPage(new UserTasksPage { DataContext = userTasksPageViewModel });
                //MyDesksPageCommand = new DelegateCommand(() => OpenPage(new MyDesksPage()));
                //MyProjectsPageCommand = new DelegateCommand(() => OpenPage(new MyProjectsPage()));
            }

            private void LoadImage()
            {
                byte[] imageBytes = _currentUser.Photo;
                if (imageBytes != null && imageBytes.Length > 0)
                {
                    BitmapImage bitmapImage = new BitmapImage();
                    bitmapImage.BeginInit();
                    bitmapImage.StreamSource = new MemoryStream(imageBytes);
                    bitmapImage.EndInit();

                    Avatar = bitmapImage;
                }
                else
                {
                    Avatar = new BitmapImage(new Uri("pack://application:,,,/Resources/avatar.jpg"));
                }
            }

            private void OpenPage(Page page)
            {
                SelectedPage = page;
                SelectedPage.DataContext = page.DataContext;
            }

            private void CreateTask()
            {

                СreateTaskWindow window = new СreateTaskWindow();
                CreateTaskViewModel viewModel = new CreateTaskViewModel(_token, CurrentUser.Email, CurrentUser.Password, CurrentUser.Id, window);
                window.DataContext = viewModel;

                window.ShowDialog();
                var userTasksPageViewModel = new UserTasksPageViewModel(_token, CurrentUser);
                OpenPage(new UserTasksPage { DataContext = userTasksPageViewModel });
            }

            private async Task Edit()
            {
                EditProfileWindow window = new EditProfileWindow();
                EditProfileViewModel viewModel = new EditProfileViewModel(CurrentUser, window);
                window.DataContext = viewModel;

                window.ShowDialog();
                _token = await _userService.GetToken(_currentUser.Email, _currentUser.Password);
                CurrentUser = await _userService.GetCurrentUser(_token);
                UserFullName = _currentUser.FirstName + " " + _currentUser.LastName;
                LoadImage();
            }
        }
    }
}
