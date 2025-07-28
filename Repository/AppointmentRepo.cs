using DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{

    public class AppointmentRepo
    {
        DentalClinicDbContext _context;
        public AppointmentRepo()
        {
            _context = new();
        }
        public List<Appointment> GetAllAppointments()
        {
            return _context.Appointments.ToList();
        }
        public List<Appointment> GetAppointmentsByPatientId(int patientId)
        {
            return _context.Appointments
                .Include(a => a.Patient)
                .Where(a => a.Patient.PatientId == patientId)
                .ToList();
        }
        public void UpdateAppointmentStatus(int appointmentId, string newStatus)
        {
            var appointment = _context.Appointments.FirstOrDefault(a => a.AppointmentId == appointmentId);
            if (appointment != null)
            {
                appointment.Status = newStatus;
                _context.SaveChanges();
            }
        }
    }
}
