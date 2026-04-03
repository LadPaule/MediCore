using System.Net.Http.Json;
using MediCore.BlazorUI.Models;

namespace MediCore.BlazorUI.Services;

public class PatientService
{
    private readonly HttpClient _http;

    public PatientService(HttpClient http)
    {
        _http = http;
    }

    public async Task<List<Patient>?> GetPatients()
    {
        return await _http.GetFromJsonAsync<List<Patient>>("api/patients");
    }

    public async Task<Patient?> GetPatient(Guid id)
    {
        return await _http.GetFromJsonAsync<Patient>($"api/patients/{id}");
    }

    public async Task CreatePatient(Patient patient)
    {
        await _http.PostAsJsonAsync("api/patients", patient);
    }
}



