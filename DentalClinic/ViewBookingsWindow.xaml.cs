using DataAccess.Models;
using Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using UserModel = DataAccess.Models.User;

namespace DentalClinic
{
   
    public partial class ViewBookingsWindow : Window
    {
        private UserModel _currentUser;
        private AppointmentService appointmentService;
        public ViewBookingsWindow(User user)

        {
            InitializeComponent();
            _currentUser = user;
            LoadData(); 
        }
        private void LoadData()
        {
            try
            {
                appointmentService = new AppointmentService();

                
                int? doctorId = appointmentService.GetDoctorIdByUserId(_currentUser.UserId);

                if (doctorId == null)
                {
                    MessageBox.Show("Không tìm thấy thông tin bác sĩ!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                
                var pendingAppointments = appointmentService
                    .GetAppointmentsByDoctorId(doctorId.Value)
                    .Where(a => !string.IsNullOrEmpty(a.Status) &&
                                a.Status.Trim().ToLower() == "chờ duyệt")
                    .ToList();

                dgBookings.ItemsSource = pendingAppointments;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải lịch hẹn: " + ex.Message);
            }


        }
        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            DoctorDashboard doctorDashboard = new DoctorDashboard(_currentUser);
            doctorDashboard.Show();
        }

        private void btnViews_Click(object sender, RoutedEventArgs e)
        {
            if (dgBookings.SelectedItem is Appointment selectedAppointment)
            {
                ConfirmAppointmentWindow confirmWindow = new ConfirmAppointmentWindow(selectedAppointment);
                bool? result = confirmWindow.ShowDialog();

                if (result == true)
                {
                    LoadData(); 
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một lịch hẹn.");
            }
        }
        }
}
