using Microsoft.EntityFrameworkCore;
using PrograAvanzada.Viernes.MyLibraryDbModel.DbModels;

namespace PrograAvanzada.Viernes.MyLibraryRepository.CRUD;

public class AuthorCrudRepository
{
    private readonly ViernesContext _context;

    public AuthorCrudRepository(ViernesContext context)
    {
        _context = context;
    }

    public async Task<Author> CreateAsync(Author author)
    {
        _context.Authors.Add(author);
        await _context.SaveChangesAsync();
        return author;
    }

    public async Task<Author> UpdateAsync(Author author)
    {
        _context.Authors.Update(author);
        await _context.SaveChangesAsync();
        return author;
    }

    public async Task DeleteAsync(int id)
    {
        var author = await _context.Authors.FindAsync(id);
        if (author != null)
        {
            _context.Authors.Remove(author);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<Author?> GetByIdAsync(int id)
    {
        return await _context.Authors.FindAsync(id);
    }

    public async Task<List<Author>> GetAllAsync()
    {
        return await _context.Authors.ToListAsync();
    }
}
