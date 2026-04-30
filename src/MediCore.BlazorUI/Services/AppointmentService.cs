using System.Net.Http.Json;
using MediCore.BlazorUI.Models;

namespace MediCore.BlazorUI.Services;

public class AppointmentService
{
    private readonly HttpClient _http;

    public AppointmentService(HttpClient http)
    {
        _http = http;
    }

    public async Task<List<Appointment>> GetAppointments()
    {
        try
        {
            return await _http.GetFromJsonAsync<List<Appointment>>("api/appointments") ?? new();
        }
        catch
        {
            return new();
        }
    }

    public async Task<List<Appointment>> GetMyAppointments()
    {
        try
        {
            return await _http.GetFromJsonAsync<List<Appointment>>("api/appointments/me") ?? new();
        }
        catch
        {
            return new();
        }
    }

    public async Task<List<Appointment>> GetPatientAppointments(Guid patientId)
    {
        try
        {
            return await _http.GetFromJsonAsync<List<Appointment>>($"api/appointments/patient/{patientId}") ?? new();
        }
        catch
        {
            return new();
        }
    }

    public async Task<bool> CreateAppointment(Appointment appointment)
    {
        try
        {
            var response = await _http.PostAsJsonAsync("api/appointments", appointment);
            return response.IsSuccessStatusCode;
        }
        catch
        {
            return false;
        }
    }

    public async Task<bool> Delete(Guid id)
    {
        try
        {
            var response = await _http.DeleteAsync($"api/appointments/{id}");
            return response.IsSuccessStatusCode;
        }
        catch
        {
            return false;
        }
    }
}
