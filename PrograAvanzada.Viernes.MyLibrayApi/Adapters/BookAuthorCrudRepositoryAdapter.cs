using PrograAvanzada.Viernes.MyLibraryDbModel.DbModels;
using PrograAvanzada.Viernes.MyLibrayApi.Interfaces;
using PrograAvanzada.Viernes.MyLibraryRepository.CRUD;

namespace PrograAvanzada.Viernes.MyLibrayApi.Adapters;

public class BookAuthorCrudRepositoryAdapter : IBookAuthorCrudRepository
{
    private readonly BookAuthorCrudRepository _repo;

    public BookAuthorCrudRepositoryAdapter(BookAuthorCrudRepository repo)
    {
        _repo = repo;
    }

    public Task<BookAuthor> CreateAsync(BookAuthor bookAuthor) => _repo.CreateAsync(bookAuthor);
    public Task<BookAuthor> UpdateAsync(BookAuthor bookAuthor) => _repo.UpdateAsync(bookAuthor);
    public Task DeleteAsync(int bookId, int authorId) => _repo.DeleteAsync(bookId, authorId);
    public Task<BookAuthor?> GetByIdAsync(int bookId, int authorId) => _repo.GetByIdAsync(bookId, authorId);
    public Task<List<BookAuthor>> GetAllAsync() => _repo.GetAllAsync();
}