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
    /// Interaction logic for AdminDashboard.xaml
    /// </summary>
    public partial class AdminDashboard : Window
    {
        UserService userService;
        private User _currentUser;
        public AdminDashboard(User user)
        {
            InitializeComponent();
            _currentUser = user;
        }

        private void btnLogout_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            LoginWindow loginWindow = new LoginWindow();
            loginWindow.Show();
        }

        private void btnCustomerManager_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            var customerManagerWindow = new CustomerManagerWindow(_currentUser);
            customerManagerWindow.ShowDialog();
        }
        private void btnViewAppointment_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            var appointmentManagerWindow = new AppointmentManagerWindow(_currentUser);
            appointmentManagerWindow.ShowDialog();

        }

        private void btnService_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            var addServiceWindow = new AddServiceWindow(_currentUser);
            addServiceWindow.ShowDialog();
        }
    }
    
}
