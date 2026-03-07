using Microsoft.AspNetCore.Mvc;
using PrograAvanzada.Viernes.MyLibraryDbModel.DbModels;
using PrograAvanzada.Viernes.MyLibraryRepository.DTOs;
using PrograAvanzada.Viernes.MyLibrayApi.Interfaces;

namespace PrograAvanzada.Viernes.MyLibrayApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUserCrudRepository _crudRepository;
    private readonly IUserRepository _queryRepository;

    public UsersController(IUserCrudRepository crudRepository, IUserRepository queryRepository)
    {
        _crudRepository = crudRepository;
        _queryRepository = queryRepository;
    }

    // CRUD operations
    [HttpGet]
    public async Task<ActionResult<List<User>>> GetAll()
    {
        var users = await _crudRepository.GetAllAsync();
        return Ok(users);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<User>> GetById(int id)
    {
        var user = await _crudRepository.GetByIdAsync(id);
        if (user == null)
        {
            return NotFound();
        }
        return Ok(user);
    }

    [HttpPost]
    public async Task<ActionResult<User>> Create(User user)
    {
        var created = await _crudRepository.CreateAsync(user);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, User user)
    {
        if (id != user.Id)
        {
            return BadRequest();
        }
        var updated = await _crudRepository.UpdateAsync(user);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _crudRepository.DeleteAsync(id);
        return NoContent();
    }

    // Query operations
    [HttpGet("query/userId/{userId}")]
    public async Task<ActionResult<List<UserDto>>> GetByUserId(int userId)
    {
        var results = await _queryRepository.GetByUserIdAsync(userId);
        return Ok(results);
    }

    [HttpGet("query/fullName/{fullName}")]
    public async Task<ActionResult<List<UserDto>>> GetByApproximateFullName(string fullName)
    {
        var results = await _queryRepository.GetByApproximateFullNameAsync(fullName);
        return Ok(results);
    }

    [HttpGet("query/email/{email}")]
    public async Task<ActionResult<List<UserDto>>> GetByApproximateEmail(string email)
    {
        var results = await _queryRepository.GetByApproximateEmailAsync(email);
        return Ok(results);
    }

    [HttpGet("query/bookName/{bookName}")]
    public async Task<ActionResult<List<UserDto>>> GetByApproximateBookName(string bookName)
    {
        var results = await _queryRepository.GetByApproximateBookNameAsync(bookName);
        return Ok(results);
    }

    [HttpGet("query/borrowDate/{startDate}/{endDate}")]
    public async Task<ActionResult<List<UserDto>>> GetByBorrowDateInterval(DateTime startDate, DateTime endDate)
    {
        var results = await _queryRepository.GetByBorrowDateIntervalAsync(startDate, endDate);
        return Ok(results);
    }

    [HttpGet("query/returnDate/{startDate}/{endDate}")]
    public async Task<ActionResult<List<UserDto>>> GetByReturnDateInterval(DateTime startDate, DateTime endDate)
    {
        var results = await _queryRepository.GetByReturnDateIntervalAsync(startDate, endDate);
        return Ok(results);
    }

    [HttpGet("query/borrowStatus/{status}")]
    public async Task<ActionResult<List<UserDto>>> GetByBorrowStatus(byte status)
    {
        var results = await _queryRepository.GetByBorrowStatusAsync(status);
        return Ok(results);
    }
}