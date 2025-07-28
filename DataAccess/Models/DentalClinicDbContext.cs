using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;

namespace DataAccess.Models;

public partial class DentalClinicDbContext : DbContext
{
    public DentalClinicDbContext()
    {
    }

    public DentalClinicDbContext(DbContextOptions<DentalClinicDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Appointment> Appointments { get; set; }

    public virtual DbSet<Patient> Patients { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Service> Services { get; set; }

    public virtual DbSet<Treatment> Treatments { get; set; }

    public virtual DbSet<TreatmentDetail> TreatmentDetails { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();

        var connectionString = configuration.GetConnectionString("DBDefault");
        optionsBuilder.UseSqlServer(connectionString);
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Appointment>(entity =>
        {
            entity.HasKey(e => e.AppointmentId).HasName("PK__Appointm__8ECDFCA230134FA4");

            entity.Property(e => e.AppointmentId).HasColumnName("AppointmentID");
            entity.Property(e => e.AppointmentDate).HasColumnType("datetime");
            entity.Property(e => e.DoctorId).HasColumnName("DoctorID");
            entity.Property(e => e.Note).HasMaxLength(255);
            entity.Property(e => e.PatientId).HasColumnName("PatientID");
            entity.Property(e => e.Status).HasMaxLength(20);

            entity.HasOne(d => d.Doctor).WithMany(p => p.Appointments)
                .HasForeignKey(d => d.DoctorId)
                .HasConstraintName("FK__Appointme__Docto__31EC6D26");

            entity.HasOne(d => d.Patient).WithMany(p => p.Appointments)
                .HasForeignKey(d => d.PatientId)
                .HasConstraintName("FK__Appointme__Patie__30F848ED");
        });

        modelBuilder.Entity<Patient>(entity =>
        {
            entity.HasKey(e => e.PatientId).HasName("PK__Patients__970EC3461511339D");

            entity.HasIndex(e => e.UserId, "UQ__Patients__1788CCAD8CF741C0").IsUnique();

            entity.Property(e => e.PatientId).HasColumnName("PatientID");
            entity.Property(e => e.Address).HasMaxLength(255);
            entity.Property(e => e.Gender).HasMaxLength(10);
            entity.Property(e => e.Note).HasMaxLength(255);
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.User).WithOne(p => p.Patient)
                .HasForeignKey<Patient>(d => d.UserId)
                .HasConstraintName("FK__Patients__UserID__2C3393D0");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.RoleId).HasName("PK__Roles__8AFACE3A743C751D");

            entity.Property(e => e.RoleId)
                .ValueGeneratedNever()
                .HasColumnName("RoleID");
            entity.Property(e => e.RoleName).HasMaxLength(50);
        });

        modelBuilder.Entity<Service>(entity =>
        {
            entity.HasKey(e => e.ServiceId).HasName("PK__Services__C51BB0EA73311AFF");

            entity.Property(e => e.ServiceId).HasColumnName("ServiceID");
            entity.Property(e => e.Description).HasMaxLength(255);
            entity.Property(e => e.Price).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.ServiceName).HasMaxLength(100);
        });

        modelBuilder.Entity<Treatment>(entity =>
        {
            entity.HasKey(e => e.TreatmentId).HasName("PK__Treatmen__1A57B71111E912D8");

            entity.Property(e => e.TreatmentId).HasColumnName("TreatmentID");
            entity.Property(e => e.DoctorId).HasColumnName("DoctorID");
            entity.Property(e => e.EndDate).HasColumnType("datetime");
            entity.Property(e => e.Note).HasMaxLength(255);
            entity.Property(e => e.PatientId).HasColumnName("PatientID");
            entity.Property(e => e.StartDate).HasColumnType("datetime");

            entity.HasOne(d => d.Doctor).WithMany(p => p.Treatments)
                .HasForeignKey(d => d.DoctorId)
                .HasConstraintName("FK__Treatment__Docto__36B12243");

            entity.HasOne(d => d.Patient).WithMany(p => p.Treatments)
                .HasForeignKey(d => d.PatientId)
                .HasConstraintName("FK__Treatment__Patie__35BCFE0A");
        });

        modelBuilder.Entity<TreatmentDetail>(entity =>
        {
            entity.HasKey(e => e.DetailId).HasName("PK__Treatmen__135C314DD42E8E32");

            entity.Property(e => e.DetailId).HasColumnName("DetailID");
            entity.Property(e => e.Description).HasMaxLength(255);
            entity.Property(e => e.Price).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.ServiceId).HasColumnName("ServiceID");
            entity.Property(e => e.ToothPosition).HasMaxLength(20);
            entity.Property(e => e.TreatmentDate).HasColumnType("datetime");
            entity.Property(e => e.TreatmentId).HasColumnName("TreatmentID");

            entity.HasOne(d => d.Service).WithMany(p => p.TreatmentDetails)
                .HasForeignKey(d => d.ServiceId)
                .HasConstraintName("FK__Treatment__Servi__3A81B327");

            entity.HasOne(d => d.Treatment).WithMany(p => p.TreatmentDetails)
                .HasForeignKey(d => d.TreatmentId)
                .HasConstraintName("FK__Treatment__Treat__398D8EEE");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Users__1788CCAC32B6916A");

            entity.HasIndex(e => e.Username, "UQ__Users__536C85E4B804758E").IsUnique();

            entity.Property(e => e.UserId).HasColumnName("UserID");
            entity.Property(e => e.Address).HasMaxLength(255);
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.FullName).HasMaxLength(100);
            entity.Property(e => e.Gender).HasMaxLength(10);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.PasswordHash).HasMaxLength(255);
            entity.Property(e => e.Phone).HasMaxLength(20);
            entity.Property(e => e.RoleId).HasColumnName("RoleID");
            entity.Property(e => e.Username).HasMaxLength(50);

            entity.HasOne(d => d.Role).WithMany(p => p.Users)
                .HasForeignKey(d => d.RoleId)
                .HasConstraintName("FK__Users__RoleID__276EDEB3");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
