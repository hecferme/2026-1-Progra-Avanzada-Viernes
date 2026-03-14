using Microsoft.AspNetCore.Mvc;
using PrograAvanzada.Viernes.MyLibraryDbModel.DbModels;
using PrograAvanzada.Viernes.MyLibrayApi.Interfaces;

namespace PrograAvanzada.Viernes.MyLibrayApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BookThemesController : ControllerBase
{
    private readonly IBookThemeCrudRepository _repository;

    public BookThemesController(IBookThemeCrudRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    public async Task<ActionResult<List<BookTheme>>> GetAll()
    {
        var items = await _repository.GetAllAsync();
        return Ok(items);
    }

    [HttpGet("{bookId}/{themeId}")]
    public async Task<ActionResult<BookTheme>> GetById(int bookId, int themeId)
    {
        var item = await _repository.GetByIdAsync(bookId, themeId);
        if (item == null)
        {
            return NotFound();
        }
        return Ok(item);
    }

    [HttpPost]
    public async Task<ActionResult<BookTheme>> Create(BookTheme item)
    {
        var created = await _repository.CreateAsync(item);
        return CreatedAtAction(nameof(GetById), new { bookId = created.book_id, themeId = created.theme_id }, created);
    }

    [HttpPut("{bookId}/{themeId}")]
    public async Task<IActionResult> Update(int bookId, int themeId, BookTheme item)
    {
        if (bookId != item.book_id || themeId != item.theme_id)
        {
            return BadRequest();
        }
        var updated = await _repository.UpdateAsync(item);
        return NoContent();
    }

    [HttpDelete("{bookId}/{themeId}")]
    public async Task<IActionResult> Delete(int bookId, int themeId)
    {
        await _repository.DeleteAsync(bookId, themeId);
        return NoContent();
    }
}