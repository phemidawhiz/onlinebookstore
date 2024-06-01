namespace WebApi.Repositories;

using Dapper;
using WebApi.Entities;
using WebApi.Helpers;

public interface ICartItemRepository
{
    Task<IEnumerable<CartItem>> GetAll();
    Task<CartItem> GetById(int id);
    Task<IEnumerable<CartItem>> GetByCartId(int id); 
    Task<CartItemBook> GetByCartItemBook(int id);
    Task Create(CartItem cartItem);
    Task Update(CartItem cartItem);
    Task Delete(int id);
}

public class CartItemRepository : ICartItemRepository
{
    private DataContext _context;

    public CartItemRepository(DataContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<CartItem>> GetAll()
    {
        using var connection = _context.CreateConnection();
        var sql = """
            SELECT * FROM CartItems
        """;
        return await connection.QueryAsync<CartItem>(sql);
    }

    public async Task<CartItem> GetById(int id)
    {
        using var connection = _context.CreateConnection();
        var sql = """
            SELECT * FROM CartItems 
            WHERE Id = @id
        """;
        return await connection.QuerySingleOrDefaultAsync<CartItem>(sql, new { id });
    }

    public async Task<IEnumerable<CartItem>> GetByCartId(int id)
    {
        using var connection = _context.CreateConnection();
        var sql = """
            SELECT * FROM CartItems 
            WHERE CartId = @id
        """;
        return await connection.QueryAsync<CartItem>(sql, new { id });
    }

    public async Task<CartItemBook> GetByCartItemBook(int id)
    {
        using var connection = _context.CreateConnection();
        var sql = """
            SELECT c.Id, c.Quantity, c.CartId, c.BookId, b.Title, b.Author, b.Genre, b.Isbn, b.Year
            FROM CartItems c
            INNER JOIN Books b
            ON c.BookId = b.Id
            WHERE c.Id = @id
        """;
        return await connection.QuerySingleOrDefaultAsync<CartItemBook>(sql, new { id });
    }

    public async Task Create(CartItem cartItem)
    {
        using var connection = _context.CreateConnection();
        var sql = """
            INSERT INTO CartItems (CartId, BookId, Quantity)
            VALUES (@CartId, @BookId, @Quantity)
        """;

        await connection.ExecuteAsync(sql, cartItem);
    }

    public async Task Update(CartItem cartItem)
    {
        using var connection = _context.CreateConnection();
        var sql = """
            UPDATE CartItems 
            SET CartId = @CartId,
                BookId = @BookId,
                Quantity = @Quantity
            WHERE Id = @Id
        """;
        await connection.ExecuteAsync(sql, cartItem);
    }

    public async Task Delete(int id)
    {
        using var connection = _context.CreateConnection();
        var sql = """
            DELETE FROM CartItems 
            WHERE Id = @id
        """;
        await connection.ExecuteAsync(sql, new { id });
    }
}