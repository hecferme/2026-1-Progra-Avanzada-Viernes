using PrograAvanzada.Viernes.MyLibraryRepository.DTOs;

namespace PrograAvanzada.Viernes.MyLibrayApi.Interfaces;

public interface IUserRepository
{
    Task<List<UserDto>> GetByUserIdAsync(int userId);
    Task<List<UserDto>> GetByApproximateFullNameAsync(string fullName);
    Task<List<UserDto>> GetByApproximateEmailAsync(string email);
    Task<List<UserDto>> GetByApproximateBookNameAsync(string bookName);
    Task<List<UserDto>> GetByBorrowDateIntervalAsync(DateTime startDate, DateTime endDate);
    Task<List<UserDto>> GetByReturnDateIntervalAsync(DateTime startDate, DateTime endDate);
    Task<List<UserDto>> GetByBorrowStatusAsync(byte status);
}