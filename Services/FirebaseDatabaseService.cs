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

    public async Task AddAsync<T>(string path, T data)
    {
        await _firebaseClient.Child(path).PostAsync(data);
    }

    public async Task<IEnumerable<T>> GetListAsync<T>(string path)
    {
        IReadOnlyCollection<FirebaseObject<T>> result = await _firebaseClient.Child(path).OnceAsync<T>();
        return result.Select(a => a.Object);
    }
}
