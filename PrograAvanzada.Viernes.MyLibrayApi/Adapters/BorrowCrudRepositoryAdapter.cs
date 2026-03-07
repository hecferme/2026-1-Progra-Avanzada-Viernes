using PrograAvanzada.Viernes.MyLibraryDbModel.DbModels;
using PrograAvanzada.Viernes.MyLibrayApi.Interfaces;
using PrograAvanzada.Viernes.MyLibraryRepository.CRUD;

namespace PrograAvanzada.Viernes.MyLibrayApi.Adapters;

public class BorrowCrudRepositoryAdapter : IBorrowCrudRepository
{
    private readonly BorrowCrudRepository _repo;

    public BorrowCrudRepositoryAdapter(BorrowCrudRepository repo)
    {
        _repo = repo;
    }

    public Task<Borrow> CreateAsync(Borrow borrow) => _repo.CreateAsync(borrow);
    public Task<Borrow> UpdateAsync(Borrow borrow) => _repo.UpdateAsync(borrow);
    public Task DeleteAsync(int id) => _repo.DeleteAsync(id);
    public Task<Borrow?> GetByIdAsync(int id) => _repo.GetByIdAsync(id);
    public Task<List<Borrow>> GetAllAsync() => _repo.GetAllAsync();
}