using System.Net.Http.Json;

public class AppointmentService
{
    private readonly HttpClient _http;

    public AppointmentService(HttpClient http)
    {
        _http = http;
    }

    public async Task<List<Appointment>?> GetAppointments()
    {
        return await _http.GetFromJsonAsync<List<Appointment>>("api/appointments");
    }

    public async Task CreateAppointment(Appointment appointment)
    {
        await _http.PostAsJsonAsync("api/appointments", appointment);
    }

    public async Task Delete(Guid id)
    {
        await _http.DeleteAsync($"api/appointments/{id}");
    }
}




