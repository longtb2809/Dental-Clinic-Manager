using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using Service;
using UserModel = DataAccess.Models.User;

namespace DentalClinic
{
    public partial class CustomerManagerWindow : Window
    {
        private readonly UserService userService;
        private readonly UserModel _currentUser;

        public CustomerManagerWindow(UserModel user)
        {
            InitializeComponent();
            _currentUser = user;
            userService = new UserService();
            LoadData();
        }

        private void LoadData()
        {
            try
            {
                var userList = userService.GetAllUsers();
                dgUsers.ItemsSource = userList;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải danh sách người dùng: " + ex.Message);
            }
        }

        private void AddUser_Click(object sender, RoutedEventArgs e)
        {
            var addWindow = new AddOrEditUserWindow(_currentUser);
            addWindow.ShowDialog();
            LoadData();
        }

        private void EditUserButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is UserModel user)
            {
                var editWindow = new AddOrEditUserWindow(_currentUser, user);
                editWindow.ShowDialog();
                LoadData();
            }
        }

        private void DeleteUserButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is UserModel user)
            {
                var result = MessageBox.Show($"Bạn có chắc chắn muốn xoá user '{user.FullName}'?",
                                             "Xác nhận xoá",
                                             MessageBoxButton.YesNo,
                                             MessageBoxImage.Warning);

                if (result == MessageBoxResult.Yes)
                {
                    bool success = userService.DeleteUser(user.UserId);
                    if (success)
                    {
                        MessageBox.Show("Xoá người dùng thành công!", "Thành công", MessageBoxButton.OK, MessageBoxImage.Information);
                        LoadData();
                    }
                    else
                    {
                        MessageBox.Show("Xoá người dùng thất bại!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            var adminDashboard = new AdminDashboard(_currentUser);
            adminDashboard.Show();
            this.Close();
        }
    }
}
