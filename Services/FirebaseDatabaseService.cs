using Attendiia.Services.Interface;
using Attendiia.Stores;
using Firebase.Database;
using Firebase.Database.Query;

namespace Attendiia.Services;

public sealed class FirebaseDatabaseService : IFirebaseDatabaseService
{
    private readonly FirebaseClient _firebaseClient;
    private readonly UserCredentialContainer _userCredentialContainer;
    public FirebaseDatabaseService(UserCredentialContainer userCredentialContainer)
    {
        _userCredentialContainer = userCredentialContainer;
        //TODO: get from appsettings.json
        _firebaseClient = new FirebaseClient(@"https://attendiia-default-rtdb.firebaseio.com/", new FirebaseOptions
        {
            AuthTokenAsyncFactory = LoginAsync
        });
    }

    private async Task<string> LoginAsync()
    {
        return await Task.FromResult(_userCredentialContainer.IdToken);
    }
    public async Task AddItemAsync<T>(string path, string key, T data)
    {
        await _firebaseClient.Child(path).Child(key).PutAsync(data);
    }

    public async Task DeleteItemAsync(string path, string key)
    {
        await _firebaseClient.Child(path).Child(key).DeleteAsync();
    }

    public async Task<T> GetItemAsync<T>(string path, string key)
    {
        T? item = await _firebaseClient.Child(path).Child(key).OnceSingleAsync<T>();
        if (item == null)
        {
            throw new NotSupportedException($"No such item. Path={path},Key={key}");
        }
        return item;
    }

    public async Task<List<T>> GetItemsAsync<T>(string path)
    {
        IReadOnlyCollection<FirebaseObject<T>> result = await _firebaseClient.Child(path).OnceAsync<T>();
        return result.Select(a => a.Object).ToList();
    }

    public async Task<List<T>> GetItemsAsync<T>(string path, string targetProperty, string equalsValue)
    {
        IReadOnlyCollection<FirebaseObject<T>> result = await _firebaseClient.Child(path).OrderBy(targetProperty).EqualTo(equalsValue).OnceAsync<T>();
        return result.Select(a => a.Object).ToList();
    }

    public async Task UpdateItemAsync<T>(string path, string key, T newItem)
    {
        await _firebaseClient.Child(path).Child(key).PutAsync(newItem);
    }
}