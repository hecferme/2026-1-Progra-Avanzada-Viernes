using PrograAvanzada.Viernes.MyLibraryDbModel.DbModels;
using PrograAvanzada.Viernes.MyLibrayApi.Interfaces;
using PrograAvanzada.Viernes.MyLibraryJsonRepository.CRUD;

namespace PrograAvanzada.Viernes.MyLibrayApi.Adapters;

public class BookThemeJsonCrudRepositoryAdapter : IBookThemeCrudRepository
{
    private readonly BookThemeCrudRepository _repo;

    public BookThemeJsonCrudRepositoryAdapter(BookThemeCrudRepository repo)
    {
        _repo = repo;
    }

    public Task<BookTheme> CreateAsync(BookTheme bookTheme) => _repo.CreateAsync(bookTheme);
    public Task<BookTheme> UpdateAsync(BookTheme bookTheme) => _repo.UpdateAsync(bookTheme);
    public Task DeleteAsync(int bookId, int themeId) => _repo.DeleteAsync(bookId, themeId);
    public Task<BookTheme?> GetByIdAsync(int bookId, int themeId) => _repo.GetByIdAsync(bookId, themeId);
    public Task<List<BookTheme>> GetAllAsync() => _repo.GetAllAsync();
}