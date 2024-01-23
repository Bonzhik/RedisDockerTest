namespace NewFeatures.Services.Cache
{
    public interface ICacheService
    {
        Task<T> GetDataAsync<T>(string key);
        Task<bool> SetDataAsync<T>(string key, T value, DateTimeOffset expirationTime);
        Task<bool> DeleteDataAsync<T>(string key);
    }
}
