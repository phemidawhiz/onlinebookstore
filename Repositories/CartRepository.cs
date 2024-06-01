namespace WebApi.Repositories;

using Dapper;
using System.Collections.Specialized;
using WebApi.Entities;
using WebApi.Helpers;

public interface ICartRepository
{
    Task<IEnumerable<Cart>> GetAll();
    Task<Cart> GetById(int id);
    Task<IEnumerable<CartInfo>> GetCartItems(int id);
    Task Create(Cart cart);
    Task Update(Cart cart);
    Task Delete(int id);
}

public class CartRepository : ICartRepository
{
    private DataContext _context;

    public CartRepository(DataContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Cart>> GetAll()
    {
        using var connection = _context.CreateConnection();
        var sql = """
            SELECT * FROM Cart
        """;
        return await connection.QueryAsync<Cart>(sql);
    }

    public async Task<Cart> GetById(int id)
    {
        using var connection = _context.CreateConnection();
        var sql = """
            SELECT * FROM Cart 
            WHERE Id = @id
        """;
        return await connection.QuerySingleOrDefaultAsync<Cart>(sql, new { id });
    }

    public async Task<IEnumerable<CartInfo>> GetCartItems(int id)
    {
        using var connection = _context.CreateConnection();
        var cartDictionary = new Dictionary<int, CartInfo>();
        var sql = """SELECT c.*, ci.*, b.* FROM Cart c INNER JOIN CartItems ci ON c.Id = ci.CartId INNER JOIN Books b ON ci.BookId = b.Id WHERE c.Id = @id""";
        return await connection.QueryAsync<CartInfo, Book, CartInfo>(sql,
            (cart, book) =>
            {
                CartInfo cartEntry;

                if (!cartDictionary.TryGetValue(cart.Id, out cartEntry))
                {
                    cartEntry = cart;
                    cartEntry.Books = new List<Book>();
                    cartDictionary.Add(cartEntry.Id, cartEntry);
                }

                cartEntry.Books.Add(book);
                return cartEntry;
            },
            new { id },
            splitOn: "BookId");
    }
    

    public async Task Create(Cart cart)
    {
        using var connection = _context.CreateConnection();
        var sql = """
            INSERT INTO Cart (UserId, OrderStatus, PaymentOption)
            VALUES (@UserId, @OrderStatus, @PaymentOption)
        """;

        await connection.ExecuteAsync(sql, cart);
    }

    public async Task Update(Cart cart)
    {
        using var connection = _context.CreateConnection();
        var sql = """
            UPDATE Cart 
            SET OrderStatus = @OrderStatus,
                PaymentOption = @PaymentOption
            WHERE Id = @Id
        """;
        await connection.ExecuteAsync(sql, cart);
    }

    public async Task Delete(int id)
    {
        using var connection = _context.CreateConnection();
        var sql = """
            DELETE FROM Cart 
            WHERE Id = @id
        """;
        await connection.ExecuteAsync(sql, new { id });
    }
}