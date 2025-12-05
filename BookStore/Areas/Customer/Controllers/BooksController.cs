using BookStore.Models;
using BookStore.Repos;
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
            var books = await _bookRepository.GetAllAsync(includes: [b=>b.Promotions]);
            if (books == null)
            {
                return NotFound(new ReturnModelResponse
                {
                    ReturnCode = 404,
                    ReturnMessage = "No books found."
                });
            }
            return Ok(books);
        }
        [HttpGet("GetBookByCategory")]
        public async Task<IActionResult> GetBookByCategory(int categoryId)
        {
            //var books = await _bookRepository.GetAllAsync(b => b.CategoryId == category);
            var books = await _bookRepository.GetAllAsync(b => b.CategoryId == categoryId);
            if (books == null || books.Count() == 0)
            {
                return NotFound(new ReturnModelResponse
                {
                    ReturnCode = 404,
                    ReturnMessage = "No books found."
                });
            }
            return Ok(books);
        }
    }
}
