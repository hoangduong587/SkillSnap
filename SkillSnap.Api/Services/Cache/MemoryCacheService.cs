using Microsoft.Extensions.Caching.Memory;

namespace SkillSnap.Api.Services.Cache
{
    public class MemoryCacheService : ICacheService
    {
        private readonly IMemoryCache _cache;

        public MemoryCacheService(IMemoryCache cache)
        {
            _cache = cache;
        }

        public T? Get<T>(string key)
        {
            return _cache.TryGetValue(key, out T value) ? value : default;
        }

        public void Set<T>(string key, T value, TimeSpan duration)
        {
            _cache.Set(key, value, duration);
        }

        public void Remove(string key)
        {
            _cache.Remove(key);
        }
    }
}
