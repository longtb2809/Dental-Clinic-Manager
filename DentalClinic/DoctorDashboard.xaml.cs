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
using Service;

namespace DentalClinic
{
    /// <summary>
    /// Interaction logic for DoctorDashboard.xaml
    /// </summary>
    public partial class DoctorDashboard : Window
    {
        UserService userService;
        private User _currentUser;

        public DoctorDashboard(User user)
        {
            InitializeComponent();
            _currentUser = user;
            LoadData(_currentUser);
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
        private void btnLogout_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            LoginWindow loginWindow = new LoginWindow();
            loginWindow.Show();
        }

        private void btnDoctorAppointment_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            DoctorAppointments doctorappointments = new DoctorAppointments(_currentUser);
            doctorappointments.Show();
        }

        private void btnViewBookings_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            ViewBookingsWindow viewBookingsWindow = new ViewBookingsWindow(_currentUser);
            viewBookingsWindow.Show();
        }

        private void AddTreatment_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            CreateTreatmentWindow createTreatmentWindow = new CreateTreatmentWindow(_currentUser);
            createTreatmentWindow.Show();

        }
        private void btnViewTreatment_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            DoctorViewTreatmentWindow treatmentWindow = new DoctorViewTreatmentWindow(_currentUser);
            treatmentWindow.Show();
        }

    }
}
