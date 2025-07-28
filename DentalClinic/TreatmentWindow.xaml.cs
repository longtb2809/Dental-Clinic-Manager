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
    /// Interaction logic for Treatment.xaml
    /// </summary>
    public partial class TreatmentWindow : Window
    {
        TreatmentService treatmentService;
        private readonly User _currentUser;
        AppointmentService appointmentService;
        public TreatmentWindow(User currentUser)
        {
            InitializeComponent();
            _currentUser = currentUser;
            LoadData();
        }
        public void LoadData()
        {
            appointmentService = new AppointmentService();
            treatmentService = new TreatmentService();
            int patientId = appointmentService.GetPatientIdByUserId(_currentUser.UserId);
            dgTreatments.ItemsSource = treatmentService.GetTreatmentsByPatientId(patientId);
        }
        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            PatientDashboard patientDashboard = new PatientDashboard(_currentUser);
            patientDashboard.Show();
        }

        private void btnViewDetails_Click(object sender, RoutedEventArgs e)
        {

            var selectedItem = dgTreatments.SelectedItem as DataAccess.Models.Treatment;
            if (selectedItem == null)
            {
                MessageBox.Show("Vui lòng chọn 1 hồ sơ và thử lại!!",
                    "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            this.Hide();
            TreatmentDetailWindow treatmentDetails = new TreatmentDetailWindow(selectedItem.TreatmentId, _currentUser);
            treatmentDetails.Show();

        }

    }
}
