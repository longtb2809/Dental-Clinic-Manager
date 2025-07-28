using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Microsoft.EntityFrameworkCore;

namespace DentalClinic
{
    public class PatientDisplayItem
    {
        public int PatientId { get; set; }
        public string DisplayName { get; set; }
        public Patient Patient { get; set; }

        public PatientDisplayItem(Patient patient)
        {
            PatientId = patient.PatientId;
            DisplayName = patient.User?.FullName ?? $"Bệnh nhân {patient.PatientId}";
            Patient = patient;
        }

        public override string ToString()
        {
            return DisplayName;
        }
    }

    public partial class CreateTreatmentWindow : Window
    {
        private List<TreatmentDetail> detailList = new();
        private readonly User currentDoctor;

        public CreateTreatmentWindow(User doctor)
        {
            InitializeComponent();
            currentDoctor = doctor;
            LoadPatients();
            LoadDoctors();
            LoadServices();
        }

        private void LoadPatients()
        {
            using var context = new DentalClinicDbContext();
            var patients = context.Patients
                .Include(p => p.User)
                .Where(p => p.User != null)
                .ToList();

            var patientDisplayItems = patients.Select(p => new PatientDisplayItem(p)).ToList();

            // Debug: In ra số lượng bệnh nhân tìm được
            Console.WriteLine($"Found {patientDisplayItems.Count} patients");
            foreach (var item in patientDisplayItems)
            {
                Console.WriteLine($"Patient ID: {item.PatientId}, Display: {item.DisplayName}");
            }

            cbPatient.ItemsSource = patientDisplayItems;
            cbPatient.DisplayMemberPath = "DisplayName";
            cbPatient.SelectedValuePath = "PatientId";
        }

        private void LoadDoctors()
        {
            using var context = new DentalClinicDbContext();
            var doctors = context.Users
                .Where(u => u.RoleId == 2) // Giả sử RoleId = 2 là bác sĩ
                .ToList();

            cbDoctor.ItemsSource = doctors;
            cbDoctor.DisplayMemberPath = "FullName";
            cbDoctor.SelectedValuePath = "UserId";

            // Tự động chọn bác sĩ hiện tại
            cbDoctor.SelectedValue = currentDoctor.UserId;
        }

        private void LoadServices()
        {
            using var context = new DentalClinicDbContext();
            var services = context.Services.ToList();
            cbService.ItemsSource = services;
            cbService.DisplayMemberPath = "ServiceName";
            cbService.SelectedValuePath = "ServiceId";
        }

        private void BtnAddDetail_Click(object sender, RoutedEventArgs e)
        {
            if (cbService.SelectedItem == null || dpTreatmentDate.SelectedDate == null || string.IsNullOrWhiteSpace(txtTooth.Text))
            {
                MessageBox.Show("Vui lòng nhập đủ thông tin chi tiết điều trị.");
                return;
            }

            if (dpTreatmentDate.SelectedDate < DateTime.Today)
            {
                MessageBox.Show("Ngày điều trị không được nhỏ hơn ngày hiện tại.");
                return;
            }

            var selectedService = cbService.SelectedItem as DataAccess.Models.Service;

            var detail = new TreatmentDetail
            {
                ServiceId = selectedService?.ServiceId,
                Service = selectedService,
                TreatmentDate = dpTreatmentDate.SelectedDate,
                ToothPosition = txtTooth.Text.Trim(),
                Description = txtDescription.Text.Trim(),
                Price = selectedService?.Price ?? 0
            };

            detailList.Add(detail);
            RefreshDetailListBox();

            // Clear the form after adding detail
            cbService.SelectedIndex = -1;
            dpTreatmentDate.SelectedDate = null;
            txtTooth.Clear();
            txtDescription.Clear();
        }

        private void RefreshDetailListBox()
        {
            lbDetails.Items.Clear();
            foreach (var d in detailList)
            {
                lbDetails.Items.Add($"{d.TreatmentDate:dd/MM/yyyy} - {d.Service?.ServiceName} - {d.ToothPosition} - {d.Price:C0}");
            }
        }

        private void BtnSaveTreatment_Click(object sender, RoutedEventArgs e)
        {
            if (cbPatient.SelectedItem == null || cbDoctor.SelectedItem == null || dpStartDate.SelectedDate == null)
            {
                MessageBox.Show("Vui lòng chọn bệnh nhân, bác sĩ và ngày bắt đầu điều trị.");
                return;
            }

            if (dpStartDate.SelectedDate < DateTime.Today)
            {
                MessageBox.Show("Ngày bắt đầu điều trị không được nhỏ hơn ngày hiện tại.");
                return;
            }

            if (dpEndDate.SelectedDate.HasValue && dpEndDate.SelectedDate < dpStartDate.SelectedDate)
            {
                MessageBox.Show("Ngày kết thúc không được nhỏ hơn ngày bắt đầu.");
                return;
            }

            if (detailList.Count == 0)
            {
                MessageBox.Show("Vui lòng thêm ít nhất một chi tiết điều trị.");
                return;
            }

            try
            {
                using var context = new DentalClinicDbContext();

                var selectedPatientItem = cbPatient.SelectedItem as PatientDisplayItem;
                var treatment = new DataAccess.Models.Treatment
                {
                    PatientId = selectedPatientItem?.PatientId,
                    DoctorId = (int?)cbDoctor.SelectedValue,
                    StartDate = dpStartDate.SelectedDate,
                    EndDate = dpEndDate.SelectedDate,
                    Note = txtNote.Text?.Trim()
                };

                context.Treatments.Add(treatment);
                context.SaveChanges(); // Lưu treatment trước để có TreatmentId

                // Bây giờ thêm các chi tiết điều trị
                foreach (var detail in detailList)
                {
                    var treatmentDetail = new TreatmentDetail
                    {
                        TreatmentId = treatment.TreatmentId,
                        ServiceId = detail.ServiceId,
                        TreatmentDate = detail.TreatmentDate,
                        ToothPosition = detail.ToothPosition,
                        Description = detail.Description,
                        Price = detail.Price
                    };
                    
                    context.TreatmentDetails.Add(treatmentDetail);
                }

                context.SaveChanges();
                MessageBox.Show("Tạo hồ sơ điều trị thành công!");
                
                // Mở lại DoctorDashboard
                var doctorDashboard = new DoctorDashboard(currentDoctor);
                doctorDashboard.Show();
                this.Close();
            }
            catch (Exception ex)
                    {
                MessageBox.Show($"Lỗi khi lưu hồ sơ điều trị: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            // Mở lại DoctorDashboard
            var doctorDashboard = new DoctorDashboard(currentDoctor);
            doctorDashboard.Show();
            this.Close();
        }
    }
}
