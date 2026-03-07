using PrograAvanzada.Viernes.MyLibraryDbModel.DbModels;

namespace PrograAvanzada.Viernes.MyLibrayApi.Interfaces;

public interface IBorrowCrudRepository
{
    Task<Borrow> CreateAsync(Borrow borrow);
    Task<Borrow> UpdateAsync(Borrow borrow);
    Task DeleteAsync(int id);
    Task<Borrow?> GetByIdAsync(int id);
    Task<List<Borrow>> GetAllAsync();
}