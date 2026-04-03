using Microsoft.JSInterop;

namespace MediCore.BlazorUI.Services;

public class ThemeService
{
    private readonly IJSRuntime _js;
    private bool _isDarkMode;

    public ThemeService(IJSRuntime js)
    {
        _js = js;
    }

    public event Action? OnChange;

    public bool IsDarkMode
    {
        get => _isDarkMode;
        private set
        {
            if (_isDarkMode != value)
            {
                _isDarkMode = value;
                NotifyStateChanged();
            }
        }
    }

    // Call this only from OnAfterRenderAsync (first render)
    public async Task InitializeThemeAsync()
    {
        var storedTheme = await _js.InvokeAsync<string>("localStorage.getItem", "theme");
        IsDarkMode = storedTheme == "dark";
        await ApplyThemeAsync(); // Apply immediately after reading
    }

    // Applies the current theme by toggling the CSS class on body
    public async Task ApplyThemeAsync()
    {
        await _js.InvokeVoidAsync("eval", $"document.body.classList.toggle('dark', {IsDarkMode.ToString().ToLower()})");
    }

    // Toggle theme from user action (safe because it's interactive)
    public async Task ToggleDarkModeAsync()
    {
        IsDarkMode = !IsDarkMode;
        await _js.InvokeVoidAsync("localStorage.setItem", "theme", IsDarkMode ? "dark" : "light");
        await ApplyThemeAsync();
    }

    private void NotifyStateChanged() => OnChange?.Invoke();
}