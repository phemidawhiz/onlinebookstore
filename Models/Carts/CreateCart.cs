namespace WebApi.Models.Carts;

using System.ComponentModel.DataAnnotations;
using WebApi.Entities;

public class CreateCart
{
    
    [Required]
    public int? UserId { get; set; }
    [Required]
    [EnumDataType(typeof(OrderStatus))]
    public string? OrderStatus { get; set; }
    [Required]
    [EnumDataType(typeof(PaymentOption))]
    public string? PaymentOption { get; set; }

}