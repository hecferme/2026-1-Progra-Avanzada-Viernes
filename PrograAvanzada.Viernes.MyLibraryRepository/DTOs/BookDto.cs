namespace PrograAvanzada.Viernes.MyLibraryRepository.DTOs;

public class BookDto
{
    public int BookId { get; set; }
    public string BookTitle { get; set; } = string.Empty;
    public string? BookIsbn { get; set; }
    public string BookAuthorName { get; set; } = string.Empty;
    public string BookThemes { get; set; } = string.Empty;
    public int BookCopyId { get; set; }
    public byte BookCopyStatus { get; set; }
}
