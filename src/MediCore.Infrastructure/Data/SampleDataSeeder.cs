using MediCore.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace MediCore.Infrastructure.Data.Seed;

public static class SampleDataSeeder
{
    public static async Task SeedAsync(
        AppDbContext context,
        UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager)
    {
        await SeedDoctorsAsync(userManager, roleManager);
        await SeedPatientsAsync(context, userManager);
        await BackfillPatientAssignmentsAsync(context, userManager);
        await SeedMedicinesAsync(context);
        await SeedAppointmentsAsync(context, userManager);
    }

    private static async Task SeedDoctorsAsync(
        UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager)
    {
        if (!await roleManager.RoleExistsAsync("Doctor"))
            await roleManager.CreateAsync(new IdentityRole("Doctor"));

        var doctorSeeds = new[]
        {
            ("alice.smith@medicore.com", "Alice", "Smith"),
            ("bob.jones@medicore.com", "Bob", "Jones"),
            ("clara.lee@medicore.com", "Clara", "Lee")
        };

        foreach (var (email, first, last) in doctorSeeds)
        {
            if (await userManager.FindByEmailAsync(email) != null) continue;

            var user = new ApplicationUser
            {
                UserName = email,
                Email = email,
                FirstName = first,
                LastName = last,
                EmailConfirmed = true
            };
            var result = await userManager.CreateAsync(user, "Doctor@12345");
            if (result.Succeeded)
                await userManager.AddToRoleAsync(user, "Doctor");
        }
    }

    private static async Task SeedPatientsAsync(
        AppDbContext context,
        UserManager<ApplicationUser> userManager)
    {
        if (await context.Patients.AnyAsync()) return;

        var alice = await userManager.FindByEmailAsync("alice.smith@medicore.com");
        var bob = await userManager.FindByEmailAsync("bob.jones@medicore.com");
        var clara = await userManager.FindByEmailAsync("clara.lee@medicore.com");

        var patients = new[]
        {
            new Patient
            {
                FirstName = "John", LastName = "Doe",
                DateOfBirth = new DateTime(1990, 5, 14, 0, 0, 0, DateTimeKind.Utc),
                Gender = "Male",
                PhoneNumber = "555-0101",
                Email = "john.doe@example.com",
                Address = "123 Main St",
                AssignedDoctorId = alice?.Id
            },
            new Patient
            {
                FirstName = "Jane", LastName = "Doe",
                DateOfBirth = new DateTime(1992, 9, 23, 0, 0, 0, DateTimeKind.Utc),
                Gender = "Female",
                PhoneNumber = "555-0102",
                Email = "jane.doe@example.com",
                Address = "123 Main St",
                AssignedDoctorId = alice?.Id
            },
            new Patient
            {
                FirstName = "Maria", LastName = "Garcia",
                DateOfBirth = new DateTime(1985, 1, 7, 0, 0, 0, DateTimeKind.Utc),
                Gender = "Female",
                PhoneNumber = "555-0103",
                Email = "maria.garcia@example.com",
                Address = "456 Oak Ave",
                AssignedDoctorId = bob?.Id
            },
            new Patient
            {
                FirstName = "Ahmed", LastName = "Khan",
                DateOfBirth = new DateTime(1978, 11, 30, 0, 0, 0, DateTimeKind.Utc),
                Gender = "Male",
                PhoneNumber = "555-0104",
                Email = "ahmed.khan@example.com",
                Address = "78 River Rd",
                AssignedDoctorId = bob?.Id
            },
            new Patient
            {
                FirstName = "Sara", LastName = "Nguyen",
                DateOfBirth = new DateTime(2001, 3, 18, 0, 0, 0, DateTimeKind.Utc),
                Gender = "Female",
                PhoneNumber = "555-0105",
                Email = "sara.nguyen@example.com",
                Address = "9 Birch Ln",
                AssignedDoctorId = clara?.Id
            }
        };

        context.Patients.AddRange(patients);
        await context.SaveChangesAsync();
    }

