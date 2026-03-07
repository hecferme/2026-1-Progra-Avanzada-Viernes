using Microsoft.AspNetCore.Mvc;
using PrograAvanzada.Viernes.MyLibraryDbModel.DbModels;
using PrograAvanzada.Viernes.MyLibrayApi.Interfaces;

namespace PrograAvanzada.Viernes.MyLibrayApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BorrowsController : ControllerBase
{
    private readonly IBorrowCrudRepository _repository;

    public BorrowsController(IBorrowCrudRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    public async Task<ActionResult<List<Borrow>>> GetAll()
    {
        var items = await _repository.GetAllAsync();
        return Ok(items);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Borrow>> GetById(int id)
    {
        var item = await _repository.GetByIdAsync(id);
        if (item == null)
        {
            return NotFound();
        }
        return Ok(item);
    }

    [HttpPost]
    public async Task<ActionResult<Borrow>> Create(Borrow item)
    {
        var created = await _repository.CreateAsync(item);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, Borrow item)
    {
        if (id != item.Id)
        {
            return BadRequest();
        }
        var updated = await _repository.UpdateAsync(item);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _repository.DeleteAsync(id);
        return NoContent();
    }
}