namespace WebApi.Repositories;

using Dapper;
using System.Collections.Specialized;
using WebApi.Entities;
using WebApi.Helpers;

public interface ICartRepository
{
    Task<IEnumerable<Cart>> GetAll();
    Task<Cart> GetById(int id);
    Task<IEnumerable<CartItemBook>> GetCartItems(int id);
    Task<IEnumerable<CartInfo>> GetUserCartHistory(int id); 
    Task<IEnumerable<CartInfo>> GetUserPurchaseHistory(int id);
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

    public async Task<IEnumerable<CartItemBook>> GetCartItems(int id)
    {
        using var connection = _context.CreateConnection();
        var cartDictionary = new Dictionary<int, CartItemBook>();
        var sql = """SELECT ci.*, b.* FROM CartItems ci INNER JOIN Books b ON ci.BookId = b.Id WHERE ci.CartId = @id""";
        return await connection.QueryAsync<CartItemBook>(sql, new { id });
    }

    public async Task<IEnumerable<CartInfo>> GetUserCartHistory(int id)
    {
        using var connection = _context.CreateConnection();
        var cartDictionary = new Dictionary<int, CartInfo>();
        var sql = """SELECT c.*, ci.*, b.* FROM Cart c INNER JOIN CartItems ci ON c.Id = ci.CartId INNER JOIN Books b ON ci.BookId = b.Id WHERE c.UserId = @id""";
        
        //return await connection.QueryAsync<CartInfo>(sql, new { id });
        return await connection.QueryAsync<CartInfo, CartItemBook, CartInfo>(sql,
            (cart, cartItem) =>
            {
                CartInfo cartEntry;

                if (!cartDictionary.TryGetValue(cart.Id, out cartEntry))
                {
                    cartEntry = cart;
                    cartEntry.CartItemBooks = new List<CartItemBook>();
                    cartDictionary.Add(cartEntry.Id, cartEntry);
                }

                cartEntry.CartItemBooks.Add(cartItem);
                return cartEntry;
            },
            new { id },
            splitOn: "BookId");
    }

    public async Task<IEnumerable<CartInfo>> GetUserPurchaseHistory(int id)
    {
        using var connection = _context.CreateConnection();
        var cartDictionary = new Dictionary<int, CartInfo>();
        var sql = """SELECT c.*, ci.*, b.* FROM Cart c INNER JOIN CartItems ci ON c.Id = ci.CartId INNER JOIN Books b ON ci.BookId = b.Id WHERE c.UserId = @id AND c.OrderStatus = @OrderStatus""";

        //return await connection.QueryAsync<CartInfo>(sql, new { id });
        return await connection.QueryAsync<CartInfo, CartItemBook, CartInfo>(sql,
            (cart, cartItem) =>
            {
                CartInfo cartEntry;

                if (!cartDictionary.TryGetValue(cart.Id, out cartEntry))
                {
                    cartEntry = cart;
                    cartEntry.CartItemBooks = new List<CartItemBook>();
                    cartDictionary.Add(cartEntry.Id, cartEntry);
                }

                cartEntry.CartItemBooks.Add(cartItem);
                return cartEntry;
            },
            new { id, OrderStatus = OrderStatus.PaidFor },
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