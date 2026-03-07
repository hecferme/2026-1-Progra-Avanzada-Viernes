using PrograAvanzada.Viernes.MyLibraryDbModel.DbModels;
using PrograAvanzada.Viernes.MyLibrayApi.Interfaces;
using PrograAvanzada.Viernes.MyLibraryRepository.CRUD;

namespace PrograAvanzada.Viernes.MyLibrayApi.Adapters;

public class BookCrudRepositoryAdapter : IBookCrudRepository
{
    private readonly BookCrudRepository _repo;

    public BookCrudRepositoryAdapter(BookCrudRepository repo)
    {
        _repo = repo;
    }

    public Task<Book> CreateAsync(Book book) => _repo.CreateAsync(book);
    public Task<Book> UpdateAsync(Book book) => _repo.UpdateAsync(book);
    public Task DeleteAsync(int id) => _repo.DeleteAsync(id);
    public Task<Book?> GetByIdAsync(int id) => _repo.GetByIdAsync(id);
    public Task<List<Book>> GetAllAsync() => _repo.GetAllAsync();
}