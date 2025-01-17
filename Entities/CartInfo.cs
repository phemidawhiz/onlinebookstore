﻿namespace WebApi.Entities
{
    public class CartInfo
    {
        public int Id { get; set; }
        public List<CartItemBook> CartItemBooks { get; set; }
        public int UserId { get; set; }
        public OrderStatus OrderStatus { get; set; }
        public PaymentOption PaymentOption { get; set; }
    }
}
