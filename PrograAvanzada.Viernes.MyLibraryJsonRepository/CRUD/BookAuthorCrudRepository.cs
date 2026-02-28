using PrograAvanzada.Viernes.MyLibraryDbModel.DbModels;
using PrograAvanzada.Viernes.MyLibraryJsonRepository.Data;

namespace PrograAvanzada.Viernes.MyLibraryJsonRepository.CRUD;

public class BookAuthorCrudRepository
{
    private readonly JsonDataStore _dataStore;

    public BookAuthorCrudRepository(string jsonPath = "database/json")
    {
        _dataStore = JsonDataStore.GetInstance(jsonPath);
    }

    public Task<BookAuthor> CreateAsync(BookAuthor bookAuthor)
    {
        _dataStore.BookAuthors.Add(bookAuthor);
        return Task.FromResult(bookAuthor);
    }

    public Task<BookAuthor> UpdateAsync(BookAuthor bookAuthor)
    {
        var existingBookAuthor = _dataStore.BookAuthors
            .FirstOrDefault(ba => ba.BookId == bookAuthor.BookId && ba.AuthorId == bookAuthor.AuthorId);
        if (existingBookAuthor != null)
        {
            // BookAuthor is a composite key entity, so there's not much to update
            existingBookAuthor.BookId = bookAuthor.BookId;
            existingBookAuthor.AuthorId = bookAuthor.AuthorId;
        }
        return Task.FromResult(bookAuthor);
    }

    public Task DeleteAsync(int bookId, int authorId)
    {
        var bookAuthor = _dataStore.BookAuthors
            .FirstOrDefault(ba => ba.BookId == bookId && ba.AuthorId == authorId);
        if (bookAuthor != null)
        {
            _dataStore.BookAuthors.Remove(bookAuthor);
        }
        return Task.CompletedTask;
    }

    public Task<BookAuthor?> GetByIdAsync(int bookId, int authorId)
    {
        var bookAuthor = _dataStore.BookAuthors
            .FirstOrDefault(ba => ba.BookId == bookId && ba.AuthorId == authorId);
        
        if (bookAuthor != null)
        {
            bookAuthor.Book = _dataStore.Books.FirstOrDefault(b => b.Id == bookAuthor.BookId);
            bookAuthor.Author = _dataStore.Authors.FirstOrDefault(a => a.Id == bookAuthor.AuthorId);
        }
        
        return Task.FromResult(bookAuthor);
    }

    public Task<List<BookAuthor>> GetAllAsync()
    {
        var bookAuthors = _dataStore.BookAuthors.ToList();
        
        foreach (var bookAuthor in bookAuthors)
        {
            bookAuthor.Book = _dataStore.Books.FirstOrDefault(b => b.Id == bookAuthor.BookId);
            bookAuthor.Author = _dataStore.Authors.FirstOrDefault(a => a.Id == bookAuthor.AuthorId);
        }
        
        return Task.FromResult(bookAuthors);
    }

    public Task<List<BookAuthor>> GetByBookIdAsync(int bookId)
    {
        var bookAuthors = _dataStore.BookAuthors.Where(ba => ba.BookId == bookId).ToList();
        
        foreach (var bookAuthor in bookAuthors)
        {
            bookAuthor.Author = _dataStore.Authors.FirstOrDefault(a => a.Id == bookAuthor.AuthorId);
        }
        
        return Task.FromResult(bookAuthors);
    }

    public Task<List<BookAuthor>> GetByAuthorIdAsync(int authorId)
    {
        var bookAuthors = _dataStore.BookAuthors.Where(ba => ba.AuthorId == authorId).ToList();
        
        foreach (var bookAuthor in bookAuthors)
        {
            bookAuthor.Book = _dataStore.Books.FirstOrDefault(b => b.Id == bookAuthor.BookId);
        }
        
        return Task.FromResult(bookAuthors);
    }
}
