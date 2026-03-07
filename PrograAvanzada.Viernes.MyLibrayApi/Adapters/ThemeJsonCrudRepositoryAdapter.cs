using PrograAvanzada.Viernes.MyLibraryDbModel.DbModels;
using PrograAvanzada.Viernes.MyLibrayApi.Interfaces;
using PrograAvanzada.Viernes.MyLibraryJsonRepository.CRUD;

namespace PrograAvanzada.Viernes.MyLibrayApi.Adapters;

public class ThemeJsonCrudRepositoryAdapter : IThemeCrudRepository
{
    private readonly ThemeCrudRepository _repo;

    public ThemeJsonCrudRepositoryAdapter(ThemeCrudRepository repo)
    {
        _repo = repo;
    }

    public Task<Theme> CreateAsync(Theme theme) => _repo.CreateAsync(theme);
    public Task<Theme> UpdateAsync(Theme theme) => _repo.UpdateAsync(theme);
    public Task DeleteAsync(int id) => _repo.DeleteAsync(id);
    public Task<Theme?> GetByIdAsync(int id) => _repo.GetByIdAsync(id);
    public Task<List<Theme>> GetAllAsync() => _repo.GetAllAsync();
}