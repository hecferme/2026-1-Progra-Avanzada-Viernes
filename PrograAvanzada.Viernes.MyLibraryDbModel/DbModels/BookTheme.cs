using System;
using System.Collections.Generic;

namespace PrograAvanzada.Viernes.MyLibraryDbModel.DbModels;

public partial class BookTheme
{
    public int BookId { get; set; }

    public int ThemeId { get; set; }

    public virtual Book Book { get; set; } = null!;

    public virtual Theme Theme { get; set; } = null!;
}
