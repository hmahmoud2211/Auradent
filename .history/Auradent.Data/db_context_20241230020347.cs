using Auradent.core;
using Microsoft.EntityFrameworkCore;
using MySql.Data.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySQL.Data.EntityFrameworkCore;

namespace Auradent.Data
{
    public class db_context : DbContext
    {
        public DbSet<Employee> Employees { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var serverVersion = new MySqlServerVersion(new Version(8, 0, 21));
            optionsBuilder.UseMySql("server=localhost;database=auradent_db_final;user=root;password=Aabbcc_112233", serverVersion);
        }


        public DbSet<DoctorandNurse> DoctorandNurses { get; set; }
        public DbSet<Medicine> Medicine { get; set; }
        public DbSet<Patient> Patient { get; set; }
        public DbSet<Prescription> Prescription { get; set; }
        public DbSet<Appointment> Appointment { get; set; }
        public DbSet<Finance> Finance { get; set; }
        public DbSet<Medical_Record> medical_Records { get; set; }
        public DbSet<RadiologyORtest> radiologyORtests { get; set; }
        public DbSet<Visit_Record> Visit_Records { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Employee>().HasKey(a => a.ID);
            modelBuilder.Entity<DoctorandNurse>().HasKey(a => a.ID);
            modelBuilder.Entity<Patient>().HasKey(p => p.PatientID);
            modelBuilder.Entity<Prescription>().HasKey(p => p.PrescriptionID);
            modelBuilder.Entity<Appointment>().HasKey(p => p.AppointmentID);
            modelBuilder.Entity<Finance>().HasKey(p => p.FinanceId);
            modelBuilder.Entity<Medical_Record>().HasKey(p => p.RecordId);
            modelBuilder.Entity<RadiologyORtest>().HasKey(p => p.RadiologyORtestID);
            modelBuilder.Entity<Visit_Record>().HasKey(p => p.VisitId);

            modelBuilder.Entity<Appointment>()
                .HasOne(a => a.Patient)
                .WithMany(p => p.Appointments)
                .HasForeignKey(a => a.PatientID_FK);

            modelBuilder.Entity<Appointment>()
                .HasOne(a => a.DoctorandNurse)
                .WithMany(p => p.Appointments)
                .HasForeignKey(a => a.DoctorandNurseID_FK);

            modelBuilder.Entity<Appointment>()
                .HasOne(a => a.Finance)
                .WithOne(p => p.Appointments)
                .HasForeignKey<Appointment>(a => a.Fainance_Fk);

            modelBuilder.Entity<Prescription>()
                .HasOne(a => a.DoctorandNurse)
                .WithMany(p => p.Prescriptions)
                .HasForeignKey(a => a.DoctorandNursID_FK);

            modelBuilder.Entity<Prescription>()
                .HasOne(a => a.Patient)
                .WithMany(p => p.Prescriptions)
                .HasForeignKey(a => a.PatientID_FK);

            modelBuilder.Entity<Patient>()
                .HasOne(a => a.Medical_Record)
                .WithOne(p => p.Patient)
                .HasForeignKey<Patient>(a => a.MedicalRecordID);

            modelBuilder.Entity<RadiologyORtest>()
               .HasOne(a => a.Medical_Records)
               .WithMany(p => p.RadiologyORtests)
               .HasForeignKey(a => a.MedicalRecordID);

            modelBuilder.Entity<Visit_Record>()
                .HasOne(v => v.Patient)
                .WithMany()
                .HasForeignKey(v => v.PatientID_FK);
        }


    }
}