namespace WebApi.Entities
{
    public class UserPurchaseHistory
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public OrderStatus OrderStatus { get; set; }
        public PaymentOption PaymentOption { get; set; }

        public List<CartItemBook> CartItemBook { get; set; }
    }
}
