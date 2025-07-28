using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Service;

namespace DentalClinic
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        UserService _userService = new();
        public LoginWindow()
        {
            InitializeComponent();
        }
        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            string username = txtUsername.Text.Trim(); // Trim() để loại bỏ khoảng trắng ở đầu và cuối
            string password = txtPassword.Password.Trim();

            var userLogged = _userService.Login(username, password);
            if (userLogged == null)
            {
                MessageBox.Show("Đăng nhập không thành công. Vui lòng thử lại!!",
                    "Thông báo",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
            if (userLogged.RoleId == 1) //admin
            {
                var adminDashboard = new AdminDashboard(userLogged);
                adminDashboard.Show();
                this.Close();
            }
            else if (userLogged.RoleId == 2) //doctor
            {
                var doctorDashboard = new DoctorDashboard(userLogged);
                doctorDashboard.Show();
                this.Close();
            }
            else if (userLogged.RoleId == 4) //patient
            {

                var patientDashboard = new PatientDashboard(userLogged);
                patientDashboard.Show();
                this.Close();
            }
            else
            {
                // Chuyển sang cửa sổ khác nếu cần
                PatientDashboard patientDashboard = new PatientDashboard(userLogged);
                patientDashboard.Show();
                this.Hide();// Ẩn cửa sổ hiện tại
            }
        }

    }
}