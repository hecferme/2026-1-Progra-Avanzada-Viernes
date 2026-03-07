using PrograAvanzada.Viernes.MyLibraryDbModel.DbModels;

namespace PrograAvanzada.Viernes.MyLibrayApi.Interfaces;

public interface IBookCopyCrudRepository
{
    Task<BookCopy> CreateAsync(BookCopy bookCopy);
    Task<BookCopy> UpdateAsync(BookCopy bookCopy);
    Task DeleteAsync(int id);
    Task<BookCopy?> GetByIdAsync(int id);
    Task<List<BookCopy>> GetAllAsync();
}