using Microsoft.AspNetCore.Mvc;
using PrograAvanzada.Viernes.MyLibraryDbModel.DbModels;
using PrograAvanzada.Viernes.MyLibraryRepository.DTOs;
using PrograAvanzada.Viernes.MyLibrayApi.Interfaces;

namespace PrograAvanzada.Viernes.MyLibrayApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BooksController : ControllerBase
{
    private readonly IBookCrudRepository _crudRepository;
    private readonly IBookRepository _queryRepository;

    public BooksController(IBookCrudRepository crudRepository, IBookRepository queryRepository)
    {
        _crudRepository = crudRepository;
        _queryRepository = queryRepository;
    }

    // CRUD operations
    [HttpGet]
    public async Task<ActionResult<List<Book>>> GetAll()
    {
        var books = await _crudRepository.GetAllAsync();
        return Ok(books);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Book>> GetById(int id)
    {
        var book = await _crudRepository.GetByIdAsync(id);
        if (book == null)
        {
            return NotFound();
        }
        return Ok(book);
    }

    [HttpPost]
    public async Task<ActionResult<Book>> Create(Book book)
    {
        var created = await _crudRepository.CreateAsync(book);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, Book book)
    {
        if (id != book.Id)
        {
            return BadRequest();
        }
        var updated = await _crudRepository.UpdateAsync(book);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _crudRepository.DeleteAsync(id);
        return NoContent();
    }

    // Query operations
    [HttpGet("query/bookId/{bookId}")]
    public async Task<ActionResult<List<BookDto>>> GetByBookId(int bookId)
    {
        var results = await _queryRepository.GetByBookIdAsync(bookId);
        return Ok(results);
    }

    [HttpGet("query/title/{title}")]
    public async Task<ActionResult<List<BookDto>>> GetByApproximateTitle(string title)
    {
        var results = await _queryRepository.GetByApproximateTitleAsync(title);
        return Ok(results);
    }

    [HttpGet("query/isbn/{isbn}")]
    public async Task<ActionResult<List<BookDto>>> GetByExactIsbn(string isbn)
    {
        var results = await _queryRepository.GetByExactIsbnAsync(isbn);
        return Ok(results);
    }

    [HttpGet("query/author/{authorName}")]
    public async Task<ActionResult<List<BookDto>>> GetByApproximateAuthorName(string authorName)
    {
        var results = await _queryRepository.GetByApproximateAuthorNameAsync(authorName);
        return Ok(results);
    }

    [HttpGet("query/theme/{theme}")]
    public async Task<ActionResult<List<BookDto>>> GetByExactTheme(string theme)
    {
        var results = await _queryRepository.GetByExactThemeAsync(theme);
        return Ok(results);
    }

    [HttpGet("query/status/{status}")]
    public async Task<ActionResult<List<BookDto>>> GetByBookCopyStatus(byte status)
    {
        var results = await _queryRepository.GetByBookCopyStatusAsync(status);
        return Ok(results);
    }
}