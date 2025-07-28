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
using Microsoft.EntityFrameworkCore.Diagnostics;
using Service;
using UserModel = DataAccess.Models.User;

namespace DentalClinic
{
    /// <summary>
    /// Interaction logic for DoctorAppointments.xaml
    /// </summary>
    public partial class DoctorAppointments : Window
    {
        AppointmentService appointmentService;
        private UserModel _currentUser;
        public DoctorAppointments(UserModel user)
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
                int doctorId = appointmentService.GetDoctorIdByUserId(_currentUser.UserId);
                dgDoctorAppointments.ItemsSource = appointmentService.GetAppointmentsByDoctorId(doctorId);
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
    }
}
