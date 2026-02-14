using System;
using System.Collections.Generic;

namespace PrograAvanzada.Viernes.MyLibraryDbModel.DbModels;

public partial class Author
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<BookAuthor> BookAuthors { get; set; } = new List<BookAuthor>();
}
