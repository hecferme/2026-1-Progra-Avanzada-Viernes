using PrograAvanzada.Viernes.MyLibraryDbModel.DbModels;
using PrograAvanzada.Viernes.MyLibraryJsonRepository.Data;

namespace PrograAvanzada.Viernes.MyLibraryJsonRepository.CRUD;

public class BorrowCrudRepository
{
    private readonly JsonDataStore _dataStore;

    public BorrowCrudRepository(string jsonPath = "database/json")
    {
        _dataStore = JsonDataStore.GetInstance(jsonPath);
    }

    public Task<Borrow> CreateAsync(Borrow borrow)
    {
        // Generate new ID
        var maxId = _dataStore.Borrows.Any() ? _dataStore.Borrows.Max(b => b.Id) : 0;
        borrow.Id = maxId + 1;
        
        _dataStore.Borrows.Add(borrow);
        return Task.FromResult(borrow);
    }

    public Task<Borrow> UpdateAsync(Borrow borrow)
    {
        var existingBorrow = _dataStore.Borrows.FirstOrDefault(b => b.Id == borrow.Id);
        if (existingBorrow != null)
        {
            existingBorrow.UserId = borrow.UserId;
            existingBorrow.BookCopyId = borrow.BookCopyId;
            existingBorrow.Status = borrow.Status;
            existingBorrow.BorrowDate = borrow.BorrowDate;
            existingBorrow.ReturnDate = borrow.ReturnDate;
        }
        return Task.FromResult(borrow);
    }

    public Task DeleteAsync(int id)
    {
        var borrow = _dataStore.Borrows.FirstOrDefault(b => b.Id == id);
        if (borrow != null)
        {
            _dataStore.Borrows.Remove(borrow);
        }
        return Task.CompletedTask;
    }

    public Task<Borrow?> GetByIdAsync(int id)
    {
        var borrow = _dataStore.Borrows.FirstOrDefault(b => b.Id == id);
        
        if (borrow != null)
        {
            borrow.User = _dataStore.Users.FirstOrDefault(u => u.Id == borrow.UserId);
            borrow.BookCopy = _dataStore.BookCopies.FirstOrDefault(bc => bc.Id == borrow.BookCopyId);
            if (borrow.BookCopy != null)
            {
                borrow.BookCopy.Book = _dataStore.Books.FirstOrDefault(b => b.Id == borrow.BookCopy.BookId);
            }
        }
        
        return Task.FromResult(borrow);
    }

    public Task<List<Borrow>> GetAllAsync()
    {
        var borrows = _dataStore.Borrows.ToList();
        
        foreach (var borrow in borrows)
        {
            borrow.User = _dataStore.Users.FirstOrDefault(u => u.Id == borrow.UserId);
            borrow.BookCopy = _dataStore.BookCopies.FirstOrDefault(bc => bc.Id == borrow.BookCopyId);
            if (borrow.BookCopy != null)
            {
                borrow.BookCopy.Book = _dataStore.Books.FirstOrDefault(b => b.Id == borrow.BookCopy.BookId);
            }
        }
        
        return Task.FromResult(borrows);
    }

    public Task<List<Borrow>> GetByUserIdAsync(int userId)
    {
        var borrows = _dataStore.Borrows.Where(b => b.UserId == userId).ToList();
        
        foreach (var borrow in borrows)
        {
            borrow.BookCopy = _dataStore.BookCopies.FirstOrDefault(bc => bc.Id == borrow.BookCopyId);
            if (borrow.BookCopy != null)
            {
                borrow.BookCopy.Book = _dataStore.Books.FirstOrDefault(b => b.Id == borrow.BookCopy.BookId);
            }
        }
        
        return Task.FromResult(borrows);
    }

    public Task<List<Borrow>> GetByBookCopyIdAsync(int bookCopyId)
    {
        var borrows = _dataStore.Borrows.Where(b => b.BookCopyId == bookCopyId).ToList();
        
        foreach (var borrow in borrows)
        {
            borrow.User = _dataStore.Users.FirstOrDefault(u => u.Id == borrow.UserId);
        }
        
        return Task.FromResult(borrows);
    }
}
