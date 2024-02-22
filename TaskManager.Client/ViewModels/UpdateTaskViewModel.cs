using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Net;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using TaskManager.Client.Commands;
using TaskManager.Client.Models;
using TaskManager.Client.Services;
using TaskManager.Common.Models;

namespace TaskManager.Client.ViewModels
{
    public class UpdateTaskViewModel : INotifyPropertyChanged
    {
        private readonly TasksRequestService _taskService;
        private readonly AuthToken _token;
        private TaskModel _task;
        private Window _currentWindow;
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

        private bool _isSaveButtonClicked;
        public bool IsSaveButtonClicked
        {
            get { return _isSaveButtonClicked; }
            set
            {
                if (_isSaveButtonClicked != value)
                {
                    _isSaveButtonClicked = value;
                    OnPropertyChanged(nameof(IsSaveButtonClicked));

                    OnPropertyChanged(nameof(IsCloseButtonEnabled));
                }
            }
        }
        public bool IsCloseButtonEnabled => !_isSaveButtonClicked;

        public ICommand SaveCommand { get; private set; }

        public UpdateTaskViewModel(TasksRequestService taskService, AuthToken token, TaskModel task, Window window)
        {
            _taskService = taskService;
            _token = token;
            _task = task;

            _currentWindow = window;

            SaveCommand = new RelayCommand(async () => await SaveAsync());;
        }
        private UpdateTaskViewModel()
        {
            
        }

        private async Task SaveAsync()
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(Name) && !string.IsNullOrWhiteSpace(Description))
                {
                    IsSaveButtonClicked = true;
                    IsProgressBarVisible = Visibility.Visible;
                    
                    _task.Name = Name;
                    _task.Description = Description;

                    if (EndDate < _task.Startdate)
                    {
                        EndDate = _task.Startdate;
                    }
                    _task.Enddate = EndDate;

                    HttpStatusCode result = await _taskService.UpdateTask(_token, _task);
                    await Task.Delay(1000);
                }
                else
                {
                    MessageBox.Show("Please, enter data in Name and Description fields.");
                    return;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                MessageBox.Show("An error occurred while saving the task. Please try again later.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            HideProgressBarAndCloseWindow();
        }

        private void HideProgressBarAndCloseWindow()
        {
            IsProgressBarVisible = Visibility.Collapsed;
            _currentWindow.Close();
        }
    }
}
