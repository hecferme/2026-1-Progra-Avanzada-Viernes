using PrograAvanzada.Viernes.MyLibraryDbModel.DbModels;

namespace PrograAvanzada.Viernes.MyLibrayApi.Interfaces;

public interface IBookThemeCrudRepository
{
    Task<BookTheme> CreateAsync(BookTheme bookTheme);
    Task<BookTheme> UpdateAsync(BookTheme bookTheme);
    Task DeleteAsync(int bookId, int themeId);
    Task<BookTheme?> GetByIdAsync(int bookId, int themeId);
    Task<List<BookTheme>> GetAllAsync();
}