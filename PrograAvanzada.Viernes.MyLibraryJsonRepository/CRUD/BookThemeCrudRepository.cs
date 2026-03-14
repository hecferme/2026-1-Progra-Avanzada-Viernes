using PrograAvanzada.Viernes.MyLibraryDbModel.DbModels;
using PrograAvanzada.Viernes.MyLibraryJsonRepository.Data;

namespace PrograAvanzada.Viernes.MyLibraryJsonRepository.CRUD;

public class BookThemeCrudRepository
{
    private readonly JsonDataStore _dataStore;

    public BookThemeCrudRepository(string jsonPath = "database/json")
    {
        _dataStore = JsonDataStore.GetInstance(jsonPath);
    }

    public Task<BookTheme> CreateAsync(BookTheme bookTheme)
    {
        _dataStore.BookThemes.Add(bookTheme);
        return Task.FromResult(bookTheme);
    }

    public Task<BookTheme> UpdateAsync(BookTheme bookTheme)
    {
        var existingBookTheme = _dataStore.BookThemes
            .FirstOrDefault(bt => bt.book_id == bookTheme.book_id && bt.theme_id == bookTheme.theme_id);
        if (existingBookTheme != null)
        {
            // BookTheme is a composite key entity, so there's not much to update
            existingBookTheme.book_id = bookTheme.book_id;
            existingBookTheme.theme_id = bookTheme.theme_id;
        }
        return Task.FromResult(bookTheme);
    }

    public Task DeleteAsync(int bookId, int themeId)
    {
        var bookTheme = _dataStore.BookThemes
            .FirstOrDefault(bt => bt.book_id == bookId && bt.theme_id == themeId);
        if (bookTheme != null)
        {
            _dataStore.BookThemes.Remove(bookTheme);
        }
        return Task.CompletedTask;
    }

    public Task<BookTheme?> GetByIdAsync(int bookId, int themeId)
    {
        var bookTheme = _dataStore.BookThemes
            .FirstOrDefault(bt => bt.book_id == bookId && bt.theme_id == themeId);
        
        if (bookTheme != null)
        {
            bookTheme.Book = _dataStore.Books.FirstOrDefault(b => b.Id == bookTheme.book_id);
            bookTheme.Theme = _dataStore.Themes.FirstOrDefault(t => t.Id == bookTheme.theme_id);
        }
        
        return Task.FromResult(bookTheme);
    }

    public Task<List<BookTheme>> GetAllAsync()
    {
        var bookThemes = _dataStore.BookThemes.ToList();
        
        foreach (var bookTheme in bookThemes)
        {
            bookTheme.Book = _dataStore.Books.FirstOrDefault(b => b.Id == bookTheme.book_id);
            bookTheme.Theme = _dataStore.Themes.FirstOrDefault(t => t.Id == bookTheme.theme_id);
        }
        
        return Task.FromResult(bookThemes);
    }

    public Task<List<BookTheme>> GetByBookIdAsync(int bookId)
    {
        var bookThemes = _dataStore.BookThemes.Where(bt => bt.book_id == bookId).ToList();
        
        foreach (var bookTheme in bookThemes)
        {
            bookTheme.Theme = _dataStore.Themes.FirstOrDefault(t => t.Id == bookTheme.theme_id);
        }
        
        return Task.FromResult(bookThemes);
    }

    public Task<List<BookTheme>> GetByThemeIdAsync(int themeId)
    {
        var bookThemes = _dataStore.BookThemes.Where(bt => bt.theme_id == themeId).ToList();
        
        foreach (var bookTheme in bookThemes)
        {
            bookTheme.Book = _dataStore.Books.FirstOrDefault(b => b.Id == bookTheme.book_id);
        }
        
        return Task.FromResult(bookThemes);
    }
}
