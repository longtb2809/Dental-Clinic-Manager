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
using DataAccess.Models;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Service;

namespace DentalClinic
{
    public partial class PatientDashboard : Window
    {
        UserService userService;
        private User _currentUser;

        public PatientDashboard(User user)
        {
            InitializeComponent();
            _currentUser = user;
            LoadData(_currentUser);
        }

        private void btnLogout_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            LoginWindow loginWindow = new LoginWindow();
            loginWindow.Show();
        }


        private void LoadData(User user)
        {
            txtAddress.Text = user.Address;
            txtEmail.Text = user.Email;
            txtFullName.Text = user.FullName;
            txtGender.Text = user.Gender;
            txtPhone.Text = user.Phone;
            txtDob.Text = user.DateOfBirth.HasValue ? user.DateOfBirth.Value.ToString("dd/MM/yyyy") : "Chưa cập nhật";
        }

        private void btnAppointments_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            Appointments appointments = new Appointments(_currentUser);
            appointments.Show();
        }

        private void btnBook_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            BookingAppointment bookingAppointment = new BookingAppointment(_currentUser);
            bookingAppointment.Show();
        }

        private void btnTreatment_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            TreatmentWindow treatmentWindow = new TreatmentWindow(_currentUser);
            treatmentWindow.Show();

        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            UpdateProfile updateProfile = new UpdateProfile(_currentUser);
            updateProfile.Show();
        }
    }
}
