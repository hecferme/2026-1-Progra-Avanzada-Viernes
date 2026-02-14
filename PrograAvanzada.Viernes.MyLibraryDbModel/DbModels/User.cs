using System;
using System.Collections.Generic;

namespace PrograAvanzada.Viernes.MyLibraryDbModel.DbModels;

public partial class User
{
    public int Id { get; set; }

    public string FullName { get; set; } = null!;

    public string? Email { get; set; }

    public virtual ICollection<Borrow> Borrows { get; set; } = new List<Borrow>();
}
