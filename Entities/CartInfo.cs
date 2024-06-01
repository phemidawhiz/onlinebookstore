namespace WebApi.Entities
{
    public class CartInfo
    {
        public int Id { get; set; }
        public int CartId { get; set; }
        public int BookId { get; set; }
        public int Quantity { get; set; }
        public List<Book> Books { get; set; }
        public int UserId { get; set; }
        public OrderStatus OrderStatus { get; set; }
        public PaymentOption PaymentOption { get; set; }
    }
}
