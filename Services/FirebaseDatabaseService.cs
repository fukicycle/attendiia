using Attendiia.Services.Interface;
using Firebase.Database;
using Firebase.Database.Query;

namespace Attendiia.Services;

public sealed class FirebaseDatabaseService : IFirebaseDatabaseService
{
    private readonly FirebaseClient _firebaseClient;
    public FirebaseDatabaseService()
    {
        //TODO: get from appsettings.json
        _firebaseClient = new FirebaseClient(@"https://attendiia-default-rtdb.firebaseio.com/");
    }

    public async Task AddItemAsync<T>(string path, string key, T data)
    {
        await _firebaseClient.Child(path).Child(key).PutAsync(data);
    }

    public async Task DeleteItemAsync(string path, string key)
    {
        await _firebaseClient.Child(path).Child(key).DeleteAsync();
    }

    public async Task<T?> GetItemAsync<T>(string path, string key)
    {
        return await _firebaseClient.Child($"{path}/{key}").OnceSingleAsync<T>();
    }

    public async Task<List<T>> GetItemsAsync<T>(string path)
    {
        IReadOnlyCollection<FirebaseObject<T>> result = await _firebaseClient.Child(path).OnceAsync<T>();
        return result.Select(a => a.Object).ToList();
    }

    public async Task UpdateItemAsync<T>(string path, string key, T newItem)
    {
        await _firebaseClient.Child(path).Child(key).PutAsync(newItem);
    }
}
