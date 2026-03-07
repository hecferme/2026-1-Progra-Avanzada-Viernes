using PrograAvanzada.Viernes.MyLibraryRepository.DTOs;

namespace PrograAvanzada.Viernes.MyLibrayApi.Interfaces;

public interface IBookRepository
{
    Task<List<BookDto>> GetByBookIdAsync(int bookId);
    Task<List<BookDto>> GetByApproximateTitleAsync(string title);
    Task<List<BookDto>> GetByExactIsbnAsync(string isbn);
    Task<List<BookDto>> GetByApproximateAuthorNameAsync(string authorName);
    Task<List<BookDto>> GetByExactThemeAsync(string theme);
    Task<List<BookDto>> GetByBookCopyStatusAsync(byte status);
}