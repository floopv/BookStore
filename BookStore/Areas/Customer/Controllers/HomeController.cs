using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BookStore.Areas.Customer.Controllers
{
    [Route("[Area]/[controller]")]
    [ApiController]
    [Area("Customer")]
    public class HomeController : ControllerBase
    {
        private readonly IRepository<Book> _bookRepository;

        public HomeController(IRepository<Book> bookRepository)
        {
            _bookRepository = bookRepository;
        }
        [HttpGet("GetRecommendedBooks")]
        public async Task<IActionResult> GetRecommendedBooks()
        {
            var books = await _bookRepository.GetAllAsync(b => b.Rate >= 3.5, includes: [b => b.Category, b => b.Author]);
            if (books == null || !books.Any())
            {
                return NotFound(new ReturnModelResponse
                {
                    ReturnCode = 404,
                    ReturnMessage = "No recommended books found."
                });
            }
            var recommendedBooksDTO = books.Select(b => new BookResponse
            {
                Id = b.Id,
                Title = b.Title,
                Description = b.Description,
                Rate = b.Rate,
                Img = b.Img,
                Discount = b.Discount,
                Year = b.Year,
                Quantity = b.Quantity,
                AuthorName = b.Author.Name,
                CategoryName = b.Category.Name,
                Price = b.Price,
            }).ToList().Take(4);
            return Ok(recommendedBooksDTO);
        }
        [HttpGet("GetSaleBooks")]
        public async Task<IActionResult> GetSaleBooks()
        {
            var books = await _bookRepository.GetAllAsync(b=>b.Discount > 0, includes:[b=>b.Promotions, b=>b.Author, b=>b.Category]);
            if (books is null)
            {
                return NotFound(new ReturnModelResponse
                {
                    ReturnCode = 404,
                    ReturnMessage = "No Books Found"
                }
                );
            }
            var flashSaleBooks = books
                  .Where(b => b.Promotions != null && b.Promotions.Any(p =>
                      p.ExpiryDate > DateTime.Now && p.ExpiryDate <= DateTime.Now.AddMinutes(30)
                  ))
                  .Take(4)
                  .Select(b => new BookResponse
                  {
                      Id = b.Id,
                      Title = b.Title,
                      Description = b.Description,
                      Rate = b.Rate,
                      Img = b.Img,
                      Discount = b.Discount,
                      Year = b.Year,
                      Quantity = b.Quantity,
                      AuthorName = b.Author.Name,
                      CategoryName = b.Category.Name,
                      Price = b.Price,
                      Promotions = b.Promotions
                          .Where(p => p.ExpiryDate > DateTime.UtcNow && p.ExpiryDate <= DateTime.UtcNow.AddMinutes(30))
                          .Select(p => p.Code)
                          .ToList()
                  })
                  .ToList();

            if (!flashSaleBooks.Any())
            {
                return NotFound(new ReturnModelResponse
                {
                    ReturnCode = 404,
                    ReturnMessage = "No flash sale books found now"
                });
            }

            return Ok(flashSaleBooks);
        }
    }
}
