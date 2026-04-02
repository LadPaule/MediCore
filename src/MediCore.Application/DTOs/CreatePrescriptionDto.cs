using MediCore.Domain.Entities;

namespace MediCore.Application.DTOs;


// Todo: Create the Prescription Data Transfer Objects
public class CreatePrescriptionDto
{
    public Guid MedicalReordId {get; set;}
    public string DoctorId {get; set;} = default!;
    public List<PrescriptionItemDto> Items {get; set;} = new();

}

public class PrescriptionItemDto
{
    public string MedicationName {get; set;} = default!;
    public string Dosage {get; set;} = default!;
    public string Frequency {get; set;} = default!;
    public int DurationDays {get; set;}


}

