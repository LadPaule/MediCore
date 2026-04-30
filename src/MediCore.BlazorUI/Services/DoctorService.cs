using System.Net.Http.Json;
using MediCore.BlazorUI.Models;

namespace MediCore.BlazorUI.Services;

public class DoctorService
{
    private readonly HttpClient _http;

    public DoctorService(HttpClient http)
    {
        _http = http;
    }

    public async Task<List<Doctor>> GetDoctorsAsync()
    {
        try
        {
            return await _http.GetFromJsonAsync<List<Doctor>>("api/doctors") ?? new();
        }
        catch
        {
            return new();
        }
    }

    public async Task<(bool Success, string? Error)> CreateDoctorAsync(CreateDoctorRequest req)
    {
        try
        {
            var res = await _http.PostAsJsonAsync("api/doctors", req);
            if (res.IsSuccessStatusCode)
                return (true, null);

            var body = await res.Content.ReadAsStringAsync();
            return (false, string.IsNullOrWhiteSpace(body) ? $"HTTP {(int)res.StatusCode}" : body);
        }
        catch (Exception ex)
        {
            return (false, ex.Message);
        }
    }
}
