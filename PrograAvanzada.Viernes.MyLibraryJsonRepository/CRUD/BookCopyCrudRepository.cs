using PrograAvanzada.Viernes.MyLibraryDbModel.DbModels;
using PrograAvanzada.Viernes.MyLibraryJsonRepository.Data;

namespace PrograAvanzada.Viernes.MyLibraryJsonRepository.CRUD;

public class BookCopyCrudRepository
{
    private readonly JsonDataStore _dataStore;

    public BookCopyCrudRepository(string jsonPath = "database/json")
    {
        _dataStore = JsonDataStore.GetInstance(jsonPath);
    }

    public Task<BookCopy> CreateAsync(BookCopy bookCopy)
    {
        // Generate new ID
        var maxId = _dataStore.BookCopies.Any() ? _dataStore.BookCopies.Max(bc => bc.Id) : 0;
        bookCopy.Id = maxId + 1;
        
        _dataStore.BookCopies.Add(bookCopy);
        return Task.FromResult(bookCopy);
    }

    public Task<BookCopy> UpdateAsync(BookCopy bookCopy)
    {
        var existingBookCopy = _dataStore.BookCopies.FirstOrDefault(bc => bc.Id == bookCopy.Id);
        if (existingBookCopy != null)
        {
            existingBookCopy.BookId = bookCopy.BookId;
            existingBookCopy.Barcode = bookCopy.Barcode;
            existingBookCopy.Status = bookCopy.Status;
        }
        return Task.FromResult(bookCopy);
    }

    public Task DeleteAsync(int id)
    {
        var bookCopy = _dataStore.BookCopies.FirstOrDefault(bc => bc.Id == id);
        if (bookCopy != null)
        {
            _dataStore.BookCopies.Remove(bookCopy);
        }
        return Task.CompletedTask;
    }

    public Task<BookCopy?> GetByIdAsync(int id)
    {
        var bookCopy = _dataStore.BookCopies.FirstOrDefault(bc => bc.Id == id);
        
        if (bookCopy != null)
        {
            bookCopy.Book = _dataStore.Books.FirstOrDefault(b => b.Id == bookCopy.BookId);
            bookCopy.Borrows = _dataStore.Borrows.Where(b => b.BookCopyId == id).ToList();
        }
        
        return Task.FromResult(bookCopy);
    }

    public Task<List<BookCopy>> GetAllAsync()
    {
        var bookCopies = _dataStore.BookCopies.ToList();
        
        foreach (var bookCopy in bookCopies)
        {
            bookCopy.Book = _dataStore.Books.FirstOrDefault(b => b.Id == bookCopy.BookId);
        }
        
        return Task.FromResult(bookCopies);
    }

    public Task<List<BookCopy>> GetByBookIdAsync(int bookId)
    {
        var bookCopies = _dataStore.BookCopies.Where(bc => bc.BookId == bookId).ToList();
        
        foreach (var bookCopy in bookCopies)
        {
            bookCopy.Book = _dataStore.Books.FirstOrDefault(b => b.Id == bookCopy.BookId);
        }
        
        return Task.FromResult(bookCopies);
    }
}
