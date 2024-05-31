namespace WebApi.Helpers;

using System.Data;
using Dapper;
using Microsoft.Extensions.Options;
using Npgsql;

public class DataContext
{
    private DbSettings _dbSettings;

    public DataContext(IOptions<DbSettings> dbSettings)
    {
        _dbSettings = dbSettings.Value;
    }

    public IDbConnection CreateConnection()
    {
        var connectionString = $"Host={_dbSettings.Server}; Database={_dbSettings.Database}; Username={_dbSettings.UserId}; Password={_dbSettings.Password};";
        return new NpgsqlConnection(connectionString);
    }

    public async Task Init()
    {
        await _initDatabase();
        await _initTables();
    }

    private async Task _initDatabase()
    {
        // create database if it doesn't exist
        var connectionString = $"Host={_dbSettings.Server}; Database=postgres; Username={_dbSettings.UserId}; Password={_dbSettings.Password};";
        using var connection = new NpgsqlConnection(connectionString);
        var sqlDbCount = $"SELECT COUNT(*) FROM pg_database WHERE datname = '{_dbSettings.Database}';";
        var dbCount = await connection.ExecuteScalarAsync<int>(sqlDbCount);
        if (dbCount == 0)
        {
            var sql = $"CREATE DATABASE \"{_dbSettings.Database}\"";
            await connection.ExecuteAsync(sql);
        }
    }

    private async Task _initTables()
    {
        // create tables if they don't exist
        using var connection = CreateConnection();
        await _initUsers();
        await _initBooks();
        await _initCart();
        await _initCartItems();

        async Task _initUsers()
        {
            var sql = """
                CREATE TABLE IF NOT EXISTS Users (
                    Id SERIAL PRIMARY KEY,
                    Title VARCHAR,
                    FirstName VARCHAR,
                    LastName VARCHAR,
                    Email VARCHAR,
                    Role INTEGER,
                    PasswordHash VARCHAR,
                    CreatedAt TIMESTAMP NOT NULL,
                    UpdatedAt TIMESTAMP default current_timestamp,
                    CONSTRAINT Email UNIQUE(Email)
                );
            """;
            await connection.ExecuteAsync(sql);
        }

        async Task _initBooks()
        {
            var sql = """
                CREATE TABLE IF NOT EXISTS Books (
                    Id SERIAL PRIMARY KEY,
                    Title VARCHAR NOT NULL,
                    Isbn VARCHAR(50) NOT NULL,
                    Genre VARCHAR(100) NOT NULL,
                    Author VARCHAR(150) NOT NULL,
                    Year VARCHAR(4) NOT NULL,
                    Price INTEGER,
                    CreatedAt TIMESTAMP NOT NULL,
                    UpdatedAt TIMESTAMP default current_timestamp,
                    CONSTRAINT Isbn UNIQUE(Isbn)
                );
            """;
            await connection.ExecuteAsync(sql);
        }

        async Task _initCartItems()
        {
            var sql = """
                CREATE TABLE IF NOT EXISTS CartItems (
                    Id SERIAL PRIMARY KEY,
                    CartId INTEGER,
                    BookId INTEGER,
                    Quantity INTEGER,
                    CreatedAt TIMESTAMP NOT NULL,
                    UpdatedAt TIMESTAMP default current_timestamp,
                    CONSTRAINT FkBookId FOREIGN KEY(BookId) REFERENCES Books(Id),
                    CONSTRAINT FkCartId FOREIGN KEY(CartId) REFERENCES Cart(Id)
                );
            """;
            await connection.ExecuteAsync(sql);
        }

        async Task _initCart()
        {
            var sql = """
                CREATE TABLE IF NOT EXISTS Cart (
                    Id SERIAL PRIMARY KEY,
                    UserId INTEGER,
                    OrderStatus INTEGER,
                    PaymentOption INTEGER,
                    CreatedAt TIMESTAMP NOT NULL,
                    UpdatedAt TIMESTAMP default current_timestamp,
                    CONSTRAINT FkUserId FOREIGN KEY(UserId) REFERENCES Users(Id)
                    ON UPDATE NO ACTION
                    ON DELETE NO ACTION
                );
            """;
            await connection.ExecuteAsync(sql);
        }

        
    }
}