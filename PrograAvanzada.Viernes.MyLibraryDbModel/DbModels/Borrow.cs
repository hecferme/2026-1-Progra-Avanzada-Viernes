using System;
using System.Collections.Generic;

namespace PrograAvanzada.Viernes.MyLibraryDbModel.DbModels;

public partial class Borrow
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public int BookCopyId { get; set; }

    public DateTime BorrowDate { get; set; }

    public DateTime? ReturnDate { get; set; }

    public byte Status { get; set; }

    public virtual BookCopy BookCopy { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
