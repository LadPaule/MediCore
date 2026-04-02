namespace MediCore.Domain.Entities;

public class PrescriptionItem
{
    public Guid Id {get; set;}
    public Guid PrescriptionId {get; set;}
    public string MedicationName {get; set;} = default!;
    public string Dosage {get; set;} = default!;
    public string Frequency {get; set;} = default!;
    public int DurationDays {get; set;}
    public Prescription Prescription {get; set;} = default!;
        
}



