using DataAccess.Models;
using Service;
using System;
using System.Windows;

namespace DentalClinic
{
    public partial class TreatmentDetailWindow : Window
    {
        private readonly TreatmentDetailService _service;
        private readonly int _treatmentId;
        private readonly User _currentUser;

    
        public TreatmentDetailWindow(int treatmentId, User currentUser)
        {
            InitializeComponent();
            _service = new TreatmentDetailService();
            _treatmentId = treatmentId;
            _currentUser = currentUser;
            LoadDataToField(_treatmentId);
        }

        public void LoadDataToField(int treatmentId)
        {
            var detail = _service.GetTreatmentDetailByTreatmentId(treatmentId);
            if (detail != null)
            {
                txtPatientName.Text = detail.Treatment?.PatientName;
                txtDoctorName.Text = detail.Treatment?.DoctorName;
                txtService.Text = detail.Service?.ServiceName;
                txtDate.Text = detail.TreatmentDate?.ToString("dd/MM/yyyy");
                txtDescription.Text = detail.Description;
                txtPrice.Text = detail.Price?.ToString("N0") + " VNĐ";
            }
            else
            {
                MessageBox.Show("Không tìm thấy thông tin chi tiết.");
                this.Close();
            }
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close(); 

            TreatmentWindow treatmentWindow = new TreatmentWindow(_currentUser);
            treatmentWindow.Show();
        }
    }
}
