namespace WebApi.Models.CartItems;

using System.ComponentModel.DataAnnotations;
using WebApi.Entities;

public class UpdateCartItem
{

    [Required]
    public int? CartId { get; set; }
    [Required]
    public int? BookId { get; set; }
    [Required]
    public int? Quantity { get; set; }
}