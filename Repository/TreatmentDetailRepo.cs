using DataAccess.Models;
using Microsoft.EntityFrameworkCore;

namespace Repository;

public class TreatmentDetailRepo
{
    private readonly DentalClinicDbContext _context;

    public TreatmentDetailRepo()
    {
        _context = new DentalClinicDbContext();
    }

    public TreatmentDetail GetTreatmentDetailByTreatmentId(int treatmentId)
    {
        return _context.TreatmentDetails
            .Include(td => td.Treatment)
                .ThenInclude(t => t.Patient)
                    .ThenInclude(p => p.User) 
            .Include(td => td.Treatment)
                .ThenInclude(t => t.Doctor)
            .Include(td => td.Service)
            .FirstOrDefault(td => td.TreatmentId == treatmentId);
    }


}

