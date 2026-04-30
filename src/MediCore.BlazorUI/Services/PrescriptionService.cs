using System.Net.Http.Json;

namespace MediCore.BlazorUI.Services;

public class PrescriptionService
{
    private readonly HttpClient _http;

    public PrescriptionService(HttpClient http)
    {
        _http = http;
    }

    public async Task<bool> CreateAsync(PrescriptionRequest req)
    {
        try
        {
            var res = await _http.PostAsJsonAsync("api/prescriptions", req);
            return res.IsSuccessStatusCode;
        }
        catch
        {
            return false;
        }
    }
}

public class PrescriptionRequest
{
    public Guid MedicalRecordId { get; set; }
    public string DoctorId { get; set; } = "";
    public List<PrescriptionItemRequest> Items { get; set; } = new();
}

public class PrescriptionItemRequest
{
    public string MedicationName { get; set; } = "";
    public string Dosage { get; set; } = "";
    public string Frequency { get; set; } = "";
    public int DurationDays { get; set; }
}
