using Microsoft.EntityFrameworkCore;
using PrograAvanzada.Viernes.MyLibraryDbModel.DbModels;

namespace PrograAvanzada.Viernes.MyLibraryRepository.CRUD;

public class BookCopyCrudRepository
{
    private readonly ViernesContext _context;

    public BookCopyCrudRepository(ViernesContext context)
    {
        _context = context;
    }

    public async Task<BookCopy> CreateAsync(BookCopy bookCopy)
    {
        _context.BookCopies.Add(bookCopy);
        await _context.SaveChangesAsync();
        return bookCopy;
    }

    public async Task<BookCopy> UpdateAsync(BookCopy bookCopy)
    {
        _context.BookCopies.Update(bookCopy);
        await _context.SaveChangesAsync();
        return bookCopy;
    }

    public async Task DeleteAsync(int id)
    {
        var bookCopy = await _context.BookCopies.FindAsync(id);
        if (bookCopy != null)
        {
            _context.BookCopies.Remove(bookCopy);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<BookCopy?> GetByIdAsync(int id)
    {
        return await _context.BookCopies
            .Include(bc => bc.Book)
            .Include(bc => bc.Borrows)
            .FirstOrDefaultAsync(bc => bc.Id == id);
    }

    public async Task<List<BookCopy>> GetAllAsync()
    {
        return await _context.BookCopies
            .Include(bc => bc.Book)
            .ToListAsync();
    }

    public async Task<List<BookCopy>> GetByBookIdAsync(int bookId)
    {
        return await _context.BookCopies
            .Where(bc => bc.BookId == bookId)
            .ToListAsync();
    }
}
