using DataAccess.Models;
using Microsoft.EntityFrameworkCore;
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

    public partial class BookingAppointment : Window
    {
        private readonly UserModel _currentUser;
        private readonly AppointmentService appointmentService;
        private readonly UserService userService;
        private readonly ServiceService serviceService;
        private DentalClinicDbContext _context = new DentalClinicDbContext();

        public BookingAppointment(UserModel user)
        {
            InitializeComponent();
            _currentUser = user;
            appointmentService = new AppointmentService();
            userService = new UserService();
            serviceService = new ServiceService();

            LoadDoctors();
            LoadServices();
        }

        private void LoadDoctors()
        {
            var doctorList = userService.GetDoctors();
            cbDoctor.ItemsSource = doctorList;
            cbDoctor.DisplayMemberPath = "FullName";
            cbDoctor.SelectedValuePath = "UserId";
        }

        private void LoadServices()
        {
            var services = serviceService.GetAllServices();
            cbService.ItemsSource = services;
            cbService.DisplayMemberPath = "ServiceName";
            cbService.SelectedValuePath = "ServiceId";
        }

       
        private void cbService_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbService.SelectedItem is DataAccess.Models.Service selectedService)
            {
                txtDescription.Text = selectedService.Description;
                txtPrice.Text = $"{selectedService.Price:#,##0} VNĐ";
            }
            else
            {
                txtDescription.Text = "";
                txtPrice.Text = "";
            }
        }

        private void BtnBook_Click(object sender, RoutedEventArgs e)
        {
            if (cbDoctor.SelectedItem == null || cbService.SelectedItem == null || dpDate.SelectedDate == null || string.IsNullOrWhiteSpace(txtNote.Text))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin.");
                return;
            }
            DateTime selectedDate = dpDate.SelectedDate.Value.Date;
            DateTime today = DateTime.Today;

            if (selectedDate < today)
            {
                MessageBox.Show("Ngày hẹn Không phù hợp.");
                return;
            }
            int patientId = appointmentService.GetPatientIdByUserId(_currentUser.UserId);

            var appointment = new Appointment
            {
                PatientId = patientId,
                DoctorId = (int)cbDoctor.SelectedValue,
                AppointmentDate = dpDate.SelectedDate.Value,
                Note = txtNote.Text.Trim(),
                Status = "Chờ duyệt"
               
            };

            bool success = appointmentService.InsertAppointment(appointment);

            if (success)
            {
                MessageBox.Show("Đặt lịch thành công!");
                this.Close();
                PatientDashboard patientDashboard = new PatientDashboard(_currentUser);
                patientDashboard.Show();
            }
            else
            {
                MessageBox.Show("Đặt lịch thất bại!");
            }
        }

        private void BtnExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            PatientDashboard patientDashboard = new PatientDashboard(_currentUser);
            patientDashboard.Show();
        }
    }

}
