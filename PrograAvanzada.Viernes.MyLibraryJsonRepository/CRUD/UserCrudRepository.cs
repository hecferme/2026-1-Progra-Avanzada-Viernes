using PrograAvanzada.Viernes.MyLibraryDbModel.DbModels;
using PrograAvanzada.Viernes.MyLibraryJsonRepository.Data;

namespace PrograAvanzada.Viernes.MyLibraryJsonRepository.CRUD;

public class UserCrudRepository
{
    private readonly JsonDataStore _dataStore;

    public UserCrudRepository(string jsonPath = "database/json")
    {
        _dataStore = JsonDataStore.GetInstance(jsonPath);
    }

    public Task<User> CreateAsync(User user)
    {
        // Generate new ID
        var maxId = _dataStore.Users.Any() ? _dataStore.Users.Max(u => u.Id) : 0;
        user.Id = maxId + 1;
        
        _dataStore.Users.Add(user);
        return Task.FromResult(user);
    }

    public Task<User> UpdateAsync(User user)
    {
        var existingUser = _dataStore.Users.FirstOrDefault(u => u.Id == user.Id);
        if (existingUser != null)
        {
            existingUser.FullName = user.FullName;
            existingUser.Email = user.Email;
        }
        return Task.FromResult(user);
    }

    public Task DeleteAsync(int id)
    {
        var user = _dataStore.Users.FirstOrDefault(u => u.Id == id);
        if (user != null)
        {
            _dataStore.Users.Remove(user);
        }
        return Task.CompletedTask;
    }

    public Task<User?> GetByIdAsync(int id)
    {
        var user = _dataStore.Users
            .FirstOrDefault(u => u.Id == id);
        
        if (user != null)
        {
            user.Borrows = _dataStore.Borrows.Where(b => b.UserId == id).ToList();
        }
        
        return Task.FromResult(user);
    }

    public Task<List<User>> GetAllAsync()
    {
        var users = _dataStore.Users.ToList();
        return Task.FromResult(users);
    }
}
