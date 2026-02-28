using Microsoft.EntityFrameworkCore;
using PrograAvanzada.Viernes.MyLibraryDbModel.DbModels;

namespace PrograAvanzada.Viernes.MyLibraryRepository.CRUD;

public class BookThemeCrudRepository
{
    private readonly ViernesContext _context;

    public BookThemeCrudRepository(ViernesContext context)
    {
        _context = context;
    }

    public async Task<BookTheme> CreateAsync(BookTheme bookTheme)
    {
        _context.BookThemes.Add(bookTheme);
        await _context.SaveChangesAsync();
        return bookTheme;
    }

    public async Task<BookTheme> UpdateAsync(BookTheme bookTheme)
    {
        _context.BookThemes.Update(bookTheme);
        await _context.SaveChangesAsync();
        return bookTheme;
    }

    public async Task DeleteAsync(int bookId, int themeId)
    {
        var bookTheme = await _context.BookThemes.FindAsync(bookId, themeId);
        if (bookTheme != null)
        {
            _context.BookThemes.Remove(bookTheme);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<BookTheme?> GetByIdAsync(int bookId, int themeId)
    {
        return await _context.BookThemes
            .Include(bt => bt.Book)
            .Include(bt => bt.Theme)
            .FirstOrDefaultAsync(bt => bt.BookId == bookId && bt.ThemeId == themeId);
    }

    public async Task<List<BookTheme>> GetAllAsync()
    {
        return await _context.BookThemes
            .Include(bt => bt.Book)
            .Include(bt => bt.Theme)
            .ToListAsync();
    }

    public async Task<List<BookTheme>> GetByBookIdAsync(int bookId)
    {
        return await _context.BookThemes
            .Include(bt => bt.Theme)
            .Where(bt => bt.BookId == bookId)
            .ToListAsync();
    }

    public async Task<List<BookTheme>> GetByThemeIdAsync(int themeId)
    {
        return await _context.BookThemes
            .Include(bt => bt.Book)
            .Where(bt => bt.ThemeId == themeId)
            .ToListAsync();
    }
}
