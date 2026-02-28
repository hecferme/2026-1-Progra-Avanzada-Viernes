using Microsoft.EntityFrameworkCore;
using PrograAvanzada.Viernes.MyLibraryDbModel.DbModels;

namespace PrograAvanzada.Viernes.MyLibraryRepository.CRUD;

public class ThemeCrudRepository
{
    private readonly ViernesContext _context;

    public ThemeCrudRepository(ViernesContext context)
    {
        _context = context;
    }

    public async Task<Theme> CreateAsync(Theme theme)
    {
        _context.Themes.Add(theme);
        await _context.SaveChangesAsync();
        return theme;
    }

    public async Task<Theme> UpdateAsync(Theme theme)
    {
        _context.Themes.Update(theme);
        await _context.SaveChangesAsync();
        return theme;
    }

    public async Task DeleteAsync(int id)
    {
        var theme = await _context.Themes.FindAsync(id);
        if (theme != null)
        {
            _context.Themes.Remove(theme);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<Theme?> GetByIdAsync(int id)
    {
        return await _context.Themes
            .Include(t => t.BookThemes)
            .FirstOrDefaultAsync(t => t.Id == id);
    }

    public async Task<List<Theme>> GetAllAsync()
    {
        return await _context.Themes.ToListAsync();
    }
}
