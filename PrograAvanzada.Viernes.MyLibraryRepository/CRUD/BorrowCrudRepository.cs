using Microsoft.EntityFrameworkCore;
using PrograAvanzada.Viernes.MyLibraryDbModel.DbModels;

namespace PrograAvanzada.Viernes.MyLibraryRepository.CRUD;

public class BorrowCrudRepository
{
    private readonly ViernesContext _context;

    public BorrowCrudRepository(ViernesContext context)
    {
        _context = context;
    }

    public async Task<Borrow> CreateAsync(Borrow borrow)
    {
        _context.Borrows.Add(borrow);
        await _context.SaveChangesAsync();
        return borrow;
    }

    public async Task<Borrow> UpdateAsync(Borrow borrow)
    {
        _context.Borrows.Update(borrow);
        await _context.SaveChangesAsync();
        return borrow;
    }

    public async Task DeleteAsync(int id)
    {
        var borrow = await _context.Borrows.FindAsync(id);
        if (borrow != null)
        {
            _context.Borrows.Remove(borrow);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<Borrow?> GetByIdAsync(int id)
    {
        return await _context.Borrows
            .Include(b => b.User)
            .Include(b => b.BookCopy).ThenInclude(bc => bc.Book)
            .FirstOrDefaultAsync(b => b.Id == id);
    }

    public async Task<List<Borrow>> GetAllAsync()
    {
        return await _context.Borrows
            .Include(b => b.User)
            .Include(b => b.BookCopy).ThenInclude(bc => bc.Book)
            .ToListAsync();
    }

    public async Task<List<Borrow>> GetByUserIdAsync(int userId)
    {
        return await _context.Borrows
            .Include(b => b.BookCopy).ThenInclude(bc => bc.Book)
            .Where(b => b.UserId == userId)
            .ToListAsync();
    }

    public async Task<List<Borrow>> GetByBookCopyIdAsync(int bookCopyId)
    {
        return await _context.Borrows
            .Include(b => b.User)
            .Where(b => b.BookCopyId == bookCopyId)
            .ToListAsync();
    }
}
