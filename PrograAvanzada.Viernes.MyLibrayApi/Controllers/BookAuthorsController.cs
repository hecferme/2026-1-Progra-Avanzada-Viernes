using Microsoft.AspNetCore.Mvc;
using PrograAvanzada.Viernes.MyLibraryDbModel.DbModels;
using PrograAvanzada.Viernes.MyLibrayApi.Interfaces;

namespace PrograAvanzada.Viernes.MyLibrayApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BookAuthorsController : ControllerBase
{
    private readonly IBookAuthorCrudRepository _repository;

    public BookAuthorsController(IBookAuthorCrudRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    public async Task<ActionResult<List<BookAuthor>>> GetAll()
    {
        var items = await _repository.GetAllAsync();
        return Ok(items);
    }

    [HttpGet("{bookId}/{authorId}")]
    public async Task<ActionResult<BookAuthor>> GetById(int bookId, int authorId)
    {
        var item = await _repository.GetByIdAsync(bookId, authorId);
        if (item == null)
        {
            return NotFound();
        }
        return Ok(item);
    }

    [HttpPost]
    public async Task<ActionResult<BookAuthor>> Create(BookAuthor item)
    {
        var created = await _repository.CreateAsync(item);
        return CreatedAtAction(nameof(GetById), new { bookId = created.BookId, authorId = created.AuthorId }, created);
    }

    [HttpPut("{bookId}/{authorId}")]
    public async Task<IActionResult> Update(int bookId, int authorId, BookAuthor item)
    {
        if (bookId != item.BookId || authorId != item.AuthorId)
        {
            return BadRequest();
        }
        var updated = await _repository.UpdateAsync(item);
        return NoContent();
    }

    [HttpDelete("{bookId}/{authorId}")]
    public async Task<IActionResult> Delete(int bookId, int authorId)
    {
        await _repository.DeleteAsync(bookId, authorId);
        return NoContent();
    }
}