namespace BookStore.DTOs.Response
{
    public class BookResponse
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public double Rate { get; set; }
        public decimal Price { get; set; }
        public string Img { get; set; }
        public decimal? Discount { get; set; }
        public int Year { get; set; }
        public int Quantity { get; set; }
        public string AuthorName { get; set; }
        public string CategoryName { get; set; }
        public List<string> Promotions { get; set; }
    }
}
