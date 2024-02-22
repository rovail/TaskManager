using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using TaskManager.Client.Commands;
using TaskManager.Client.Models;
using TaskManager.Client.Services;
using TaskManager.Common.Models;

#nullable disable
namespace TaskManager.Client.ViewModels
{
    class EditProfileViewModel : INotifyPropertyChanged
    {
        public ICommand ChangeAvatarCommand { get; private set; }
        public ICommand SaveCommand { get; private set; }


        private string _firstName;
        public string FirstName
        {
            get { return _firstName; }
            set { SetProperty(ref _firstName, value); }
        }

        private string _lastName;
        public string LastName
        {
            get { return _lastName; }
            set { SetProperty(ref _lastName, value); }
        }

        private byte[] _avatar;
        public byte[] Avatar
        {
            get { return _avatar; }
            set { SetProperty(ref _avatar, value);  }
        }


        private UserModel _currentUser;
        private UsersRequestService _userService;
        private Window _currentWindow;
        private AuthToken _token;
        public EditProfileViewModel(UserModel user, Window window)
        {
            _userService = new UsersRequestService();
            _currentWindow = window;

            _currentUser = new UserModel();
            _currentUser.Id = user.Id;
            _currentUser.Status = user.Status;
            _currentUser.Email = user.Email;
            _currentUser.Password = user.Password;
            _currentUser.Phone = user.Phone;
            _currentUser.Photo = user.Photo;
            _currentUser.RegistrationDate = user.RegistrationDate;
            _currentUser.LastLoginDate = user.LastLoginDate;

            ChangeAvatarCommand = new RelayCommand(ChangeAvatar);
            SaveCommand = new RelayCommand(async () => await Save());
        }

        public event PropertyChangedEventHandler? PropertyChanged;
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

        public byte[] ConvertImageToByteArray(string imagePath)
        {
            byte[] byteArray = File.ReadAllBytes(imagePath);
            return byteArray;
        }

        private void ChangeAvatar()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image files (*.png;*.jpeg;*.jpg)|*.png;*.jpeg;*.jpg|All files (*.*)|*.*";

            if (openFileDialog.ShowDialog() == true)
            {
                string selectedImagePath = openFileDialog.FileName;
                byte[] imageBytes = ConvertImageToByteArray(selectedImagePath);
                _currentUser.Photo = imageBytes;
            }
        }

        private async Task Save()
        {
            if (string.IsNullOrWhiteSpace(FirstName) || string.IsNullOrWhiteSpace(LastName))
            {
                MessageBox.Show("Please enter valid values for First Name and Last Name.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            try
            {
                _token = await _userService.GetToken(_currentUser.Email, _currentUser.Password);
                _currentUser.FirstName = FirstName;
                _currentUser.LastName = LastName;
                await _userService.UpdateUser(_token, _currentUser);
                _currentWindow.Close();
                return;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while saving user information: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }
    }
}
