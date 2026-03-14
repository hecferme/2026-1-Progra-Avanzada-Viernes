using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace PrograAvanzada.Viernes.MyLibraryDbModel.DbModels;

public partial class ViernesContext : DbContext
{
    public ViernesContext()
    {
    }

    public ViernesContext(DbContextOptions<ViernesContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Author> Authors { get; set; }

    public virtual DbSet<Book> Books { get; set; }

    public virtual DbSet<BookAuthor> BookAuthors { get; set; }

    public virtual DbSet<BookCopy> BookCopies { get; set; }

    public virtual DbSet<BookTheme> BookThemes { get; set; }

    public virtual DbSet<Borrow> Borrows { get; set; }

    public virtual DbSet<Theme> Themes { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Name=ConnectionStrings:MySqlServerConnectionString");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Author>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Authors__3213E83F11872B9F");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(300)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Book>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Books__3213E83F9FE62FE7");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Isbn)
                .HasMaxLength(50)
                .HasColumnName("isbn");
            entity.Property(e => e.PublishedDate).HasColumnName("published_date");
            entity.Property(e => e.Title)
                .HasMaxLength(400)
                .HasColumnName("title");
        });

        modelBuilder.Entity<BookAuthor>(entity =>
        {
            entity.HasKey(e => new { e.BookId, e.AuthorId });

            entity.ToTable("BookAuthors");

            entity.Property(e => e.BookId).HasColumnName("book_id");
            entity.Property(e => e.AuthorId).HasColumnName("author_id");

            entity.HasOne(d => d.Author).WithMany(p => p.BookAuthors)
                .HasForeignKey(d => d.AuthorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__BookAutho__autho__0C85DE4D");

            entity.HasOne(d => d.Book).WithMany(p => p.BookAuthors)
                .HasForeignKey(d => d.BookId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__BookAutho__book___0B91BA14");
        });

        modelBuilder.Entity<BookCopy>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__BookCopi__3213E83F2423A2FB");

            entity.HasIndex(e => e.BookId, "IX_BookCopies_BookId");

            entity.HasIndex(e => e.Barcode, "UQ__BookCopi__C16E36F82D1053AD").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Barcode)
                .HasMaxLength(100)
                .HasColumnName("barcode");
            entity.Property(e => e.BookId).HasColumnName("book_id");
            entity.Property(e => e.Status)
                .HasDefaultValue((byte)2)
                .HasColumnName("status");

            entity.HasOne(d => d.Book).WithMany(p => p.BookCopies)
                .HasForeignKey(d => d.BookId)
                .HasConstraintName("FK__BookCopie__book___7C4F7684");
        });

        modelBuilder.Entity<BookTheme>(entity =>
        {
            entity.HasKey(e => new { e.book_id, e.theme_id });

            entity.ToTable("BookThemes");

            entity.Property(e => e.book_id).HasColumnName("book_id");
            entity.Property(e => e.theme_id).HasColumnName("theme_id");

            entity.HasOne(d => d.Book).WithMany(p => p.BookThemes)
                .HasForeignKey(d => d.book_id)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__BookTheme__book___07C12930");

            entity.HasOne(d => d.Theme).WithMany(p => p.BookThemes)
                .HasForeignKey(d => d.theme_id)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__BookTheme__theme__08B54D69");
        });


        modelBuilder.Entity<Borrow>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Borrows__3213E83FB686B057");

            entity.ToTable(tb =>
                {
                    tb.HasTrigger("trg_Borrows_AfterInsert");
                    tb.HasTrigger("trg_Borrows_AfterUpdate");
                });

            entity.HasIndex(e => e.BookCopyId, "IX_Borrows_CopyId");

            entity.HasIndex(e => e.UserId, "IX_Borrows_UserId");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.BookCopyId).HasColumnName("book_copy_id");
            entity.Property(e => e.BorrowDate)
                .HasDefaultValueSql("(sysutcdatetime())")
                .HasColumnName("borrow_date");
            entity.Property(e => e.ReturnDate).HasColumnName("return_date");
            entity.Property(e => e.Status)
                .HasDefaultValue((byte)1)
                .HasColumnName("status");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.BookCopy).WithMany(p => p.Borrows)
                .HasForeignKey(d => d.BookCopyId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Borrows__book_co__02084FDA");

            entity.HasOne(d => d.User).WithMany(p => p.Borrows)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Borrows__user_id__01142BA1");
        });

        modelBuilder.Entity<Theme>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Themes__3213E83F7FF4C23E");

            entity.HasIndex(e => e.Name, "UQ__Themes__72E12F1B1C4B3587").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(200)
                .HasColumnName("name");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Users__3213E83F890ABB88");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Email)
                .HasMaxLength(320)
                .HasColumnName("email");
            entity.Property(e => e.FullName)
                .HasMaxLength(300)
                .HasColumnName("full_name");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}