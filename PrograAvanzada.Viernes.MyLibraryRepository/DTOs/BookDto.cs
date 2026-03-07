namespace PrograAvanzada.Viernes.MyLibraryRepository.DTOs;

public class BookDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Isbn { get; set; }
    public string BookAuthorName { get; set; } = string.Empty;
    public string BookThemes { get; set; } = string.Empty;
    public int BookCopyId { get; set; }
    public byte BookCopyStatus { get; set; }
}
