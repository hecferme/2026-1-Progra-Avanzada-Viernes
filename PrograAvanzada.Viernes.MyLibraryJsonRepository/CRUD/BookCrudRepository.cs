using PrograAvanzada.Viernes.MyLibraryDbModel.DbModels;
using PrograAvanzada.Viernes.MyLibraryJsonRepository.Data;

namespace PrograAvanzada.Viernes.MyLibraryJsonRepository.CRUD;

public class BookCrudRepository
{
    private readonly JsonDataStore _dataStore;

    public BookCrudRepository(string jsonPath = "database/json")
    {
        _dataStore = JsonDataStore.GetInstance(jsonPath);
    }

    public Task<Book> CreateAsync(Book book)
    {
        // Generate new ID
        var maxId = _dataStore.Books.Any() ? _dataStore.Books.Max(b => b.Id) : 0;
        book.Id = maxId + 1;
        
        _dataStore.Books.Add(book);
        return Task.FromResult(book);
    }

    public Task<Book> UpdateAsync(Book book)
    {
        var existingBook = _dataStore.Books.FirstOrDefault(b => b.Id == book.Id);
        if (existingBook != null)
        {
            existingBook.Title = book.Title;
            existingBook.Isbn = book.Isbn;
            existingBook.PublishedDate = book.PublishedDate;
        }
        return Task.FromResult(book);
    }

    public Task DeleteAsync(int id)
    {
        var book = _dataStore.Books.FirstOrDefault(b => b.Id == id);
        if (book != null)
        {
            _dataStore.Books.Remove(book);
        }
        return Task.CompletedTask;
    }

    public Task<Book?> GetByIdAsync(int id)
    {
        var book = _dataStore.Books
            .FirstOrDefault(b => b.Id == id);
        
        if (book != null)
        {
            // Load related data
            book.BookAuthors = _dataStore.BookAuthors.Where(ba => ba.BookId == id).ToList();
            book.BookThemes = _dataStore.BookThemes.Where(bt => bt.BookId == id).ToList();
            book.BookCopies = _dataStore.BookCopies.Where(bc => bc.BookId == id).ToList();
        }
        
        return Task.FromResult(book);
    }

    public Task<List<Book>> GetAllAsync()
    {
        var books = _dataStore.Books.ToList();
        
        foreach (var book in books)
        {
            book.BookAuthors = _dataStore.BookAuthors.Where(ba => ba.BookId == book.Id).ToList();
            book.BookThemes = _dataStore.BookThemes.Where(bt => bt.BookId == book.Id).ToList();
            book.BookCopies = _dataStore.BookCopies.Where(bc => bc.BookId == book.Id).ToList();
        }
        
        return Task.FromResult(books);
    }
}
