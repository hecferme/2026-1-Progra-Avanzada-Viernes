using PrograAvanzada.Viernes.MyLibraryDbModel.DbModels;

namespace PrograAvanzada.Viernes.MyLibrayApi.Interfaces;

public interface IAuthorCrudRepository
{
    Task<Author> CreateAsync(Author author);
    Task<Author> UpdateAsync(Author author);
    Task DeleteAsync(int id);
    Task<Author?> GetByIdAsync(int id);
    Task<List<Author>> GetAllAsync();
}