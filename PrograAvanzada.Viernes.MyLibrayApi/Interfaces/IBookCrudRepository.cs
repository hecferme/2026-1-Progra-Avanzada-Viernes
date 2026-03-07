using PrograAvanzada.Viernes.MyLibraryDbModel.DbModels;

namespace PrograAvanzada.Viernes.MyLibrayApi.Interfaces;

public interface IBookCrudRepository
{
    Task<Book> CreateAsync(Book book);
    Task<Book> UpdateAsync(Book book);
    Task DeleteAsync(int id);
    Task<Book?> GetByIdAsync(int id);
    Task<List<Book>> GetAllAsync();
}