using Microsoft.EntityFrameworkCore;
using PrograAvanzada.Viernes.MyLibraryDbModel.DbModels;
using PrograAvanzada.Viernes.MyLibraryRepository.DTOs;

namespace PrograAvanzada.Viernes.MyLibraryRepository.Repositories;

public class BookRepository
{
    private readonly ViernesContext _context;

    public BookRepository(ViernesContext context)
    {
        _context = context;
    }

    private IQueryable<BookDto> GetBaseQuery()
    {
        return _context.Books
            .Include(b => b.BookCopies)
            .Include(b => b.BookAuthors).ThenInclude(ba => ba.Author)
            .Include(b => b.BookThemes).ThenInclude(bt => bt.Theme)
            .SelectMany(b => b.BookCopies, (b, bc) => new { Book = b, BookCopy = bc })
            .SelectMany(x => x.Book.BookAuthors, (x, ba) => new { x.Book, x.BookCopy, BookAuthor = ba })
            .Select(x => new BookDto
            {
                Id = x.Book.Id,
                Title = x.Book.Title,
                Isbn = x.Book.Isbn,
                BookAuthorName = x.BookAuthor.Author.Name,
                BookThemes = x.Book.Themes,
                BookCopyId = x.BookCopy.Id,
                BookCopyStatus = x.BookCopy.Status
            });
    }

    /// <summary>
    /// Query by exact book id
    /// </summary>
    public async Task<List<BookDto>> GetByBookIdAsync(int bookId)
    {
        return await GetBaseQuery()
            .Where(b => b.Id == bookId)
            .ToListAsync();
    }

    /// <summary>
    /// Query by approximate book title
    /// </summary>
    public async Task<List<BookDto>> GetByApproximateTitleAsync(string title)
    {
        return await GetBaseQuery()
            .Where(b => b.Title.Contains(title))
            .ToListAsync();
    }

    /// <summary>
    /// Query by exact ISBN
    /// </summary>
    public async Task<List<BookDto>> GetByExactIsbnAsync(string isbn)
    {
        return await GetBaseQuery()
            .Where(b => b.Isbn == isbn)
            .ToListAsync();
    }

    /// <summary>
    /// Query by approximate author name
    /// </summary>
    public async Task<List<BookDto>> GetByApproximateAuthorNameAsync(string authorName)
    {
        return await GetBaseQuery()
            .Where(b => b.BookAuthorName.Contains(authorName))
            .ToListAsync();
    }

    /// <summary>
    /// Query by exact theme
    /// </summary>
    public async Task<List<BookDto>> GetByExactThemeAsync(string theme)
    {
        var booksWithTheme = _context.Books
            .Include(b => b.BookThemes).ThenInclude(bt => bt.Theme)
            .Where(b => b.BookThemes.Any(bt => bt.Theme.Name == theme))
            .Select(b => b.Id)
            .ToHashSet();

        return await GetBaseQuery()
            .Where(b => booksWithTheme.Contains(b.Id))
            .ToListAsync();
    }

    /// <summary>
    /// Query by book copy status
    /// </summary>
    public async Task<List<BookDto>> GetByBookCopyStatusAsync(byte status)
    {
        return await GetBaseQuery()
            .Where(b => b.BookCopyStatus == status)
            .ToListAsync();
    }
}
