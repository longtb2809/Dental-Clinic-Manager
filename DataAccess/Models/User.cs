using System;
using System.Collections.Generic;

namespace DataAccess.Models;

public partial class User
{
    public int UserId { get; set; }

    public string Username { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public string? FullName { get; set; }

    public string? Email { get; set; }

    public string? Phone { get; set; }

    public int? RoleId { get; set; }

    public bool? IsActive { get; set; }

    public string? Gender { get; set; }

    public DateOnly? DateOfBirth { get; set; }

    public string? Address { get; set; }

    public virtual ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();

    public virtual Patient? Patient { get; set; }

    public virtual Role? Role { get; set; }

    public virtual ICollection<Treatment> Treatments { get; set; } = new List<Treatment>();
}
