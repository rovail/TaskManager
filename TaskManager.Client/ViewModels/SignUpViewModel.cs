using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using TaskManager.Client.Models;
using TaskManager.Client.Services;
using TaskManager.Common.Models;
using Prism.Mvvm;
using System.Threading;
using Prism.Commands;

#nullable disable
namespace TaskManager.Client.ViewModels
{
    public class SignUpViewModel : BindableBase
    {

        private UsersRequestService _usersRequestService;
        public DelegateCommand<object> RegisterUserCommand { get; private set; }

        public string UserLogin { get; set; }
        public string UserPassword { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }


        private AuthToken _authToken;
        private Window _currentWindow;

        public AuthToken AuthToken
        {
            get => _authToken;
            set
            {
                _authToken = value;
                RaisePropertyChanged(nameof(AuthToken));
            }
        }

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

        public SignUpViewModel()
        {
            _usersRequestService = new UsersRequestService();

            RegisterUserCommand = new DelegateCommand<object>(RegisterUser);
        }

        private async void RegisterUser(object param)
        {
            var passBox = param as PasswordBox;
            if (passBox != null)
                UserPassword = passBox.Password;
            if (UserLogin != null && UserPassword != null && FirstName != null && LastName != null)
            {
                if (Validator.ValidateEmail(UserLogin) && Validator.ValidatePassword(UserPassword))
                {
                    var resp = await _usersRequestService.CreateUser(null, new UserModel()
                    {
                        Email = UserLogin,
                        Password = UserPassword,
                        FirstName = FirstName,
                        LastName = LastName,
                        Status = UserStatus.User
                    });
                    if (resp != HttpStatusCode.OK)
                    {
                        MessageBox.Show("Registration error. User exists.");
                        return;
                    }
                    _currentWindow = Window.GetWindow(passBox);
                    AuthToken = await _usersRequestService.GetToken(UserLogin, UserPassword);

                    User = (AuthToken != null) ? await _usersRequestService.GetCurrentUser(AuthToken) : null;
                    if (User != null)
                    {
                        MessageBox.Show("You have successfully registered!");
                        Thread.Sleep(500);
                        _currentWindow.Close();
                        return;
                    }
                    else
                    {
                        MessageBox.Show("Sign Up failed. Please check your credentials.");
                        Thread.Sleep(500);
                        //_currentWindow.Close();
                        return;
                    }
                }
                MessageBox.Show("Invalid email or password format. Please check your input."); 
                Thread.Sleep(500);
            }
            return;
        }
    }
}
