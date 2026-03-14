using PrograAvanzada.Viernes.MyLibraryDbModel.DbModels;
using PrograAvanzada.Viernes.MyLibraryJsonRepository.Data;
using PrograAvanzada.Viernes.MyLibraryRepository.DTOs;

namespace PrograAvanzada.Viernes.MyLibraryJsonRepository.Repositories;

public class BookRepository
{
    private readonly JsonDataStore _dataStore;

    public BookRepository(string jsonPath = "database/json")
    {
        _dataStore = JsonDataStore.GetInstance(jsonPath);
    }

    private List<BookDto> GetBaseQuery()
    {
        var result = new List<BookDto>();

        foreach (var book in _dataStore.Books)
        {
            var bookAuthors = book.BookAuthors ?? new List<BookAuthor>();
            var bookCopies = book.BookCopies ?? new List<BookCopy>();
            
            // Get themes for this book
            var themes = _dataStore.BookThemes
                .Where(bt => bt.book_id == book.Id)
                .Select(bt => bt.Theme?.Name ?? "")
                .ToList();
            var themesString = string.Join(", ", themes);

            foreach (var bookCopy in bookCopies)
            {
                foreach (var bookAuthor in bookAuthors)
                {
                    result.Add(new BookDto
                    {
                        Id = book.Id,
                        Title = book.Title,
                        Isbn = book.Isbn,
                        BookAuthorName = bookAuthor.Author?.Name ?? "",
                        BookThemes = themesString,
                        BookCopyId = bookCopy.Id,
                        BookCopyStatus = bookCopy.Status
                    });
                }
            }
        }

        return result;
    }

    /// <summary>
    /// Query by exact book id
    /// </summary>
    public Task<List<BookDto>> GetByBookIdAsync(int bookId)
    {
        var query = GetBaseQuery()
            .Where(b => b.Id == bookId)
            .ToList();

        return Task.FromResult(query);
    }

    /// <summary>
    /// Query by approximate book title
    /// </summary>
    public Task<List<BookDto>> GetByApproximateTitleAsync(string title)
    {
        var query = GetBaseQuery()
            .Where
            (b => b.Title.Contains(title))
            .ToList();

        return Task.FromResult(query);
    }

    /// <summary>
    /// Query by exact ISBN
    /// </summary>
    public Task<List<BookDto>> GetByExactIsbnAsync(string isbn)
    {
        var query = GetBaseQuery()
            .Where(b => b.Isbn == isbn)
            .ToList();

        return Task.FromResult(query);
    }

    /// <summary>
    /// Query by approximate author name
    /// </summary>
    public Task<List<BookDto>> GetByApproximateAuthorNameAsync(string authorName)
    {
        var query = GetBaseQuery()
            .Where(b => b.BookAuthorName.Contains(authorName))
            .ToList();

        return Task.FromResult(query);
    }

    /// <summary>
    /// Query by exact theme
    /// </summary>
    public Task<List<BookDto>> GetByExactThemeAsync(string theme)
    {
        var booksWithTheme = _dataStore.Books
            .Where(b => (b.BookThemes ?? new List<BookTheme>()).Any(bt => bt.Theme?.Name == theme))
            .Select(b => b.Id)
            .ToHashSet();

        var query = GetBaseQuery()
            .Where(b => booksWithTheme.Contains(b.Id))
            .ToList();

        return Task.FromResult(query);
    }

    /// <summary>
    /// Query by book copy status
    /// </summary>
    public Task<List<BookDto>> GetByBookCopyStatusAsync(byte status)
    {
        var query = GetBaseQuery()
            .Where(b => b.BookCopyStatus == status)
            .ToList();

        return Task.FromResult(query);
    }
}
