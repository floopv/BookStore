using static BookStore.Utilities.Enum;
namespace BookStore.Models
{
    public class Order
    {
        public int Id { get; set; }
        public Status Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Address { get; set; }
        public string ApplicationUserId { get; set; }
        public ApplicationUser? ApplicationUser { get; set; }
        public ICollection<Book>? Books { get; set; }
    }
}
