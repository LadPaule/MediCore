namespace MediCore.Domain.Entities;

public class Prescription
{
    public Guid Id {get; set;}
    public Guid MedicalRecordId {get; set;}
    public string DoctorId {get; set;} = default!;
    public DateTime CreatedAt {get; set;} = DateTime.UtcNow;
    public MedicalRecord MedicalRecord {get; set;} = default!;
    public ApplicationUser Doctor {get; set;} = default!;
    public List<PrescriptionItem> Items {get; set;} = new();

}



