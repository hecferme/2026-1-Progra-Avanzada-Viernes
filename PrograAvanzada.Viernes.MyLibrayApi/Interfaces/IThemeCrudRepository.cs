using PrograAvanzada.Viernes.MyLibraryDbModel.DbModels;

namespace PrograAvanzada.Viernes.MyLibrayApi.Interfaces;

public interface IThemeCrudRepository
{
    Task<Theme> CreateAsync(Theme theme);
    Task<Theme> UpdateAsync(Theme theme);
    Task DeleteAsync(int id);
    Task<Theme?> GetByIdAsync(int id);
    Task<List<Theme>> GetAllAsync();
}