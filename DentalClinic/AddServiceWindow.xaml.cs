using System;
using System.Windows;
using DataAccess.Models;
using Service;

namespace DentalClinic
{
    public partial class AddServiceWindow : Window
    {
        private readonly ServiceService _serviceService;
        private readonly User _currentUser;

        public AddServiceWindow(User user)
        {
            InitializeComponent();
            _currentUser = user;
            _serviceService = new ServiceService();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            string name = txtServiceName.Text.Trim();
            string description = txtDescription.Text.Trim();
            string priceText = txtPrice.Text.Trim();

            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(priceText))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ tên dịch vụ và giá tiền!", "Cảnh báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!decimal.TryParse(priceText, out decimal price) || price < 0)
            {
                MessageBox.Show("Giá tiền không hợp lệ!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var service = new DataAccess.Models.Service
            {
                ServiceName = name,
                Description = description,
                Price = price
            };

            bool success = _serviceService.AddService(service);
            if (success)
            {
                MessageBox.Show("Thêm dịch vụ thành công!", "Thành công", MessageBoxButton.OK, MessageBoxImage.Information);
                this.Close();
                var adminDashboard = new AdminDashboard(_currentUser);
                adminDashboard.Show();
            }
            else
            {
                MessageBox.Show("Thêm dịch vụ thất bại!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            var adminDashboard = new AdminDashboard(_currentUser);
            adminDashboard.Show();
        }
    }
}
