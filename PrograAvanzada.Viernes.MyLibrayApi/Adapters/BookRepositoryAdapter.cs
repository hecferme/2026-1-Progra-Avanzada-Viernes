using PrograAvanzada.Viernes.MyLibraryRepository.DTOs;
using PrograAvanzada.Viernes.MyLibrayApi.Interfaces;
using PrograAvanzada.Viernes.MyLibraryRepository.Repositories;

namespace PrograAvanzada.Viernes.MyLibrayApi.Adapters;

public class BookRepositoryAdapter : IBookRepository
{
    private readonly BookRepository _repo;

    public BookRepositoryAdapter(BookRepository repo)
    {
        _repo = repo;
    }

    public Task<List<BookDto>> GetByBookIdAsync(int bookId) => _repo.GetByBookIdAsync(bookId);
    public Task<List<BookDto>> GetByApproximateTitleAsync(string title) => _repo.GetByApproximateTitleAsync(title);
    public Task<List<BookDto>> GetByExactIsbnAsync(string isbn) => _repo.GetByExactIsbnAsync(isbn);
    public Task<List<BookDto>> GetByApproximateAuthorNameAsync(string authorName) => _repo.GetByApproximateAuthorNameAsync(authorName);
    public Task<List<BookDto>> GetByExactThemeAsync(string theme) => _repo.GetByExactThemeAsync(theme);
    public Task<List<BookDto>> GetByThemeIdAsync(int themeId) => _repo.GetByThemeIdAsync(themeId);
    public Task<List<BookDto>> GetByBookCopyStatusAsync(byte status) => _repo.GetByBookCopyStatusAsync(status);
}