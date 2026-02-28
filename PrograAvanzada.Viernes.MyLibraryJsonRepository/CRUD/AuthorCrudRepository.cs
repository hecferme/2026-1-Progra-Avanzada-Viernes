using PrograAvanzada.Viernes.MyLibraryDbModel.DbModels;
using PrograAvanzada.Viernes.MyLibraryJsonRepository.Data;

namespace PrograAvanzada.Viernes.MyLibraryJsonRepository.CRUD;

public class AuthorCrudRepository
{
    private readonly JsonDataStore _dataStore;

    public AuthorCrudRepository(string jsonPath = "database/json")
    {
        _dataStore = JsonDataStore.GetInstance(jsonPath);
    }

    public Task<Author> CreateAsync(Author author)
    {
        // Generate new ID
        var maxId = _dataStore.Authors.Any() ? _dataStore.Authors.Max(a => a.Id) : 0;
        author.Id = maxId + 1;
        
        _dataStore.Authors.Add(author);
        return Task.FromResult(author);
    }

    public Task<Author> UpdateAsync(Author author)
    {
        var existingAuthor = _dataStore.Authors.FirstOrDefault(a => a.Id == author.Id);
        if (existingAuthor != null)
        {
            existingAuthor.Name = author.Name;
        }
        return Task.FromResult(author);
    }

    public Task DeleteAsync(int id)
    {
        var author = _dataStore.Authors.FirstOrDefault(a => a.Id == id);
        if (author != null)
        {
            _dataStore.Authors.Remove(author);
        }
        return Task.CompletedTask;
    }

    public Task<Author?> GetByIdAsync(int id)
    {
        var author = _dataStore.Authors.FirstOrDefault(a => a.Id == id);
        return Task.FromResult(author);
    }

    public Task<List<Author>> GetAllAsync()
    {
        var authors = _dataStore.Authors.ToList();
        return Task.FromResult(authors);
    }
}