    // For existing databases that already have patients but no doctor assignment.
    // We try to derive an assignment from any existing appointment so doctor-scoped
    // views show data instead of being empty after the migration.
    private static async Task BackfillPatientAssignmentsAsync(
        AppDbContext context,
        UserManager<ApplicationUser> userManager)
    {
        var unassigned = await context.Patients
            .Where(p => p.AssignedDoctorId == null)
            .ToListAsync();

        if (unassigned.Count == 0) return;

        var fallbackDoctor = await userManager.FindByEmailAsync("alice.smith@medicore.com");

        foreach (var patient in unassigned)
        {
            var doctorIdFromAppointment = await context.Appointments
                .Where(a => a.PatientId == patient.Id)
                .OrderBy(a => a.AppointmentDate)
                .Select(a => a.DoctorId)
                .FirstOrDefaultAsync();

            patient.AssignedDoctorId = !string.IsNullOrEmpty(doctorIdFromAppointment)
                ? doctorIdFromAppointment
                : fallbackDoctor?.Id;
        }

        await context.SaveChangesAsync();
    }

    private static async Task SeedMedicinesAsync(AppDbContext context)
    {
        if (await context.Medications.AnyAsync()) return;

        var now = DateTime.UtcNow.Date;
        var meds = new[]
        {
            new Medication
            {
                Name = "Paracetamol 500mg",
                StockQuantity = 250, ReorderLevel = 100,
                Price = 0.50m,
                ExpiryDate = now.AddYears(2),
                Description = "Pain relief and fever reducer",
                Manufacturer = "GenericPharma"
            },
            new Medication
            {
                Name = "Amoxicillin 500mg",
                StockQuantity = 80, ReorderLevel = 100,
                Price = 1.20m,
                ExpiryDate = now.AddMonths(18),
                Description = "Broad-spectrum antibiotic",
                Manufacturer = "MedPlus"
            },
            new Medication
            {
                Name = "Ibuprofen 200mg",
                StockQuantity = 500, ReorderLevel = 150,
                Price = 0.30m,
                ExpiryDate = now.AddYears(1),
                Description = "NSAID anti-inflammatory",
                Manufacturer = "GenericPharma"
            },
            new Medication
            {
                Name = "Lisinopril 10mg",
                StockQuantity = 40, ReorderLevel = 50,
                Price = 0.95m,
                ExpiryDate = now.AddDays(20),
                Description = "ACE inhibitor for hypertension",
                Manufacturer = "CardioCo"
            },
            new Medication
            {
                Name = "Metformin 500mg",
                StockQuantity = 320, ReorderLevel = 100,
                Price = 0.40m,
                ExpiryDate = now.AddYears(2),
                Description = "Type 2 diabetes treatment",
                Manufacturer = "MedPlus"
            },
            new Medication
            {
                Name = "Atorvastatin 20mg",
                StockQuantity = 60, ReorderLevel = 80,
                Price = 1.10m,
                ExpiryDate = now.AddMonths(10),
                Description = "Statin for cholesterol",
                Manufacturer = "CardioCo"
            }
        };

        var stockTxs = meds.Select(m => new StockTransaction
        {
            MedicationId = m.Id,
            QuantityChanged = m.StockQuantity,
            TransactionType = "InitialStock",
            CreatedAt = DateTime.UtcNow
        });

        context.Medications.AddRange(meds);
        context.StockTransactions.AddRange(stockTxs);
        await context.SaveChangesAsync();
    }

    private static async Task SeedAppointmentsAsync(
        AppDbContext context,
        UserManager<ApplicationUser> userManager)
    {
        if (await context.Appointments.AnyAsync()) return;

        var patients = await context.Patients.Take(3).ToListAsync();
        if (patients.Count == 0) return;

        var alice = await userManager.FindByEmailAsync("alice.smith@medicore.com");
        if (alice == null) return;

        var now = DateTime.UtcNow;
        var appointments = new[]
        {
            new Appointment
            {
                PatientId = patients[0].Id,
                DoctorId = alice.Id,
                AppointmentDate = now.AddDays(1),
                Reason = "Annual physical",
                Status = "Confirmed"
            },
            new Appointment
            {
                PatientId = patients[Math.Min(1, patients.Count - 1)].Id,
                DoctorId = alice.Id,
                AppointmentDate = now.AddDays(3),
                Reason = "Follow-up consultation",
                Status = "Pending"
            },
            new Appointment
            {
                PatientId = patients[Math.Min(2, patients.Count - 1)].Id,
                DoctorId = alice.Id,
                AppointmentDate = now.AddDays(-7),
                Reason = "Blood pressure check",
                Status = "Completed"
            }
        };

        context.Appointments.AddRange(appointments);
        await context.SaveChangesAsync();
    }
}
