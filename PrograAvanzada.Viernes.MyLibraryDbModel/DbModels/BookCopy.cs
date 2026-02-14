using System;
using System.Collections.Generic;

namespace PrograAvanzada.Viernes.MyLibraryDbModel.DbModels;

public partial class BookCopy
{
    public int Id { get; set; }

    public int BookId { get; set; }

    public string? Barcode { get; set; }

    public byte Status { get; set; }

    public virtual Book Book { get; set; } = null!;

    public virtual ICollection<Borrow> Borrows { get; set; } = new List<Borrow>();
}
