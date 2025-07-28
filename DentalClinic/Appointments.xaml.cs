using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Microsoft.EntityFrameworkCore;
using Service;
using UserModel = DataAccess.Models.User; 

namespace DentalClinic
{
    public partial class Appointments : Window
    {
        AppointmentService appointmentService;
        private UserModel _currentUser;

        public Appointments(UserModel user)
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
                int patientId = appointmentService.GetPatientIdByUserId(_currentUser.UserId);
                dgAppointments.ItemsSource = appointmentService.GetAppointmentsByPatientId(patientId);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải lịch hẹn: " + ex.Message);
            }
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            PatientDashboard patientDashboard = new PatientDashboard(_currentUser);
            patientDashboard.Show();
        }
    }
}
