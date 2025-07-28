using System.Windows;
using DataAccess.Models;
using Service;

namespace DentalClinic
{
    /// <summary>
    /// Interaction logic for DoctorViewTreatmentWindow.xaml
    /// </summary>
    public partial class DoctorViewTreatmentWindow : Window
    {
        private readonly User _currentDoctor;
        private readonly TreatmentService _treatmentService;

        public DoctorViewTreatmentWindow(User doctor)
        {
            InitializeComponent();
            _currentDoctor = doctor;
            _treatmentService = new TreatmentService();
            LoadTreatments();
        }

        private void LoadTreatments()
        {
            // Lấy danh sách hồ sơ điều trị do bác sĩ phụ trách
            var treatments = _treatmentService.GetTreatmentsByDoctorId(_currentDoctor.UserId);
            dgDoctorTreatments.ItemsSource = treatments;
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            DoctorDashboard dashboard = new DoctorDashboard(_currentDoctor);
            dashboard.Show();
        }

        private void btnExamine_Click(object sender, RoutedEventArgs e)
        {
            var selectedItem = dgDoctorTreatments.SelectedItem as Treatment;
            if (selectedItem == null)
            {
                MessageBox.Show("Vui lòng chọn 1 hồ sơ điều trị!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            this.Hide();
            TreatmentDetailWindow treatmentDetailWindow = new TreatmentDetailWindow(selectedItem.TreatmentId, _currentDoctor);
            treatmentDetailWindow.Show();
        }
    }
}
