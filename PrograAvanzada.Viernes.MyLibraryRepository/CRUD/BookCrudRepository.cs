using Microsoft.EntityFrameworkCore;
using PrograAvanzada.Viernes.MyLibraryDbModel.DbModels;

namespace PrograAvanzada.Viernes.MyLibraryRepository.CRUD;

public class BookCrudRepository
{
    private readonly ViernesContext _context;

    public BookCrudRepository(ViernesContext context)
    {
        _context = context;
    }

    public async Task<Book> CreateAsync(Book book)
    {
        _context.Books.Add(book);
        await _context.SaveChangesAsync();
        return book;
    }

    public async Task<Book> UpdateAsync(Book book)
    {
        _context.Books.Update(book);
        await _context.SaveChangesAsync();
        return book;
    }

    public async Task DeleteAsync(int id)
    {
        var book = await _context.Books.FindAsync(id);
        if (book != null)
        {
            _context.Books.Remove(book);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<Book?> GetByIdAsync(int id)
    {
        return await _context.Books
            .Include(b => b.BookAuthors)
            .Include(b => b.BookThemes)
            .Include(b => b.BookCopies)
            .FirstOrDefaultAsync(b => b.Id == id);
    }

    public async Task<List<Book>> GetAllAsync()
    {
        return await _context.Books
            .Include(b => b.BookAuthors)
            .Include(b => b.BookThemes)
            .Include(b => b.BookCopies)
            .ToListAsync();
    }
}
