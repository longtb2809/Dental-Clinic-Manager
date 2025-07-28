using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess.Models;

public partial class Treatment
{
    public int TreatmentId { get; set; }

    public int? PatientId { get; set; }

    public int? DoctorId { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public string? Note { get; set; }

    public virtual User? Doctor { get; set; }

    public virtual Patient? Patient { get; set; }

    public virtual ICollection<TreatmentDetail> TreatmentDetails { get; set; } = new List<TreatmentDetail>();

    [NotMapped]
    public string PatientName => Patient?.User?.FullName ?? "Không rõ";

    [NotMapped]
    public string DoctorName => Doctor?.FullName ?? "Không rõ";
}
