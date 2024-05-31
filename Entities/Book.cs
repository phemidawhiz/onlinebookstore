namespace WebApi.Entities;

using System.Text.Json.Serialization;

public class Book
{
    public int Id { get; set; }
    public string? Title { get; set; }
    public string? Isbn { get; set; }
    public string? Genre { get; set; }
    public string? Author { get; set; }
    public string? Year { get; set; }
    public int Price { get; set; }
}