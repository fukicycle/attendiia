namespace Attendiia.Services.Interface;

public interface IFirebaseDatabaseService
{
    Task<IEnumerable<T>> GetListAsync<T>(string path);

    Task AddAsync<T>(string path, T data);
}
