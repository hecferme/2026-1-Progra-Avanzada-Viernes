using PrograAvanzada.Viernes.MyLibraryRepository.DTOs;
using PrograAvanzada.Viernes.MyLibrayApi.Interfaces;
using PrograAvanzada.Viernes.MyLibraryJsonRepository.Repositories;

namespace PrograAvanzada.Viernes.MyLibrayApi.Adapters;

public class UserJsonRepositoryAdapter : IUserRepository
{
    private readonly UserRepository _repo;

    public UserJsonRepositoryAdapter(UserRepository repo)
    {
        _repo = repo;
    }

    public Task<List<UserDto>> GetByUserIdAsync(int userId) => _repo.GetByUserIdAsync(userId);
    public Task<List<UserDto>> GetByApproximateFullNameAsync(string fullName) => _repo.GetByApproximateFullNameAsync(fullName);
    public Task<List<UserDto>> GetByApproximateEmailAsync(string email) => _repo.GetByApproximateEmailAsync(email);
    public Task<List<UserDto>> GetByApproximateBookNameAsync(string bookName) => _repo.GetByApproximateBookNameAsync(bookName);
    public Task<List<UserDto>> GetByBorrowDateIntervalAsync(DateTime startDate, DateTime endDate) => _repo.GetByBorrowDateIntervalAsync(startDate, endDate);
    public Task<List<UserDto>> GetByReturnDateIntervalAsync(DateTime startDate, DateTime endDate) => _repo.GetByReturnDateIntervalAsync(startDate, endDate);
    public Task<List<UserDto>> GetByBorrowStatusAsync(byte status) => _repo.GetByBorrowStatusAsync(status);
}