namespace BookStore.Models
{
    public class Book
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int AuthorId { get; set; }
        public Author? Author { get; set; }
        public string Description { get; set; }
        public double Rate { get; set; }
        public decimal Price { get; set; }
        public int CategoryId { get; set; }
        public Category? Category { get; set; }
        public string Img { get; set; }
        public decimal? Discount { get; set; }
        public int Year { get; set; }
        public int Quantity { get; set; }
        public ICollection<Promotion>? Promotions { get; set; } 
    }
}
