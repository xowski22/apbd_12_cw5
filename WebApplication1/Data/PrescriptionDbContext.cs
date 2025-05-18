using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;
namespace WebApplication1.Data;

public class PrescriptionDbContext : DbContext
{
    public PrescriptionDbContext(DbContextOptions<PrescriptionDbContext> options) : base(options) { }
    
    public DbSet<Patient> Patients { get; set; }
    public DbSet<Doctor> Doctors { get; set; }
    public DbSet<Prescription> Prescriptions { get; set; }
    public DbSet<Medicament> Medicaments { get; set; }
    public DbSet<Prescription_Medicament> PrescriptionMedicaments { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Prescription_Medicament>()
            .HasKey(pm => new { pm.IdMedicament, pm.IdPrescription });
        
        modelBuilder.Entity<Prescription_Medicament>()
            .HasOne(pm => pm.Medicament)
            .WithMany(m => m.Prescription_Medicaments)
            .HasForeignKey(pm => pm.IdMedicament)
            .OnDelete(DeleteBehavior.Restrict);
        
        modelBuilder.Entity<Prescription_Medicament>()
            .HasOne(pm => pm.Prescription)
            .WithMany(p => p.Prescription_Medicaments)
            .HasForeignKey(pm => pm.IdPrescription)
            .OnDelete(DeleteBehavior.Restrict);
        
        modelBuilder.Entity<Prescription>()
            .HasOne(p => p.Patient)
            .WithMany(pt => pt.Prescriptions)
            .HasForeignKey(p => p.IdPatient)
            .OnDelete(DeleteBehavior.Restrict);
        
        modelBuilder.Entity<Prescription>()
            .HasOne(p => p.Doctor)
            .WithMany(d => d.Prescriptions)
            .HasForeignKey(p => p.IdDoctor)
            .OnDelete(DeleteBehavior.Restrict);
            
    }
}