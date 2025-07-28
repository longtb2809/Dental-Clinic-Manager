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

namespace DentalClinic
{
    /// <summary>
    /// Interaction logic for ConfirmAppointmentWindow.xaml
    /// </summary>
    public partial class ConfirmAppointmentWindow : Window
    {
        private Appointment _appointment;
        private AppointmentService appointmentService;

        public ConfirmAppointmentWindow(Appointment appointment)
        {
            InitializeComponent();
            _appointment = appointment;

            this.DataContext = _appointment;
        }

        private void BtnConfirm_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                appointmentService = new AppointmentService();
                appointmentService.UpdateAppointmentStatus(_appointment.AppointmentId, "Đã đặt");

                MessageBox.Show("✅ Lịch hẹn đã được xác nhận!", "Thành công", MessageBoxButton.OK, MessageBoxImage.Information);
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("❌ Lỗi khi xác nhận lịch hẹn: " + ex.Message);
            }
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Bạn có chắc chắn muốn hủy lịch hẹn này?", "Xác nhận", MessageBoxButton.YesNo, MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    appointmentService = new AppointmentService();
                    appointmentService.UpdateAppointmentStatus(_appointment.AppointmentId, "Đã hủy");

                    MessageBox.Show("❌ Lịch hẹn đã bị hủy!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                    this.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi hủy lịch hẹn: " + ex.Message);
                }
            }
        }
    }
   }
