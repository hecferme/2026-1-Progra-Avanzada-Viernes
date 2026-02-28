using PrograAvanzada.Viernes.MyLibraryDbModel.DbModels;
using PrograAvanzada.Viernes.MyLibraryJsonRepository.Data;
using PrograAvanzada.Viernes.MyLibraryRepository.DTOs;

namespace PrograAvanzada.Viernes.MyLibraryJsonRepository.Repositories;

public class UserRepository
{
    private readonly JsonDataStore _dataStore;

    public UserRepository(string jsonPath = "database/json")
    {
        _dataStore = JsonDataStore.GetInstance(jsonPath);
    }

    private List<UserDto> GetBaseQuery()
    {
        var result = new List<UserDto>();

        foreach (var user in _dataStore.Users)
        {
            foreach (var borrow in user.Borrows)
            {
                result.Add(new UserDto
                {
                    UserFullName = user.FullName,
                    UserEmail = user.Email,
                    BookName = borrow.BookCopy?.Book?.Title ?? "",
                    BorrowDate = borrow.BorrowDate,
                    BorrowReturnDate = borrow.ReturnDate,
                    BorrowStatus = borrow.Status
                });
            }
        }

        return result;
    }

    /// <summary>
    /// Query by user id
    /// </summary>
    public Task<List<UserDto>> GetByUserIdAsync(int userId)
    {
        var query = GetBaseQuery()
            .Where(u => _dataStore.Users.Any(u2 => u2.Id == userId && u2.Email == u.UserEmail))
            .ToList();

        return Task.FromResult(query);
    }

    /// <summary>
    /// Query by approximate user full name
    /// </summary>
    public Task<List<UserDto>> GetByApproximateFullNameAsync(string fullName)
    {
        var query = GetBaseQuery()
            .Where(u => u.UserFullName.Contains(fullName))
            .ToList();

        return Task.FromResult(query);
    }

    /// <summary>
    /// Query by approximate user email
    /// </summary>
    public Task<List<UserDto>> GetByApproximateEmailAsync(string email)
    {
        var query = GetBaseQuery()
            .Where(u => u.UserEmail != null && u.UserEmail.Contains(email))
            .ToList();

        return Task.FromResult(query);
    }

    /// <summary>
    /// Query by approximate book name
    /// </summary>
    public Task<List<UserDto>> GetByApproximateBookNameAsync(string bookName)
    {
        var query = GetBaseQuery()
            .Where(u => u.BookName.Contains(bookName))
            .ToList();

        return Task.FromResult(query);
    }

    /// <summary>
    /// Query by borrow date interval
    /// </summary>
    public Task<List<UserDto>> GetByBorrowDateIntervalAsync(DateTime startDate, DateTime endDate)
    {
        var query = GetBaseQuery()
            .Where(u => u.BorrowDate >= startDate && u.BorrowDate <= endDate)
            .ToList();

        return Task.FromResult(query);
    }

    /// <summary>
    /// Query by return date interval
    /// </summary>
    public Task<List<UserDto>> GetByReturnDateIntervalAsync(DateTime startDate, DateTime endDate)
    {
        var query = GetBaseQuery()
            .Where(u => u.BorrowReturnDate != null && u.BorrowReturnDate >= startDate && u.BorrowReturnDate <= endDate)
            .ToList();

        return Task.FromResult(query);
    }

    /// <summary>
    /// Query by exact borrow status
    /// </summary>
    public Task<List<UserDto>> GetByBorrowStatusAsync(byte status)
    {
        var query = GetBaseQuery()
            .Where(u => u.BorrowStatus == status)
            .ToList();

        return Task.FromResult(query);
    }
}
