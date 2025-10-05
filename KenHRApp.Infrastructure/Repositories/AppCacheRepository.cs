using KenHRApp.Domain.Interfaces;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KenHRApp.Infrastructure.Repositories
{
    public class AppCacheRepository : IAppCacheRepository
    {
        private readonly IMemoryCache _cache;

        public AppCacheRepository(IMemoryCache cache)
        {
            _cache = cache;
        }

        public Task SetAsync<T>(string key, T value, TimeSpan? expiry = null)
        {
            var options = expiry.HasValue
                ? new MemoryCacheEntryOptions { AbsoluteExpirationRelativeToNow = expiry }
                : new MemoryCacheEntryOptions();

            _cache.Set(key, value, options);
            return Task.CompletedTask;
        }

        public Task<T?> GetAsync<T>(string key)
        {
            return Task.FromResult(_cache.TryGetValue(key, out T? value) ? value : default);
        }

        public Task RemoveAsync(string key)
        {
            _cache.Remove(key);
            return Task.CompletedTask;
        }

        public Task<bool> ExistsAsync(string key)
        {
            return Task.FromResult(_cache.TryGetValue(key, out _));
        }
    }
}
