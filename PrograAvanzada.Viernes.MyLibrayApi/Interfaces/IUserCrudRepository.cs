using PrograAvanzada.Viernes.MyLibraryDbModel.DbModels;

namespace PrograAvanzada.Viernes.MyLibrayApi.Interfaces;

public interface IUserCrudRepository
{
    Task<User> CreateAsync(User user);
    Task<User> UpdateAsync(User user);
    Task DeleteAsync(int id);
    Task<User?> GetByIdAsync(int id);
    Task<List<User>> GetAllAsync();
}