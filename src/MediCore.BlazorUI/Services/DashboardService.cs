public class DashboardService
{
    private readonly HttpClient _http;
    public DashboardService(HttpClient http)
    {
        _http = http;
    }

    public async Task<int> GetPatientCount()
    {
        return await _http.GetFromJsonAsync<int>("api/dashboard/patients");

    }
    public async Task<int> GetTodayAppointments()
    {
        return await _http.GetFromJsonAsync<int>("api/dashboard/appointments/today");
    }

    // public async Task<DashboardStats> GetStats()
    // {
    //     return await _http.GetFromJsonAsync<DashboardStats>("api/dasboard") ?? new DashboardStats();
    // }
}


