using Microsoft.EntityFrameworkCore;
using PrograAvanzada.Viernes.MyLibraryDbModel.DbModels;

namespace PrograAvanzada.Viernes.MyLibraryRepository.CRUD;

public class BookAuthorCrudRepository
{
    private readonly ViernesContext _context;

    public BookAuthorCrudRepository(ViernesContext context)
    {
        _context = context;
    }

    public async Task<BookAuthor> CreateAsync(BookAuthor bookAuthor)
    {
        _context.BookAuthors.Add(bookAuthor);
        await _context.SaveChangesAsync();
        return bookAuthor;
    }

    public async Task<BookAuthor> UpdateAsync(BookAuthor bookAuthor)
    {
        _context.BookAuthors.Update(bookAuthor);
        await _context.SaveChangesAsync();
        return bookAuthor;
    }

    public async Task DeleteAsync(int bookId, int authorId)
    {
        var bookAuthor = await _context.BookAuthors.FindAsync(bookId, authorId);
        if (bookAuthor != null)
        {
            _context.BookAuthors.Remove(bookAuthor);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<BookAuthor?> GetByIdAsync(int bookId, int authorId)
    {
        return await _context.BookAuthors
            .Include(ba => ba.Book)
            .Include(ba => ba.Author)
            .FirstOrDefaultAsync(ba => ba.BookId == bookId && ba.AuthorId == authorId);
    }

    public async Task<List<BookAuthor>> GetAllAsync()
    {
        return await _context.BookAuthors
            .Include(ba => ba.Book)
            .Include(ba => ba.Author)
            .ToListAsync();
    }

    public async Task<List<BookAuthor>> GetByBookIdAsync(int bookId)
    {
        return await _context.BookAuthors
            .Include(ba => ba.Author)
            .Where(ba => ba.BookId == bookId)
            .ToListAsync();
    }

    public async Task<List<BookAuthor>> GetByAuthorIdAsync(int authorId)
    {
        return await _context.BookAuthors
            .Include(ba => ba.Book)
            .Where(ba => ba.AuthorId == authorId)
            .ToListAsync();
    }
}
