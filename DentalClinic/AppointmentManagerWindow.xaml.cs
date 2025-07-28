using DataAccess.Models;
using Service;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using UserModel = DataAccess.Models.User;

namespace DentalClinic
{
    public partial class AppointmentManagerWindow : Window
    {
        public ObservableCollection<Appointment> Appointments { get; set; } = new ObservableCollection<Appointment>();
        private UserModel _currentUser;
        private AppointmentService _appointmentService;

        public AppointmentManagerWindow(UserModel user)
        {
            InitializeComponent();
            _currentUser = user;
            _appointmentService = new AppointmentService();
            LoadAppointments();
        }

        private void LoadAppointments()
        {
            try
            {
                var appointments = _appointmentService.GetAllAppointments();
                Appointments = new ObservableCollection<Appointment>(appointments);
                AppointmentDataGrid.ItemsSource = Appointments;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải danh sách cuộc hẹn: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        

        

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            var selected = (Appointment)AppointmentDataGrid.SelectedItem;
            if (selected == null)
            {
                MessageBox.Show("Vui lòng chọn một cuộc hẹn để xoá.");
                return;
            }

            var result = MessageBox.Show($"Bạn có chắc chắn muốn xoá cuộc hẹn ID {selected.AppointmentId}?", "Xác nhận", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    if (_appointmentService.DeleteAppointment(selected.AppointmentId))
                    {
                        LoadAppointments(); // Tải lại danh sách
                        MessageBox.Show("Xoá cuộc hẹn thành công!", "Thành công", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        MessageBox.Show("Xoá cuộc hẹn thất bại!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi khi xoá cuộc hẹn: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            var adminDashboard = new AdminDashboard(_currentUser);
            adminDashboard.Show();
        }
    }
}
