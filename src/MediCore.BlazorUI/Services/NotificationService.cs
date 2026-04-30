namespace MediCore.BlazorUI.Services;

public class NotificationService
{
    private readonly List<NotificationItem> _notifications = new();

    public event Action? OnChange;

    public IReadOnlyList<NotificationItem> Notifications => _notifications.AsReadOnly();

    public int UnreadCount => _notifications.Count(n => !n.IsRead);

    public void Add(string message, string type = "info")
    {
        _notifications.Insert(0, new NotificationItem
        {
            Message = message,
            Type = type,
            CreatedAt = DateTime.Now
        });
        OnChange?.Invoke();
    }

    public void MarkAllRead()
    {
        foreach (var n in _notifications)
            n.IsRead = true;
        OnChange?.Invoke();
    }

    public void Clear()
    {
        _notifications.Clear();
        OnChange?.Invoke();
    }
}

public class NotificationItem
{
    public string Message { get; set; } = "";
    public string Type { get; set; } = "info";
    public DateTime CreatedAt { get; set; }
    public bool IsRead { get; set; }
}
