namespace Attendiia.Services.Interface;

public interface IFirebaseDatabaseService
{
    Task<List<T>> GetItemsAsync<T>(string path);
    Task<List<T>> GetItemsAsync<T>(string path, string targetProperty, string equalsValue);
    Task AddItemAsync<T>(string path, string key, T item);
    Task<T> GetItemAsync<T>(string path, string key);
    Task DeleteItemAsync(string path, string key);
    Task UpdateItemAsync<T>(string path, string key, T newItem);
}
