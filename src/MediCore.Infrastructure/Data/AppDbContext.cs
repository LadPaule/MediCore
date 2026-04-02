using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using MediCore.Domain.Entities;

namespace  MediCore.Infrastructure.Data;

public class AppDbContext : IdentityDbContext<ApplicationUser>
{
    public  AppDbContext(DbContextOptions<AppDbContext> options) 
        : base(options)
    {
        
    }
    // Todo: Hosipital Tables
    public DbSet <Patient> Patients {get; set;}
    public DbSet <Doctor> Doctors {get; set;}
    public DbSet <Department> Departments {get; set;}
    public DbSet <Appointment> Appointments  {get; set;}
    public DbSet <MedicalRecord> MedicalRecords  {get; set;}
    public DbSet <Prescription> Prescriptions  {get; set;}
    public DbSet <PrescriptionItem> PrescriptionItems  {get; set;}
    public DbSet <Medication> Medications  {get; set;}
    public DbSet <PharmacyInventory> PharmacyInventories  {get; set;}
    public DbSet <StockTransaction> StockTransactions  {get; set;}
    public DbSet <DispensedMedication> DispensedMedications  {get; set;}

    public DbSet<AppUser> AppUsers { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.Entity<Medication>()
            .Property(m => m.Price)
            .HasPrecision(18, 2);

        builder.Entity<Medication>()
            .HasIndex(m => m.Name);
    }
}

