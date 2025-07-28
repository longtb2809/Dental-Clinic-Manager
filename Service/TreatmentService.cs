using DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using Repository;
using System.Collections.Generic;
using System.Linq;

public class TreatmentService
{
    private readonly TreatmentRepo _treatmentRepo;
    private readonly UserRepo _userRepo;
    private readonly DentalClinicDbContext _context;

    public TreatmentService()
    {
        _context = new DentalClinicDbContext();
        _treatmentRepo = new TreatmentRepo(_context);
        _userRepo = new UserRepo(_context);
    }

    public List<Treatment> GetAllTreatments()
    {
        return _treatmentRepo.GetAllTreatments();
    }

    public List<Treatment> GetTreatmentsByPatientId(int patientId)
    {
        return _context.Treatments
            .Include(t => t.Doctor)
            .Include(t => t.Patient)
                .ThenInclude(p => p.User)
            .Where(t => t.PatientId == patientId)
            .ToList();
    }

    public int GetPatientIdByUserId(int userId)
    {
        var patient = _context.Patients.FirstOrDefault(p => p.UserId == userId);
        return patient?.PatientId ?? -1;
    }

    public List<Treatment> GetTreatmentsByDoctorId(int doctorUserId)
    {
        return _context.Treatments
            .Include(t => t.Patient)
                .ThenInclude(p => p.User)
            .Where(t => t.DoctorId == doctorUserId)
            .ToList();
    }
}
