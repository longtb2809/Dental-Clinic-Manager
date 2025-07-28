using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess.Models;

public partial class Appointment
{
    public int AppointmentId { get; set; }

    public int? PatientId { get; set; }

    public int? DoctorId { get; set; }

    public DateTime? AppointmentDate { get; set; }

    public string? Status { get; set; }

    public string? Note { get; set; }

    public virtual User? Doctor { get; set; }

    public virtual Patient? Patient { get; set; }

    [NotMapped] 
    public string PatientName => Patient?.User?.FullName ?? "Không rõ";


    [NotMapped] 
    public string DoctorName => Doctor?.FullName ?? "Không rõ";
}
