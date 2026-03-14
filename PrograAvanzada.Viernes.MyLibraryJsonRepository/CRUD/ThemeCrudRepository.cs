using PrograAvanzada.Viernes.MyLibraryDbModel.DbModels;
using PrograAvanzada.Viernes.MyLibraryJsonRepository.Data;

namespace PrograAvanzada.Viernes.MyLibraryJsonRepository.CRUD;

public class ThemeCrudRepository
{
    private readonly JsonDataStore _dataStore;

    public ThemeCrudRepository(string jsonPath = "database/json")
    {
        _dataStore = JsonDataStore.GetInstance(jsonPath);
    }

    public Task<Theme> CreateAsync(Theme theme)
    {
        // Generate new ID
        var maxId = _dataStore.Themes.Any() ? _dataStore.Themes.Max(t => t.Id) : 0;
        theme.Id = maxId + 1;
        
        _dataStore.Themes.Add(theme);
        return Task.FromResult(theme);
    }

    public Task<Theme> UpdateAsync(Theme theme)
    {
        var existingTheme = _dataStore.Themes.FirstOrDefault(t => t.Id == theme.Id);
        if (existingTheme != null)
        {
            existingTheme.Name = theme.Name;
        }
        return Task.FromResult(theme);
    }

    public Task DeleteAsync(int id)
    {
        var theme = _dataStore.Themes.FirstOrDefault(t => t.Id == id);
        if (theme != null)
        {
            _dataStore.Themes.Remove(theme);
        }
        return Task.CompletedTask;
    }

    public Task<Theme?> GetByIdAsync(int id)
    {
        var theme = _dataStore.Themes.FirstOrDefault(t => t.Id == id);
        
        if (theme != null)
        {
            // Load BookThemes forward ref, but prevent cycle
            theme.BookThemes = _dataStore.BookThemes
                .Where(bt => bt.theme_id == id)
                .Select(bt => { bt.Book = null; bt.Theme = null; return bt; })
                .ToList();
        }
        
        return Task.FromResult(theme);
    }

    public Task<List<Theme>> GetAllAsync()
    {
        var themes = _dataStore.Themes.ToList();
        foreach (var theme in themes)
        {
            // Load BookThemes forward ref, but prevent cycle
            theme.BookThemes = _dataStore.BookThemes
                .Where(bt => bt.theme_id == theme.Id)
                .Select(bt => { bt.Book = null; bt.Theme = null; return bt; })
                .ToList();
        }
        return Task.FromResult(themes);
    }
}
