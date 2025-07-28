using System;
using System.Collections.Generic;

namespace DataAccess.Models;

public partial class Patient
{
    public int PatientId { get; set; }

    public int? UserId { get; set; }

    public string? Gender { get; set; }

    public DateOnly? DateOfBirth { get; set; }

    public string? Address { get; set; }

    public string? Note { get; set; }

    public virtual ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();

    public virtual ICollection<Treatment> Treatments { get; set; } = new List<Treatment>();

    public virtual User? User { get; set; }
}
