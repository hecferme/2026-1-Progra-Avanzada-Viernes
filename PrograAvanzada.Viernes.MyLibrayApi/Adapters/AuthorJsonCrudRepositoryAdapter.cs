using PrograAvanzada.Viernes.MyLibraryDbModel.DbModels;
using PrograAvanzada.Viernes.MyLibrayApi.Interfaces;
using PrograAvanzada.Viernes.MyLibraryJsonRepository.CRUD;

namespace PrograAvanzada.Viernes.MyLibrayApi.Adapters;

public class AuthorJsonCrudRepositoryAdapter : IAuthorCrudRepository
{
    private readonly AuthorCrudRepository _repo;

    public AuthorJsonCrudRepositoryAdapter(AuthorCrudRepository repo)
    {
        _repo = repo;
    }

    public Task<Author> CreateAsync(Author author) => _repo.CreateAsync(author);
    public Task<Author> UpdateAsync(Author author) => _repo.UpdateAsync(author);
    public Task DeleteAsync(int id) => _repo.DeleteAsync(id);
    public Task<Author?> GetByIdAsync(int id) => _repo.GetByIdAsync(id);
    public Task<List<Author>> GetAllAsync() => _repo.GetAllAsync();
}