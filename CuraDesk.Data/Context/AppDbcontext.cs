using System;
using System.Collections.Generic;
using System.Text;
using CuraDesk.Model.Entities;
using Microsoft.EntityFrameworkCore;
namespace CuraDesk.Data.Context
{
    public class AppDbContext:DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext>options):base(options)
        {

        }
        public DbSet<User> Users { get; set; }
        public DbSet<Appointments> Appointments { get; set; }
        public DbSet<DoctorAvailability> DoctorAvailability { get; set; }
        public DbSet<MedicalReport>MedicalReports { get; set; }
        public DbSet<PatientProfile> PatientProfiles { get; set; }
        public DbSet<DoctorProfile> DoctorProfiles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<User>()
                .HasIndex(u => u.EmailId)
                .IsUnique();

            modelBuilder.Entity<Appointments>()
                .HasOne(a => a.Patient)
                .WithMany()
                .HasForeignKey(a => a.PatientUserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Appointments>()
                .HasOne(a => a.Doctor)
                .WithMany()
                .HasForeignKey(a => a.DoctorId)
                .OnDelete(DeleteBehavior.Restrict);
        }

    }
}
