using StackExchange.Redis;
using System.Text.Json;

namespace NewFeatures.Services.Cache
{
    public class CacheService : ICacheService
    {
        private IDatabase _cacheDb;
        public CacheService()
        {
            var redis = ConnectionMultiplexer.Connect("app-redis:6379");
            _cacheDb = redis.GetDatabase();
        }

        public async Task<bool> DeleteDataAsync<T>(string key)
        {
            if (await _cacheDb.KeyExistsAsync(key))
            {
                return await _cacheDb.KeyDeleteAsync(key);
            }
            return false;
        }

        public async Task<T> GetDataAsync<T>(string key)
        {
            var value = await _cacheDb.StringGetAsync(key);
            if (!string.IsNullOrEmpty(value))
            {
                Console.WriteLine("Данные из кеша");
                return JsonSerializer.Deserialize<T>(value);
            }
            return default(T);
        }

        public async Task<bool> SetDataAsync<T>(string key, T value, DateTimeOffset expirationTime)
        {
            var expTime = expirationTime.DateTime.Subtract(DateTime.Now);
            return await _cacheDb.StringSetAsync(key, JsonSerializer.Serialize(value), expTime);
        }
    }
}
