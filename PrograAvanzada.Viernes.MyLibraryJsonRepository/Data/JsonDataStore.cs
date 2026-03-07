using System.Text.Json;
using PrograAvanzada.Viernes.MyLibraryDbModel.DbModels;

namespace PrograAvanzada.Viernes.MyLibraryJsonRepository.Data;

public class JsonDataStore
{
    private static JsonDataStore? _instance;
    private static readonly object _lock = new object();
    
    private readonly string _jsonPath;
    
    public List<Author> Authors { get; private set; } = new();
    public List<Book> Books { get; private set; } = new();
    public List<User> Users { get; private set; } = new();
    public List<Theme> Themes { get; private set; } = new();
    public List<BookCopy> BookCopies { get; private set; } = new();
    public List<Borrow> Borrows { get; private set; } = new();
    public List<BookAuthor> BookAuthors { get; private set; } = new();
    public List<BookTheme> BookThemes { get; private set; } = new();

    private JsonDataStore(string jsonPath)
    {
        _jsonPath = jsonPath;
        LoadData();
    }

    public static JsonDataStore GetInstance(string jsonPath = "database/json")
    {
        if (_instance == null)
        {
            lock (_lock)
            {
                if (_instance == null)
                {
                    _instance = new JsonDataStore(jsonPath);
                }
            }
        }
        return _instance;
    }

    public static void ResetInstance()
    {
        lock (_lock)
        {
            _instance = null;
        }
    }

    private void LoadData()
    {
        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        // Load Authors
        var authorsJson = File.ReadAllText(Path.Combine(_jsonPath, "authors.json"));
        Authors = JsonSerializer.Deserialize<List<Author>>(authorsJson, options) ?? new List<Author>();

        // Load Books
        var booksJson = File.ReadAllText(Path.Combine(_jsonPath, "books.json"));
        var booksData = JsonSerializer.Deserialize<List<BookJsonModel>>(booksJson, options) ?? new List<BookJsonModel>();
        Books = booksData.Select(b => new Book
        {
            Id = b.Id,
            Title = b.Title,
            Isbn = b.Isbn,
            PublishedDate = DateTime.TryParse(b.PublishedDate, out var date) ? DateOnly.FromDateTime(date) : null
        }).ToList();

        // Load Users
        var usersJson = File.ReadAllText(Path.Combine(_jsonPath, "users.json"));
        var usersData = JsonSerializer.Deserialize<List<UserJsonModel>>(usersJson, options) ?? new List<UserJsonModel>();
        Users = usersData.Select(u => new User
        {
            Id = u.Id,
            FullName = u.FullName,
            Email = u.Email
        }).ToList();

        // Load Themes
        var themesJson = File.ReadAllText(Path.Combine(_jsonPath, "themes.json"));
        Themes = JsonSerializer.Deserialize<List<Theme>>(themesJson, options) ?? new List<Theme>();

        // Load BookCopies
        var bookCopiesJson = File.ReadAllText(Path.Combine(_jsonPath, "book_copies.json"));
        var bookCopiesData = JsonSerializer.Deserialize<List<BookCopyJsonModel>>(bookCopiesJson, options) ?? new List<BookCopyJsonModel>();
        BookCopies = bookCopiesData.Select(bc => new BookCopy
        {
            Id = bc.Id,
            BookId = bc.BookId,
            Barcode = bc.Barcode,
            Status = bc.Status
        }).ToList();

        // Load Borrows
        var borrowsJson = File.ReadAllText(Path.Combine(_jsonPath, "borrows.json"));
        var borrowsData = JsonSerializer.Deserialize<List<BorrowJsonModel>>(borrowsJson, options) ?? new List<BorrowJsonModel>();
        Borrows = borrowsData.Select(b => new Borrow
        {
            Id = b.Id,
            UserId = b.UserId,
            BookCopyId = b.BookCopyId,
            Status = b.Status,
            BorrowDate = DateTime.TryParse(b.BorrowDate, out var borrowDate) ? borrowDate : DateTime.MinValue,
            ReturnDate = DateTime.TryParse(b.ReturnDate, out var returnDate) ? returnDate : null
        }).ToList();

        // Load BookAuthors
        var bookAuthorsJson = File.ReadAllText(Path.Combine(_jsonPath, "book_authors.json"));
        BookAuthors = JsonSerializer.Deserialize<List<BookAuthor>>(bookAuthorsJson, options) ?? new List<BookAuthor>();

        // Load BookThemes
        var bookThemesJson = File.ReadAllText(Path.Combine(_jsonPath, "book_themes.json"));
        BookThemes = JsonSerializer.Deserialize<List<BookTheme>>(bookThemesJson, options) ?? new List<BookTheme>();

        // Set up navigation properties
        SetUpNavigationProperties();
    }

