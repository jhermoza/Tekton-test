using ProductApi.Application.Interfaces;
using Microsoft.Extensions.Caching.Memory;

namespace ProductApi.Infrastructure.Services
{
    public class StatusCache : IStatusCache
    {
        private readonly IMemoryCache _cache;
        private readonly Dictionary<int, string> _statusNames = new()
        {
            { 0, "Inactive" },
            { 1, "Active" }
        };
        public StatusCache(IMemoryCache cache)
        {
            _cache = cache;
        }
        public Task<string> GetStatusNameAsync(int status)
        {
            if (!_cache.TryGetValue(status, out string? name))
            {
                name = _statusNames.GetValueOrDefault(status, "Unknown");
                _cache.Set(status, name, TimeSpan.FromMinutes(5));
            }
            return Task.FromResult(name!);
        }
    }
}
