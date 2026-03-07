using Microsoft.EntityFrameworkCore;
using PrograAvanzada.Viernes.MyLibraryDbModel.DbModels;
using PrograAvanzada.Viernes.MyLibraryRepository.CRUD;
using PrograAvanzada.Viernes.MyLibraryRepository.Repositories;
using PrograAvanzada.Viernes.MyLibraryJsonRepository.CRUD;
using PrograAvanzada.Viernes.MyLibraryJsonRepository.Repositories;
using PrograAvanzada.Viernes.MyLibrayApi.Interfaces;
using PrograAvanzada.Viernes.MyLibrayApi.Adapters;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

var repositoryType = builder.Configuration.GetValue<string>("RepositoryType") ?? "json";

if (repositoryType == "db")
{
    builder.Configuration.AddUserSecrets<Program>();
    var connectionString = builder.Configuration.GetConnectionString("MySqlServerConnectionString");
    builder.Services.AddDbContext<ViernesContext>(options =>
        options.UseSqlServer(connectionString));

    // Register concrete repositories
    builder.Services.AddScoped<PrograAvanzada.Viernes.MyLibraryRepository.CRUD.AuthorCrudRepository>();
    builder.Services.AddScoped<PrograAvanzada.Viernes.MyLibraryRepository.CRUD.BookCrudRepository>();
    builder.Services.AddScoped<PrograAvanzada.Viernes.MyLibraryRepository.CRUD.BookAuthorCrudRepository>();
    builder.Services.AddScoped<PrograAvanzada.Viernes.MyLibraryRepository.CRUD.BookCopyCrudRepository>();
    builder.Services.AddScoped<PrograAvanzada.Viernes.MyLibraryRepository.CRUD.BookThemeCrudRepository>();
    builder.Services.AddScoped<PrograAvanzada.Viernes.MyLibraryRepository.CRUD.BorrowCrudRepository>();
    builder.Services.AddScoped<PrograAvanzada.Viernes.MyLibraryRepository.CRUD.ThemeCrudRepository>();
    builder.Services.AddScoped<PrograAvanzada.Viernes.MyLibraryRepository.CRUD.UserCrudRepository>();
    builder.Services.AddScoped<PrograAvanzada.Viernes.MyLibraryRepository.Repositories.BookRepository>();
    builder.Services.AddScoped<PrograAvanzada.Viernes.MyLibraryRepository.Repositories.UserRepository>();

    // Register adapters
    builder.Services.AddScoped<IAuthorCrudRepository, AuthorCrudRepositoryAdapter>();
    builder.Services.AddScoped<IBookCrudRepository, BookCrudRepositoryAdapter>();
    builder.Services.AddScoped<IBookAuthorCrudRepository, BookAuthorCrudRepositoryAdapter>();
    builder.Services.AddScoped<IBookCopyCrudRepository, BookCopyCrudRepositoryAdapter>();
    builder.Services.AddScoped<IBookThemeCrudRepository, BookThemeCrudRepositoryAdapter>();
    builder.Services.AddScoped<IBorrowCrudRepository, BorrowCrudRepositoryAdapter>();
    builder.Services.AddScoped<IThemeCrudRepository, ThemeCrudRepositoryAdapter>();
    builder.Services.AddScoped<IUserCrudRepository, UserCrudRepositoryAdapter>();
    builder.Services.AddScoped<IBookRepository, BookRepositoryAdapter>();
    builder.Services.AddScoped<IUserRepository, UserRepositoryAdapter>();
}
else
{
    // Register concrete repositories
    builder.Services.AddScoped<PrograAvanzada.Viernes.MyLibraryJsonRepository.CRUD.AuthorCrudRepository>();
    builder.Services.AddScoped<PrograAvanzada.Viernes.MyLibraryJsonRepository.CRUD.BookCrudRepository>();
    builder.Services.AddScoped<PrograAvanzada.Viernes.MyLibraryJsonRepository.CRUD.BookAuthorCrudRepository>();
    builder.Services.AddScoped<PrograAvanzada.Viernes.MyLibraryJsonRepository.CRUD.BookCopyCrudRepository>();
    builder.Services.AddScoped<PrograAvanzada.Viernes.MyLibraryJsonRepository.CRUD.BookThemeCrudRepository>();
    builder.Services.AddScoped<PrograAvanzada.Viernes.MyLibraryJsonRepository.CRUD.BorrowCrudRepository>();
    builder.Services.AddScoped<PrograAvanzada.Viernes.MyLibraryJsonRepository.CRUD.ThemeCrudRepository>();
    builder.Services.AddScoped<PrograAvanzada.Viernes.MyLibraryJsonRepository.CRUD.UserCrudRepository>();
    builder.Services.AddScoped<PrograAvanzada.Viernes.MyLibraryJsonRepository.Repositories.BookRepository>();
    builder.Services.AddScoped<PrograAvanzada.Viernes.MyLibraryJsonRepository.Repositories.UserRepository>();

    // Register adapters
    builder.Services.AddScoped<IAuthorCrudRepository, AuthorJsonCrudRepositoryAdapter>();
    builder.Services.AddScoped<IBookCrudRepository, BookJsonCrudRepositoryAdapter>();
    builder.Services.AddScoped<IBookAuthorCrudRepository, BookAuthorJsonCrudRepositoryAdapter>();
    builder.Services.AddScoped<IBookCopyCrudRepository, BookCopyJsonCrudRepositoryAdapter>();
    builder.Services.AddScoped<IBookThemeCrudRepository, BookThemeJsonCrudRepositoryAdapter>();
    builder.Services.AddScoped<IBorrowCrudRepository, BorrowJsonCrudRepositoryAdapter>();
    builder.Services.AddScoped<IThemeCrudRepository, ThemeJsonCrudRepositoryAdapter>();
    builder.Services.AddScoped<IUserCrudRepository, UserJsonCrudRepositoryAdapter>();
    builder.Services.AddScoped<IBookRepository, BookJsonRepositoryAdapter>();
    builder.Services.AddScoped<IUserRepository, UserJsonRepositoryAdapter>();
}

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
