using System.ComponentModel.DataAnnotations;

namespace BookStore.DTOs.Response
{
    public class ApplicationUserResponse
    {
        public string Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Address { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
    }
}
