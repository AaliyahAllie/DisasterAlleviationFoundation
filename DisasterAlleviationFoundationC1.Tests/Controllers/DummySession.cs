using Microsoft.AspNetCore.Http;
using System.Text;

public class DummySession : ISession
{
    private readonly Dictionary<string, byte[]> _sessionStorage = new();
    public IEnumerable<string> Keys => _sessionStorage.Keys;
    public string Id => Guid.NewGuid().ToString();
    public bool IsAvailable => true;
    public void Clear() => _sessionStorage.Clear();
    public Task CommitAsync(CancellationToken cancellationToken = default) => Task.CompletedTask;
    public Task LoadAsync(CancellationToken cancellationToken = default) => Task.CompletedTask;
    public void Remove(string key) => _sessionStorage.Remove(key);
    public void Set(string key, byte[] value) => _sessionStorage[key] = value;
    public bool TryGetValue(string key, out byte[] value) => _sessionStorage.TryGetValue(key, out value);

    // Add these for SetString/GetString
    public void SetString(string key, string value) => _sessionStorage[key] = Encoding.UTF8.GetBytes(value);
    public string GetString(string key) => _sessionStorage.TryGetValue(key, out var val) ? Encoding.UTF8.GetString(val) : null;
}
