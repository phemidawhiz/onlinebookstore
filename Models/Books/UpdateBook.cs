namespace WebApi.Models.Books;

using System.ComponentModel.DataAnnotations;
using WebApi.Entities;

public class UpdateBook
{
    [Required]
    public string? Title { get; set; }

    [Required]
    public string? Isbn { get; set; }

    [Required]
    public string? Genre { get; set; }

    [Required]
    public string? Author { get; set; }

    [Required]
    public string? Year { get; set; }

    [Required]
    public int? Price { get; set; }
}