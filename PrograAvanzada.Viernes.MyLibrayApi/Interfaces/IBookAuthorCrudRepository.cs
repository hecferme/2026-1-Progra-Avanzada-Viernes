using PrograAvanzada.Viernes.MyLibraryDbModel.DbModels;

namespace PrograAvanzada.Viernes.MyLibrayApi.Interfaces;

public interface IBookAuthorCrudRepository
{
    Task<BookAuthor> CreateAsync(BookAuthor bookAuthor);
    Task<BookAuthor> UpdateAsync(BookAuthor bookAuthor);
    Task DeleteAsync(int bookId, int authorId);
    Task<BookAuthor?> GetByIdAsync(int bookId, int authorId);
    Task<List<BookAuthor>> GetAllAsync();
}