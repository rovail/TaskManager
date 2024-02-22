using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using TaskManager.Client.Commands;
using TaskManager.Client.Models;
using TaskManager.Client.Services;
using TaskManager.Common.Models;

namespace TaskManager.Client.ViewModels
{
    public class CreateTaskViewModel : INotifyPropertyChanged
    {
        private readonly TasksRequestService _taskService;
        private readonly AuthToken _token;
        private readonly Window _currentWindow;
        private readonly string _login;
        private readonly string _password;
        private readonly int _id;

        private string _name;
        private string _description;
        private DateTime _endDate;

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetProperty<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }
        [Required]
        public string Name
        {
            get { return _name; }
            set { SetProperty(ref _name, value); }
        }
        [Required]

        public string Description
        {
            get { return _description; }
            set { SetProperty(ref _description, value); }
        }

        [Required]

        public DateTime EndDate
        {
            get { return _endDate; }
            set { SetProperty(ref _endDate, value); }
        }

        private Visibility _isProgressBarVisible = Visibility.Collapsed;

        public Visibility IsProgressBarVisible
        {
            get { return _isProgressBarVisible; }
            set
            {
                if (_isProgressBarVisible != value)
                {
                    SetProperty(ref _isProgressBarVisible, value);
                }
            }
        }
        private bool _isCreateButtonClicked;
        public bool IsCreateButtonClicked
        {
            get { return _isCreateButtonClicked; }
            set
            {
                if (_isCreateButtonClicked != value)
                {
                    _isCreateButtonClicked = value;
                    OnPropertyChanged(nameof(IsCreateButtonClicked));

                    OnPropertyChanged(nameof(IsCloseButtonEnabled));
                }
            }
        }
        public bool IsCloseButtonEnabled => !_isCreateButtonClicked;

        public ICommand CreateCommand { get; private set; }

        public CreateTaskViewModel(AuthToken token, string login, string password, int id, Window window)
        {
            _taskService = new TasksRequestService();
            _token = token;
            _currentWindow = window;

            _login = login;
            _password = password;
            _id = id;
            

            CreateCommand = new RelayCommand(async () =>
            {
                IsCreateButtonClicked = true;
                await CreateAsync();
            });
        }

        public CreateTaskViewModel()
        {
            
        }

        private async Task CreateAsync()
        {
            if (!string.IsNullOrWhiteSpace(Name) && !string.IsNullOrWhiteSpace(Description))
            {
                try
                {
                    IsProgressBarVisible = Visibility.Visible;

                    var token = await new UsersRequestService().GetToken(_login, _password);

                    var taskModel = new TaskModel(_name, _description, DateTime.Now, _endDate, "Task");
                    taskModel.ExecutorId = _id;
                    taskModel.DeskId = 6;

                    var res = await _taskService.CreateTask(token, taskModel);
                    await Task.Delay(1000);
                    IsProgressBarVisible = Visibility.Collapsed;
                    _currentWindow.Close();
                    return;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                    MessageBox.Show("An error occurred while creating the task. Please try again later.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    IsProgressBarVisible = Visibility.Collapsed;
                    IsCreateButtonClicked = false;
                    return;
                }
            }
            MessageBox.Show("Please, enter data in Name and Description fields.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            IsProgressBarVisible = Visibility.Collapsed;
            IsCreateButtonClicked = false;
            return;
        }
    }
}
