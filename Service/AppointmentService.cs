using DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Service
{
    public class AppointmentService
    {
        AppointmentRepo _appointmentrepo;
        public AppointmentService()
        {
            _appointmentrepo = new();
        }
        public List<Appointment> GetAllAppointments()
        {
            using (var context = new DentalClinicDbContext())
            {
                return context.Appointments
                    .Include(a => a.Patient)
                        .ThenInclude(p => p.User)
                    .Include(a => a.Doctor)
                    .ToList();
            }
        }


        public List<Appointment> GetAppointmentsByPatientId(int patientId)
        {
            using (var context = new DentalClinicDbContext())
            {
                return context.Appointments
                    .Include(a => a.Doctor)
                    .Include(a => a.Patient)
                        .ThenInclude(p => p.User)
                    .Where(a => a.PatientId == patientId)
                    .ToList();
            }
        }
        public int GetPatientIdByUserId(int userId)
        {
            using var db = new DentalClinicDbContext();
            var patient = db.Patients.FirstOrDefault(p => p.UserId == userId);
            return patient?.PatientId ?? -1; // -1 nếu không tìm thấy
        }
        public List<Appointment> GetAppointmentsByDoctorId(int doctorUserId)
        {
            using (var context = new DentalClinicDbContext())
            {
                return context.Appointments
                    .Include(a => a.Patient)
                        .ThenInclude(p => p.User)
                    .Where(a => a.DoctorId == doctorUserId)
                    .ToList();
            }
        }
        public int GetDoctorIdByUserId(int userId)
        {
            using (var context = new DentalClinicDbContext())
            {
                var user = context.Users.FirstOrDefault(u => u.UserId == userId && u.RoleId == 2);
                return user != null ? user.UserId : 0;
            }
        }
        public bool InsertAppointment(Appointment a)
        {
            using (var context = new DentalClinicDbContext())
            {
                context.Appointments.Add(a);
                context.SaveChanges();
                return true;
            }
        }
        
        public bool UpdateAppointment(Appointment appointment)
        {
            using (var context = new DentalClinicDbContext())
            {
                var existingAppointment = context.Appointments
                    .Include(a => a.Patient)
                    .Include(a => a.Doctor)
                    .FirstOrDefault(a => a.AppointmentId == appointment.AppointmentId);
                
                if (existingAppointment != null)
                {
                    existingAppointment.PatientId = appointment.PatientId;
                    existingAppointment.DoctorId = appointment.DoctorId;
                    existingAppointment.AppointmentDate = appointment.AppointmentDate;
                    existingAppointment.Status = appointment.Status;
                    existingAppointment.Note = appointment.Note;
                    
                    context.SaveChanges();
                    return true;
                }
                return false;
            }
        }
        
        public bool DeleteAppointment(int appointmentId)
        {
            using (var context = new DentalClinicDbContext())
            {
                var appointment = context.Appointments.FirstOrDefault(a => a.AppointmentId == appointmentId);
                if (appointment != null)
                {
                    context.Appointments.Remove(appointment);
                    context.SaveChanges();
                    return true;
                }
                return false;
            }
        }
        
        public Appointment GetAppointmentById(int appointmentId)
        {
            using (var context = new DentalClinicDbContext())
            {
                return context.Appointments
                    .Include(a => a.Patient)
                        .ThenInclude(p => p.User)
                    .Include(a => a.Doctor)
                    .FirstOrDefault(a => a.AppointmentId == appointmentId);
            }
        }
        
        public void UpdateAppointmentStatus(int appointmentId, string newStatus)
        {
            _appointmentrepo.UpdateAppointmentStatus(appointmentId, newStatus);
        }
    }
}
