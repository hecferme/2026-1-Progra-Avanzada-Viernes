using System;
using System.Collections.Generic;

namespace PrograAvanzada.Viernes.MyLibraryDbModel.DbModels;

public partial class Theme
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<BookTheme> BookThemes { get; set; } = new List<BookTheme>();
}
