using Microsoft.EntityFrameworkCore;
using PrograAvanzada.Viernes.MyLibraryDbModel.DbModels;
using PrograAvanzada.Viernes.MyLibraryRepository.DTOs;

namespace PrograAvanzada.Viernes.MyLibraryRepository.Repositories;

public class UserRepository
{
    private readonly ViernesContext _context;

    public UserRepository(ViernesContext context)
    {
        _context = context;
    }

    private IQueryable<UserDto> GetBaseQuery()
    {
        return _context.Users
            .Include(u => u.Borrows).ThenInclude(b => b.BookCopy).ThenInclude(bc => bc.Book)
            .SelectMany(u => u.Borrows, (u, b) => new { User = u, Borrow = b })
            .Select(x => new UserDto
            {
                UserFullName = x.User.FullName,
                UserEmail = x.User.Email,
                BookName = x.Borrow.BookCopy.Book.Title,
                BorrowDate = x.Borrow.BorrowDate,
                BorrowReturnDate = x.Borrow.ReturnDate,
                BorrowStatus = x.Borrow.Status
            });
    }

    /// <summary>
    /// Query by user id
    /// </summary>
    public async Task<List<UserDto>> GetByUserIdAsync(int userId)
    {
        // Filter the IQueryable BEFORE calling ToListAsync
        return await GetBaseQuery()
            .Where(u => _context.Users.Any(u2 => u2.Id == userId && u2.Email == u.UserEmail))
            .ToListAsync();
    }

    /// <summary>
    /// Query by approximate user full name
    /// </summary>
    public async Task<List<UserDto>> GetByApproximateFullNameAsync(string fullName)
    {
        return await GetBaseQuery()
            .Where(u => u.UserFullName.Contains(fullName))
            .ToListAsync();
    }

    /// <summary>
    /// Query by approximate user email
    /// </summary>
    public async Task<List<UserDto>> GetByApproximateEmailAsync(string email)
    {
        return await GetBaseQuery()
            .Where(u => u.UserEmail != null && u.UserEmail.Contains(email))
            .ToListAsync();
    }

    /// <summary>
    /// Query by approximate book name
    /// </summary>
    public async Task<List<UserDto>> GetByApproximateBookNameAsync(string bookName)
    {
        return await GetBaseQuery()
            .Where(u => u.BookName.Contains(bookName))
            .ToListAsync();
    }

    /// <summary>
    /// Query by borrow date interval
    /// </summary>
    public async Task<List<UserDto>> GetByBorrowDateIntervalAsync(DateTime startDate, DateTime endDate)
    {
        return await GetBaseQuery()
            .Where(u => u.BorrowDate >= startDate && u.BorrowDate <= endDate)
            .ToListAsync();
    }

    /// <summary>
    /// Query by return date interval
    /// </summary>
    public async Task<List<UserDto>> GetByReturnDateIntervalAsync(DateTime startDate, DateTime endDate)
    {
        return await GetBaseQuery()
            .Where(u => u.BorrowReturnDate != null && u.BorrowReturnDate >= startDate && u.BorrowReturnDate <= endDate)
            .ToListAsync();
    }

    /// <summary>
    /// Query by exact borrow status
    /// </summary>
    public async Task<List<UserDto>> GetByBorrowStatusAsync(byte status)
    {
        return await GetBaseQuery()
            .Where(u => u.BorrowStatus == status)
            .ToListAsync();
    }
}
