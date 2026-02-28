namespace PrograAvanzada.Viernes.MyLibraryRepository.DTOs;

public class UserDto
{
    public string UserFullName { get; set; } = string.Empty;
    public string? UserEmail { get; set; }
    public string BookName { get; set; } = string.Empty;
    public DateTime BorrowDate { get; set; }
    public DateTime? BorrowReturnDate { get; set; }
    public byte BorrowStatus { get; set; }
}
