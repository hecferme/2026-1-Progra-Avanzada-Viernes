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
        var authorsData = JsonSerializer.Deserialize<List<AuthorJsonModel>>(authorsJson, options) ?? new List<AuthorJsonModel>();
        Authors = authorsData.Select(a => new Author
        {
            Id = a.Id,
            Name = a.Name
        }).ToList();

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
        var themesData = JsonSerializer.Deserialize<List<ThemeJsonModel>>(themesJson, options) ?? new List<ThemeJsonModel>();
        Themes = themesData.Select(t => new Theme
        {
            Id = t.Id,
            Name = t.Name
        }).ToList();

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
        var bookAuthorsData = JsonSerializer.Deserialize<List<BookAuthorJsonModel>>(bookAuthorsJson, options) ?? new List<BookAuthorJsonModel>();
        BookAuthors = bookAuthorsData.Select(ba => new BookAuthor
        {
            BookId = ba.BookId,
            AuthorId = ba.AuthorId
        }).ToList();

        // Load BookThemes
        var bookThemesJson = File.ReadAllText(Path.Combine(_jsonPath, "book_themes.json"));
        var bookThemesData = JsonSerializer.Deserialize<List<BookThemeJsonModel>>(bookThemesJson, options) ?? new List<BookThemeJsonModel>();
        BookThemes = bookThemesData.Select(bt => new BookTheme
        {
            book_id = bt.book_id,
            theme_id = bt.theme_id
        }).ToList();

        // Set up navigation properties
        SetUpNavigationProperties();
    }

    public void SaveData()
    {
        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            WriteIndented = true
        };

        // Save Authors
        var authorsData = Authors.Select(a => new AuthorJsonModel
        {
            Id = a.Id,
            Name = a.Name
        }).ToList();
        var authorsJson = JsonSerializer.Serialize(authorsData, options);
        File.WriteAllText(Path.Combine(_jsonPath, "authors.json"), authorsJson);

        // Save Books
        var booksData = Books.Select(b => new BookJsonModel
        {
            Id = b.Id,
            Title = b.Title,
            Isbn = b.Isbn,
            PublishedDate = b.PublishedDate?.ToString("yyyy-MM-dd")
        }).ToList();
        var booksJson = JsonSerializer.Serialize(booksData, options);
        File.WriteAllText(Path.Combine(_jsonPath, "books.json"), booksJson);

        // Save Users
        var usersData = Users.Select(u => new UserJsonModel
        {
            Id = u.Id,
            FullName = u.FullName,
            Email = u.Email
        }).ToList();
        var usersJson = JsonSerializer.Serialize(usersData, options);
        File.WriteAllText(Path.Combine(_jsonPath, "users.json"), usersJson);

        // Save Themes
        var themesData = Themes.Select(t => new ThemeJsonModel
        {
            Id = t.Id,
            Name = t.Name
        }).ToList();
        var themesJson = JsonSerializer.Serialize(themesData, options);
        File.WriteAllText(Path.Combine(_jsonPath, "themes.json"), themesJson);

        // Save BookCopies
        var bookCopiesData = BookCopies.Select(bc => new BookCopyJsonModel
        {
            Id = bc.Id,
            BookId = bc.BookId,
            Barcode = bc.Barcode,
            Status = bc.Status
        }).ToList();
        var bookCopiesJson = JsonSerializer.Serialize(bookCopiesData, options);
        File.WriteAllText(Path.Combine(_jsonPath, "book_copies.json"), bookCopiesJson);

        // Save Borrows
        var borrowsData = Borrows.Select(b => new BorrowJsonModel
        {
            Id = b.Id,
            UserId = b.UserId,
            BookCopyId = b.BookCopyId,
            Status = b.Status,
            BorrowDate = b.BorrowDate.ToString("yyyy-MM-ddTHH:mm:ss"),
            ReturnDate = b.ReturnDate?.ToString("yyyy-MM-ddTHH:mm:ss")
        }).ToList();
        var borrowsJson = JsonSerializer.Serialize(borrowsData, options);
        File.WriteAllText(Path.Combine(_jsonPath, "borrows.json"), borrowsJson);

        // Save BookAuthors
        var bookAuthorsData = BookAuthors.Select(ba => new BookAuthorJsonModel
        {
            BookId = ba.BookId,
            AuthorId = ba.AuthorId
        }).ToList();
        var bookAuthorsJson = JsonSerializer.Serialize(bookAuthorsData, options);
        File.WriteAllText(Path.Combine(_jsonPath, "book_authors.json"), bookAuthorsJson);

        // Save BookThemes
        var bookThemesData = BookThemes.Select(bt => new BookThemeJsonModel
        {
            book_id = bt.book_id,
            theme_id = bt.theme_id
        }).ToList();
        var bookThemesJson = JsonSerializer.Serialize(bookThemesData, options);
        File.WriteAllText(Path.Combine(_jsonPath, "book_themes.json"), bookThemesJson);
    }

    private void SetUpNavigationProperties()
    {
        // Set up Book -> BookAuthors
        foreach (var book in Books)
        {
            book.BookAuthors = BookAuthors.Where(ba => ba.BookId == book.Id).ToList();
            book.BookThemes = BookThemes.Where(bt => bt.book_id == book.Id).ToList();
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

        // Set up Theme -> BookThemes (forward ref only, skip back-refs to break cycle)
        foreach (var theme in Themes)
        {
            theme.BookThemes = BookThemes.Where(bt => bt.theme_id == theme.Id).ToList();
        }
        // Note: BookTheme.Theme and BookTheme.Book remain null to prevent serialization cycles
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

    private class AuthorJsonModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }

    private class ThemeJsonModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }

    private class BookAuthorJsonModel
    {
        public int Id { get; set; }
        public int BookId { get; set; }
        public int AuthorId { get; set; }
    }

    private class BookThemeJsonModel
    {
        public int Id { get; set; }
        public int book_id { get; set; }
        public int theme_id { get; set; }
    }
}
