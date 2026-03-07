using PrograAvanzada.Viernes.MyLibraryDbModel.DbModels;
using PrograAvanzada.Viernes.MyLibrayApi.Interfaces;
using PrograAvanzada.Viernes.MyLibraryJsonRepository.CRUD;

namespace PrograAvanzada.Viernes.MyLibrayApi.Adapters;

public class UserJsonCrudRepositoryAdapter : IUserCrudRepository
{
    private readonly UserCrudRepository _repo;

    public UserJsonCrudRepositoryAdapter(UserCrudRepository repo)
    {
        _repo = repo;
    }

    public Task<User> CreateAsync(User user) => _repo.CreateAsync(user);
    public Task<User> UpdateAsync(User user) => _repo.UpdateAsync(user);
    public Task DeleteAsync(int id) => _repo.DeleteAsync(id);
    public Task<User?> GetByIdAsync(int id) => _repo.GetByIdAsync(id);
    public Task<List<User>> GetAllAsync() => _repo.GetAllAsync();
}