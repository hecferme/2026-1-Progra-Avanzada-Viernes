using PrograAvanzada.Viernes.MyLibraryDbModel.DbModels;
using PrograAvanzada.Viernes.MyLibrayApi.Interfaces;
using PrograAvanzada.Viernes.MyLibraryRepository.CRUD;

namespace PrograAvanzada.Viernes.MyLibrayApi.Adapters;

public class BookCopyCrudRepositoryAdapter : IBookCopyCrudRepository
{
    private readonly BookCopyCrudRepository _repo;

    public BookCopyCrudRepositoryAdapter(BookCopyCrudRepository repo)
    {
        _repo = repo;
    }

    public Task<BookCopy> CreateAsync(BookCopy bookCopy) => _repo.CreateAsync(bookCopy);
    public Task<BookCopy> UpdateAsync(BookCopy bookCopy) => _repo.UpdateAsync(bookCopy);
    public Task DeleteAsync(int id) => _repo.DeleteAsync(id);
    public Task<BookCopy?> GetByIdAsync(int id) => _repo.GetByIdAsync(id);
    public Task<List<BookCopy>> GetAllAsync() => _repo.GetAllAsync();
}