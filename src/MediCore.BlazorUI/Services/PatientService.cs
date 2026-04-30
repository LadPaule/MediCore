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

    public async Task<List<Patient>> GetMyPatients()
    {
        try
        {
            return await _http.GetFromJsonAsync<List<Patient>>("api/patients/me") ?? new();
        }
        catch
        {
            return new();
        }
    }

    public async Task<List<Patient>> GetPatientsByDoctor(string doctorId)
    {
        try
        {
            return await _http.GetFromJsonAsync<List<Patient>>($"api/patients/doctor/{doctorId}") ?? new();
        }
        catch
        {
            return new();
        }
    }

    public async Task<Patient?> GetPatient(Guid id)
    {
        return await _http.GetFromJsonAsync<Patient>($"api/patients/{id}");
    }

    public async Task<(bool Success, string? Error)> CreatePatient(object payload)
    {
        try
        {
            var response = await _http.PostAsJsonAsync("api/patients", payload);
            if (response.IsSuccessStatusCode) return (true, null);

            var body = await response.Content.ReadAsStringAsync();
            return (false, string.IsNullOrWhiteSpace(body) ? $"HTTP {(int)response.StatusCode}" : body);
        }
        catch (Exception ex)
        {
            return (false, ex.Message);
        }
    }

    public async Task<bool> AssignDoctor(Guid patientId, string? doctorId)
    {
        try
        {
            var response = await _http.PutAsJsonAsync(
                $"api/patients/{patientId}/assign-doctor",
                new { DoctorId = doctorId });
            return response.IsSuccessStatusCode;
        }
        catch
        {
            return false;
        }
    }
}
