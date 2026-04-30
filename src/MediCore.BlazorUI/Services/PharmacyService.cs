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
    {
        try
        {
            return await _http.GetFromJsonAsync<PharmacyStatsVM>("api/pharmacy/stats");
        }
        catch
        {
            return null;
        }
    }

    public async Task<List<MedicineVM>> GetMedicinesAsync()
    {
        try
        {
            return await _http.GetFromJsonAsync<List<MedicineVM>>("api/pharmacy/medicines") ?? new();
        }
        catch
        {
            return new();
        }
    }

    public async Task<List<MedicineVM>> GetLowStockAsync()
    {
        try
        {
            return await _http.GetFromJsonAsync<List<MedicineVM>>("api/pharmacy/low-stock") ?? new();
        }
        catch
        {
            return new();
        }
    }

    public async Task<List<MedicineVM>> GetExpiringAsync()
    {
        try
        {
            return await _http.GetFromJsonAsync<List<MedicineVM>>("api/pharmacy/expiring") ?? new();
        }
        catch
        {
            return new();
        }
    }

    public async Task<bool> DispenseAsync(DispenseMedicineVM dto)
    {
        try
        {
            var res = await _http.PostAsJsonAsync("api/pharmacy/dispense", dto);
            return res.IsSuccessStatusCode;
        }
        catch
        {
            return false;
        }
    }

    public async Task<List<MedicineVM>> GetInventory()
    {
        try
        {
            return await _http.GetFromJsonAsync<List<MedicineVM>>("api/pharmacy/medicines") ?? new();
        }
        catch
        {
            return new();
        }
    }

    public async Task<bool> CreateMedicineAsync(CreateMedicineVM dto)
    {
        try
        {
            var res = await _http.PostAsJsonAsync("api/pharmacy/medicines", dto);
            return res.IsSuccessStatusCode;
        }
        catch
        {
            return false;
        }
    }

    public async Task<MedicineDetailVM?> GetMedicineAsync(Guid id)
    {
        try
        {
            return await _http.GetFromJsonAsync<MedicineDetailVM>($"api/pharmacy/medicines/{id}");
        }
        catch
        {
            return null;
        }
    }

    public async Task<(bool Success, string? Error)> RestockAsync(Guid id, RestockVM dto)
    {
        try
        {
            var res = await _http.PostAsJsonAsync($"api/pharmacy/medicines/{id}/restock", dto);
            if (res.IsSuccessStatusCode) return (true, null);
            return (false, await res.Content.ReadAsStringAsync());
        }
        catch (Exception ex)
        {
            return (false, ex.Message);
        }
    }

    public async Task<(bool Success, string? Error)> AdjustStockAsync(Guid id, AdjustStockVM dto)
    {
        try
        {
            var res = await _http.PostAsJsonAsync($"api/pharmacy/medicines/{id}/adjust", dto);
            if (res.IsSuccessStatusCode) return (true, null);
            return (false, await res.Content.ReadAsStringAsync());
        }
        catch (Exception ex)
        {
            return (false, ex.Message);
        }
    }

    public async Task<(bool Success, string? Error)> UpdateMedicineAsync(Guid id, UpdateMedicineVM dto)
    {
        try
        {
            var res = await _http.PutAsJsonAsync($"api/pharmacy/medicines/{id}", dto);
            if (res.IsSuccessStatusCode) return (true, null);
            return (false, await res.Content.ReadAsStringAsync());
        }
        catch (Exception ex)
        {
            return (false, ex.Message);
        }
    }

    public async Task<bool> DeleteMedicineAsync(Guid id)
    {
        try
        {
            var res = await _http.DeleteAsync($"api/pharmacy/medicines/{id}");
            return res.IsSuccessStatusCode;
        }
        catch
        {
            return false;
        }
    }
}