    private void SetUpNavigationProperties()
    {
        // Set up Book -> BookAuthors
        foreach (var book in Books)
        {
            book.BookAuthors = BookAuthors.Where(ba => ba.BookId == book.Id).ToList();
            book.BookThemes = BookThemes.Where(bt => bt.BookId == book.Id).ToList();
            book.BookCopies = BookCopies.Where(bc => bc.BookId == book.Id).ToList();
        }

        // Set up BookCopy -> Book
        foreach (var bookCopy in BookCopies)
        {
            bookCopy.Book = Books.FirstOrDefault(b => b.Id == bookCopy.BookId) ?? new Book { Id = bookCopy.BookId, Title = "Unknown" };
            bookCopy.Borrows = Borrows.Where(b => b.BookCopyId == bookCopy.Id).ToList();
        }

        // Set up User -> Borrows
        foreach (var user in Users)
        {
            user.Borrows = Borrows.Where(b => b.UserId == user.Id).ToList();
        }

        // Set up Borrow -> User and BookCopy
        foreach (var borrow in Borrows)
        {
            borrow.User = Users.FirstOrDefault(u => u.Id == borrow.UserId) ?? new User { Id = borrow.UserId, FullName = "Unknown" };
            borrow.BookCopy = BookCopies.FirstOrDefault(bc => bc.Id == borrow.BookCopyId) ?? new BookCopy { Id = borrow.BookCopyId };
        }

        // Set up BookAuthor -> Author and Book
        foreach (var bookAuthor in BookAuthors)
        {
            bookAuthor.Author = Authors.FirstOrDefault(a => a.Id == bookAuthor.AuthorId) ?? new Author { Id = bookAuthor.AuthorId, Name = "Unknown" };
            bookAuthor.Book = Books.FirstOrDefault(b => b.Id == bookAuthor.BookId) ?? new Book { Id = bookAuthor.BookId, Title = "Unknown" };
        }

        // Set up BookTheme -> Theme and Book
        foreach (var bookTheme in BookThemes)
        {
            bookTheme.Theme = Themes.FirstOrDefault(t => t.Id == bookTheme.ThemeId) ?? new Theme { Id = bookTheme.ThemeId, Name = "Unknown" };
            bookTheme.Book = Books.FirstOrDefault(b => b.Id == bookTheme.BookId) ?? new Book { Id = bookTheme.BookId, Title = "Unknown" };
        }

        // Set up Theme -> BookThemes
        foreach (var theme in Themes)
        {
            theme.BookThemes = BookThemes.Where(bt => bt.ThemeId == theme.Id).ToList();
        }
    }

    // JSON model classes for deserialization
    private class BookJsonModel
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Isbn { get; set; }
        public string? PublishedDate { get; set; }
    }

    private class UserJsonModel
    {
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string? Email { get; set; }
    }

    private class BookCopyJsonModel
    {
        public int Id { get; set; }
        public int BookId { get; set; }
        public string? Barcode { get; set; }
        public byte Status { get; set; }
    }

    private class BorrowJsonModel
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int BookCopyId { get; set; }
        public byte Status { get; set; }
        public string? BorrowDate { get; set; }
        public string? ReturnDate { get; set; }
    }
}
