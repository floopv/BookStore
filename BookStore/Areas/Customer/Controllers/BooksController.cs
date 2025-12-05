using BookStore.Models;
using BookStore.Repos;
using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BookStore.Areas.Customer.Controllers
{
    [Route("[Area]/[controller]")]
    [ApiController]
    [Area("Customer")]
    public class BooksController : ControllerBase
    {
        private readonly IRepository<Book> _bookRepository;

        public BooksController(IRepository<Book> bookRepository)
        {
            _bookRepository = bookRepository;
        }

        [HttpGet("GetAllBooks")]
        public async Task<IActionResult> GetAllBooks()
        {
            var books = await _bookRepository.GetAllAsync(includes: [b=>b.Promotions , b=>b.Author , b=>b.Category]);
            if (books == null)
            {
                return NotFound(new ReturnModelResponse
                {
                    ReturnCode = 404,
                    ReturnMessage = "No books found."
                });
            }
            var booksDTO = books.Select(b=> new BookResponse
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
                Promotions = b.Promotions.Select(p => p.Code).ToList()
            });

            return Ok(booksDTO);
        }
        [HttpGet("GetBookByCategory")]
        public async Task<IActionResult> GetBookByCategory(int categoryId)
        {
            //var books = await _bookRepository.GetAllAsync(b => b.CategoryId == category);
            var books = await _bookRepository.GetAllAsync(b => b.CategoryId == categoryId , includes: [b => b.Promotions, b => b.Author, b => b.Category]);
            if (books == null || books.Count() == 0)
            {
                return NotFound(new ReturnModelResponse
                {
                    ReturnCode = 404,
                    ReturnMessage = "No books found."
                });
            }
            var booksDTO = books.Select(b=>new BookResponse
            {
                Id = b.Id,
                Title = b.Title,
                Description = b.Description,
                Discount = b.Discount,
                Year = b.Year,
                Quantity= b.Quantity,
                AuthorName = b.Author.Name,
                CategoryName = b.Category.Name,
                Price = b.Price,
                Img = b.Img,
                Promotions = b.Promotions.Select(p=>p.Code).ToList()
            });
            return Ok(booksDTO);
        }
    }
}
