using Newtonsoft.Json.Linq;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using TaskManager.Client.Models;
using TaskManager.Client.Services;
using TaskManager.Client.Views;
using TaskManager.Common.Models;

#nullable disable
namespace TaskManager.Client.ViewModels
{
    public class UserTasksPageViewModel : BindableBase
    {
        private AuthToken _token;
        private TasksRequestService _tasksRequestService;
        private UsersRequestService _usersRequestService;
        private UserModel _currentUser;
        public DelegateCommand<TaskClient> UpdateCommand { get; private set; }
        public DelegateCommand<TaskClient> DeleteCommand { get; private set; }
        public DelegateCommand<TaskClient> CreateCommand { get; private set; }

        public UserTasksPageViewModel(AuthToken token, UserModel user)
        {
            _token = token;
            _currentUser = user;
            _tasksRequestService = new TasksRequestService();
            _usersRequestService = new UsersRequestService();

            UpdateCommand = new DelegateCommand<TaskClient>(async task => await UpdateMethodAsync(task));
            DeleteCommand = new DelegateCommand<TaskClient>(async task => await DeleteMethodAsync(task));
            CreateCommand = new DelegateCommand<TaskClient>(async task => await CreateMethodAsync());

            AllTasks = new ObservableCollection<TaskClient>();
            _ = LoadAllTasks();
        }
        private ObservableCollection<TaskClient> _allTasks;
        public ObservableCollection<TaskClient> AllTasks
        {
            get => _allTasks;
            set => SetProperty(ref _allTasks, value);
        }

        private TaskClient _selectedItem;
        public TaskClient SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                _selectedItem = value;
                RaisePropertyChanged(nameof(SelectedItem));
            }
        }

        public async Task<ObservableCollection<TaskClient>> GetAllTasksAsync()
        {
            var tasks = (await _tasksRequestService.GetAllTasks(_token)).Select(async task =>
            {
                var taskClient = new TaskClient(task);

                if (task.CreatorId != null)
                {
                    int id = (int)task.CreatorId;
                    taskClient.Creator = await _usersRequestService.GetUserById(_token, id);
                }
                if (task.ExecutorId != null)
                {
                    int id = (int)task.ExecutorId;
                    taskClient.Executor = await _usersRequestService.GetUserById(_token, id);
                }

                return taskClient;
            });

            return new ObservableCollection<TaskClient>(await Task.WhenAll(tasks));
        }

        public async Task LoadAllTasks()
        {
            AllTasks = await GetAllTasksAsync();
            RaisePropertyChanged(nameof(AllTasks));
        }

        private async Task UpdateMethodAsync(TaskClient task)
        {
            if (task != null)
            {


                UpdateTaskWindow window = new UpdateTaskWindow();
                UpdateTaskViewModel viewModel = new UpdateTaskViewModel(_tasksRequestService, _token, task.Model, window);
                window.DataContext = viewModel;

                window.ShowDialog();

                await LoadAllTasks();
            }
        }

        private async Task DeleteMethodAsync(TaskClient task)
        {
            if (task != null)
            {
                task.IsUpdateEnabled = false;
                await _tasksRequestService.DeleteTask(_token, task.Model.Id);
                await LoadAllTasks();
                task.IsUpdateEnabled = true;
            }
        }

        private async Task CreateMethodAsync()
        {

            СreateTaskWindow window = new СreateTaskWindow();
            CreateTaskViewModel viewModel = new CreateTaskViewModel(_token, _currentUser.Email, _currentUser.Password, _currentUser.Id, window);

            window.DataContext = viewModel;

            window.ShowDialog();

            await LoadAllTasks();
        }

    }
}
