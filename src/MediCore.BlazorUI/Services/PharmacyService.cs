using System.Net.Http.Json;
using MediCore.BlazorUI.Models.Pharmacy;

namespace MediCore.BlazorUI.Services;

public class PharmacyService
{
    private readonly HttpClient _http;

    public PharmacyService(HttpClient http)
    {
        _http = http;
    }

    public async Task<PharmacyStatsVM?> GetStatsAsync()
        => await _http.GetFromJsonAsync<PharmacyStatsVM>("api/pharmacy/stats");

    public async Task<List<MedicineVM>> GetMedicinesAsync()
        => await _http.GetFromJsonAsync<List<MedicineVM>>("api/pharmacy/medicines") ?? new();

    public async Task<List<MedicineVM>> GetLowStockAsync()
        => await _http.GetFromJsonAsync<List<MedicineVM>>("api/pharmacy/low-stock") ?? new();

    public async Task<List<MedicineVM>> GetExpiringAsync()
        => await _http.GetFromJsonAsync<List<MedicineVM>>("api/pharmacy/expiring") ?? new();

    public async Task<bool> DispenseAsync(DispenseMedicineVM dto)
    {
        var res = await _http.PostAsJsonAsync("api/pharmacy/dispense", dto);
        return res.IsSuccessStatusCode;
    }
    public async Task <List<MedicineVM>> GetInventory()
    {
        return await _http.GetFromJsonAsync<List<MedicineVM>>("api/pharmacy/medicines")
            ?? new List<MedicineVM>();
    }

    public async Task DeleteMedicineAsync(int id)
        => await _http.DeleteAsync($"api/pharmacy/medicines/{id}");
}



















// using System.Net.Http.Json;
// 
// public class PharmacyService
// {
//     private readonly HttpClient _http;
//     public PharmacyService(HttpClient http)
//     {
//         _http = http;
//     }
// 
//     public async Task<List<PharmacyInventory>?> GetInventory()
//     {
//         return await _http.GetFromJsonAsync<List<PharmacyInventory>>("api/pharmacy/inventory");
//     }
// 
//     public async Task DispenseMedication(Guid prescriptionId)
//     {
//         await _http.PostAsJsonAsync("api/pharmacy/dispense", new
//         {
//             prescriptionId = prescriptionId
//         });
//     }
// }














