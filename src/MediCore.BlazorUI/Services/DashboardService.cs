using System.Net.Http.Json;

namespace MediCore.BlazorUI.Services;

public class DashboardService
{
    private readonly HttpClient _http;

    public DashboardService(HttpClient http)
    {
        _http = http;
    }

    public async Task<int> GetPatientCount()
    {
        try
        {
            return await _http.GetFromJsonAsync<int>("api/dashboard/patients");
        }
        catch
        {
            return 0;
        }
    }

    public async Task<int> GetTodayAppointments()
    {
        try
        {
            return await _http.GetFromJsonAsync<int>("api/dashboard/appointments/today");
        }
        catch
        {
            return 0;
        }
    }
}
