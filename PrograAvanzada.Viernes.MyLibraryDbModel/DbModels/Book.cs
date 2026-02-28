﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace PrograAvanzada.Viernes.MyLibraryDbModel.DbModels;

public partial class Book
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public string? Isbn { get; set; }

    public DateOnly? PublishedDate { get; set; }

    public virtual ICollection<BookCopy> BookCopies { get; set; } = new List<BookCopy>();

    public virtual ICollection<BookAuthor> BookAuthors { get; set; } = new List<BookAuthor>();

    public virtual ICollection<BookTheme> BookThemes { get; set; } = new List<BookTheme>();

    /// <summary>
    /// Non-mapped property that returns all theme names concatenated by comma
    /// </summary>
    [NotMapped]
    public string Themes
    {
        get
        {
            if (BookThemes == null || !BookThemes.Any())
                return string.Empty;
            
            return string.Join(", ", BookThemes.Select(bt => bt.Theme?.Name ?? string.Empty).Where(n => !string.IsNullOrEmpty(n)));
        }
    }
}
