using DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace Repository
{
    public class TreatmentRepo
    {
        private readonly DentalClinicDbContext _context;

        public TreatmentRepo(DentalClinicDbContext context)
        {
            _context = context;
        }

        public List<Treatment> GetAllTreatments()
        {
            return _context.Treatments
                .Include(t => t.Patient)
                    .ThenInclude(p => p.User)
                .Include(t => t.Doctor)
                .ToList();
        }

        public Treatment? GetTreatmentById(int treatmentId)
        {
            return _context.Treatments
                .Include(t => t.Patient)
                    .ThenInclude(p => p.User)
                .Include(t => t.Doctor)
                .FirstOrDefault(t => t.TreatmentId == treatmentId);
        }

        public bool AddTreatment(Treatment treatment)
        {
            try
            {
                _context.Treatments.Add(treatment);
                return _context.SaveChanges() > 0;
            }
            catch
            {
                return false;
            }
        }

        public bool UpdateTreatment(Treatment treatment)
        {
            try
            {
                var existing = _context.Treatments.FirstOrDefault(t => t.TreatmentId == treatment.TreatmentId);
                if (existing == null) return false;

                existing.Note = treatment.Note;
                existing.StartDate = treatment.StartDate;
                existing.EndDate = treatment.EndDate;

                existing.DoctorId = treatment.DoctorId;
                existing.PatientId = treatment.PatientId;

                return _context.SaveChanges() > 0;
            }
            catch
            {
                return false;
            }
        }

        public bool DeleteTreatment(int treatmentId)
        {
            var treatment = _context.Treatments.FirstOrDefault(t => t.TreatmentId == treatmentId);
            if (treatment == null) return false;

            _context.Treatments.Remove(treatment);
            return _context.SaveChanges() > 0;
        }
    }
}
