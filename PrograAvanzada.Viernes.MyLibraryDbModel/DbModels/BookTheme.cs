using System;
using System.Collections.Generic;

namespace PrograAvanzada.Viernes.MyLibraryDbModel.DbModels;

public partial class BookTheme
{
    public int book_id { get; set; }

    public int theme_id { get; set; }

    public virtual Book Book { get; set; } = null!;

    public virtual Theme Theme { get; set; } = null!;
}
