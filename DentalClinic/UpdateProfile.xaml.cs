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
    /// <summary>
    /// Interaction logic for UpdateProfile.xaml
    /// </summary>
    public partial class UpdateProfile : Window
    {
        private UserModel _currentUser;
        private User _user;
        private readonly UserService userService;

        public UpdateProfile(User user)
        {
            InitializeComponent();
            _user = user;
            _currentUser = new UserModel(); 
            userService = new UserService();
            LoadUserData();
        }
        private void LoadUserData()
        {
            txtFullName.Text = _user.FullName;
            txtEmail.Text = _user.Email;
            txtPhone.Text = _user.Phone;
            txtAddress.Text = _user.Address;

            foreach (ComboBoxItem item in cbGender.Items)
            {
                if (item.Content.ToString() == _user.Gender)
                {
                    cbGender.SelectedItem = item;
                    break;
                }
            }

            if (_user.DateOfBirth.HasValue)
                dpDateOfBirth.SelectedDate = _user.DateOfBirth.Value.ToDateTime(new TimeOnly(0, 0));
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            var patientDashboard = new PatientDashboard(_user);
            patientDashboard.Show();

        }
        private void btnSave_Click_1(object sender, RoutedEventArgs e)
        {
            _user.FullName = txtFullName.Text.Trim();
            _user.Email = txtEmail.Text.Trim();
            _user.Phone = txtPhone.Text.Trim();
            _user.Gender = (cbGender.SelectedItem as ComboBoxItem)?.Content.ToString();
            _user.DateOfBirth = dpDateOfBirth.SelectedDate.HasValue
                ? DateOnly.FromDateTime(dpDateOfBirth.SelectedDate.Value)
                : null;
            _user.Address = txtAddress.Text.Trim();

            bool success = userService.UpdateUser(_user);
            if (success)
            {
                MessageBox.Show("Cập nhật thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                PatientDashboard patientdashboard = new PatientDashboard(_user);
                patientdashboard.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("Lỗi khi cập nhật!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }
    }
}
