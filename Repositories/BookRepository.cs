namespace WebApi.Repositories;

using Dapper;
using WebApi.Entities;
using WebApi.Helpers;

public interface IBookRepository
{
    Task<IEnumerable<Book>> GetAll();
    Task<Book> GetById(int id);
    Task<IEnumerable<Book>> Search(string searchTerm);
    Task Create(Book book);
    Task Update(Book book);
    Task Delete(int id);
}

public class BookRepository : IBookRepository
{
    private DataContext _context;

    public BookRepository(DataContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Book>> GetAll()
    {
        using var connection = _context.CreateConnection();
        var sql = """
            SELECT * FROM Books
        """;
        return await connection.QueryAsync<Book>(sql);
    }

    public async Task<IEnumerable<Book>> Search(string searchterm)
    {
        using var connection = _context.CreateConnection();
        var sql = """SELECT * FROM Books WHERE Title LIKE CONCAT('%',@searchterm,'%') OR Genre LIKE CONCAT('%',@searchterm,'%') OR Year LIKE CONCAT('%',@searchterm,'%') OR Author LIKE CONCAT('%',@searchterm,'%')""";
        Console.WriteLine(new { searchterm });
        return await connection.QueryAsync<Book>(sql, new { searchterm });
    }

    public async Task<Book> GetById(int id)
    {
        using var connection = _context.CreateConnection();
        var sql = """
            SELECT * FROM Books 
            WHERE Id = @id
        """;
        return await connection.QuerySingleOrDefaultAsync<Book>(sql, new { id });
    }

    public async Task Create(Book book)
    {
        using var connection = _context.CreateConnection();
        var sql = """
            INSERT INTO Books (Title, Isbn, Genre, Author, Year, Price)
            VALUES (@Title, @Isbn, @Genre, @Author, @Year, @Price)
        """;

        await connection.ExecuteAsync(sql, book);
    }

    public async Task Update(Book book)
    {
        using var connection = _context.CreateConnection();
        var sql = """
            UPDATE Books 
            SET Title = @Title,
                Author = @Author,
                Genre = @Genre, 
                Year = @Year, 
                Price = @Price
            WHERE Id = @Id
        """;
        await connection.ExecuteAsync(sql, book);
    }

    public async Task Delete(int id)
    {
        using var connection = _context.CreateConnection();
        var sql = """
            DELETE FROM Books 
            WHERE Id = @id
        """;
        await connection.ExecuteAsync(sql, new { id });
    }
}