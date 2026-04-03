using System.Net.Http.Json;
using System.Security.Principal;
using MediCore.BlazorUI.Models;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.AspNetCore.Identity.Data;

public class AuthService
{
    private readonly HttpClient _http;
    private readonly ProtectedLocalStorage _storage;
    private readonly CustomAuthStateProvider _authProvider;

    public AuthService(HttpClient http,
        ProtectedLocalStorage storage,
        AuthenticationStateProvider authProvider)
    {
        _http = http;
        _storage = storage;
        _authProvider = (CustomAuthStateProvider)authProvider;
    }

    public async Task<bool> Login(LoginRequest request)
    {
        var response = await _http.PostAsJsonAsync("api/auth/login", request);

        if (!response.IsSuccessStatusCode)
            return false;

        var result = await response.Content.ReadFromJsonAsync<LoginResponse>();

        if (result?.Token == null)
            return false;

        await _storage.SetAsync("authToken", result.Token);

        _http.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", result.Token);

        _authProvider.NotifyUserAuthentication(result.Token);

        return true;
    }

    public async Task Logout()
    {
        await _storage.DeleteAsync("authToken");
        _http.DefaultRequestHeaders.Authorization = null;
        _authProvider.NotifyUserLogout();
    }

    public async Task <bool> Register(RegisterUserRequest request)
    {
        var response = await _http.PostAsJsonAsync("api/auth/register", request);

        if(!response.IsSuccessStatusCode)
            return false;
        
        return true;
    }


}