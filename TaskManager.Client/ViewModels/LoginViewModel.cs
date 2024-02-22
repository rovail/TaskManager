using DryIoc;
using Newtonsoft.Json;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using TaskManager.Client.Models;
using TaskManager.Client.Services;
using TaskManager.Client.ViewModels.TaskManager.Client.ViewModels;
using TaskManager.Client.Views;
using TaskManager.Common.Models;

#nullable disable
namespace TaskManager.Client.ViewModels
{
    class LoginViewModel : BindableBase
    {
        #region SERVICES

        private UsersRequestService _usersRequestService;

        #endregion


        #region COMMANDS

        public DelegateCommand<object> GetUserFromDbCommand { get; private set; }
        public DelegateCommand<object> RegisterUserCommand { get; private set; }

        #endregion

        #region PROPERTIES

        private Window _currentWindow;

        public string UserLogin { get; set; }
        public string UserPassword { get; set; }

        private UserModel _user;
        public UserModel User
        {
            get => _user;

            set
            {
                _user = value;
                RaisePropertyChanged(nameof(User));
            }
        }

        private AuthToken _authToken;
        public AuthToken AuthToken
        {
            get => _authToken;
            set
            {
                _authToken = value;
                RaisePropertyChanged(nameof(AuthToken));
            }
        }

        public LoginViewModel()
        {

            _usersRequestService = new UsersRequestService();

            GetUserFromDbCommand = new DelegateCommand<object>(GetUserFromDb);
            RegisterUserCommand = new DelegateCommand<object>(RegisterUser);
        }


        #endregion

        #region METHODS

        private async void GetUserFromDb(object param)
        {
            var passBox = param as PasswordBox;
            if (passBox != null)
                UserPassword = passBox.Password;
            if (UserLogin != null && UserPassword != null)
                AuthToken = await _usersRequestService.GetToken(UserLogin, UserPassword);
            if (AuthToken == null)
            {
                MessageBox.Show("Authentication failed. Please check your credentials.");
                return;
            }

            _currentWindow = Window.GetWindow(passBox);
            User = await _usersRequestService.GetCurrentUser(AuthToken);
            if (User != null)
            {
                OpenMainWindow();
            }
            else
            {
                MessageBox.Show("Authentication failed. Please check your credentials.");
                return;
            }
        }

        private void RegisterUser(object param)
        {
            SignUpWindow window = new SignUpWindow();
            window.DataContext = new SignUpViewModel();
            window.ShowDialog();
        }

        private async void OpenMainWindow()
        {
            MainWindow window = new MainWindow();
            window.DataContext = new MainWindowViewModel(AuthToken, User, window);
            await Task.Delay(500);
            _currentWindow.Close();
            window.Show();


        }

        #endregion
    }
}
