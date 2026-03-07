using Microsoft.AspNetCore.Mvc;
using PrograAvanzada.Viernes.MyLibraryDbModel.DbModels;
using PrograAvanzada.Viernes.MyLibrayApi.Interfaces;

namespace PrograAvanzada.Viernes.MyLibrayApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthorsController : ControllerBase
{
    private readonly IAuthorCrudRepository _repository;

    public AuthorsController(IAuthorCrudRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    public async Task<ActionResult<List<Author>>> GetAll()
    {
        var authors = await _repository.GetAllAsync();
        return Ok(authors);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Author>> GetById(int id)
    {
        var author = await _repository.GetByIdAsync(id);
        if (author == null)
        {
            return NotFound();
        }
        return Ok(author);
    }

    [HttpPost]
    public async Task<ActionResult<Author>> Create(Author author)
    {
        var created = await _repository.CreateAsync(author);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, Author author)
    {
        if (id != author.Id)
        {
            return BadRequest();
        }
        var updated = await _repository.UpdateAsync(author);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _repository.DeleteAsync(id);
        return NoContent();
    }
}