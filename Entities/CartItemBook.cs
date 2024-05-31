namespace WebApi.Entities
{
    public class CartItemBook
    {
        public int Id { get; set; }
        public int CartId { get; set; }
        public int BookId { get; set; }
        public int Quantity { get; set; }
        public string? Title { get; set; }
        public string? Isbn { get; set; }
        public string? Genre { get; set; }
        public string? Author { get; set; }
        public string? Year { get; set; }
        public int Price { get; set; }

    }
}
