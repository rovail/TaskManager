using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using TaskManager.Client.Models.Extensions;
using TaskManager.Common.Models;

namespace TaskManager.Client.Models
{
    public class TaskClient : INotifyPropertyChanged
    {
        public TaskModel Model { get; private set; }
        public UserModel Creator { get; set; }
        public UserModel Executor { get; set; }

        public BitmapImage Image 
        {
            get
            {
                return Model.LoadImage();
            }
        }

        public TaskClient(TaskModel model)
        {
            Model = model;
        }

        public bool IsHaveFile
        {
            get => Model?.File != null;
        }

        private bool _isUpdateEnabled = true;

        public bool IsUpdateEnabled
        {
            get { return _isUpdateEnabled; }
            set
            {
                if (_isUpdateEnabled != value)
                {
                    _isUpdateEnabled = value;
                    OnPropertyChanged(nameof(IsUpdateEnabled));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
