using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Stripe.Checkout;

namespace BookStore.Areas.Customer.Controllers
{
    [Route("[Area]/[controller]")]
    [ApiController]
    [Area("Customer")]
    public class CartController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IRepository<Cart> _cartRepository;
        private readonly IRepository<Promotion> _promotionRepository;
        private readonly IRepository<ApplicationUserPromotionCode> _applicationUserPromotionCodeRepository;
        private readonly IRepository<Book> _bookRepository;


        public CartController(UserManager<ApplicationUser> userManager, IRepository<Promotion> promotionRepository, IRepository<ApplicationUserPromotionCode> applicationUserPromotionCodeRepository, IRepository<Book> bookRepository, IRepository<Cart> cartRepository)
        {
            _userManager = userManager;
            _promotionRepository = promotionRepository;
            _applicationUserPromotionCodeRepository = applicationUserPromotionCodeRepository;
            _bookRepository = bookRepository;
            _cartRepository = cartRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(string? code = null)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound(new ReturnModelResponse
                {
                    ReturnCode = 404,
                    ReturnMessage = "User not found."
                });
            }

            var cartItems = await _cartRepository.GetAllAsync(c => c.ApplicationUserId == user.Id, [b => b.Book]);
            if (cartItems == null)
            {
                return BadRequest(new ReturnModelResponse
                {
                    ReturnCode = 400,
                    ReturnMessage = "Your cart is empty."
                });
            }
            if (code != null)
            {
                var promotion = await _promotionRepository.GetOneAsync(p => p.Code == code);
                if (promotion != null)
                {
                    if (promotion.IsValid && promotion.ExpiryDate > DateTime.UtcNow && promotion.MaxUsage > 0)
                    {
                        var bookInCart = cartItems.FirstOrDefault(c => c.BookId == promotion.BookId);
                        if (bookInCart != null)
                        {
                            var alreadyUsed = await _applicationUserPromotionCodeRepository.GetOneAsync(apc => apc.ApplicationUserId == user.Id && apc.PromotionCodeId == promotion.Id);
                            if (alreadyUsed != null)
                            {
                                return BadRequest(new ReturnModelResponse
                                {
                                    ReturnCode = 400,
                                    ReturnMessage = "You have already used this promotion code."
                                });
                            }
                            bookInCart.Price -= bookInCart.Price * (promotion.Discount / 100);
                            promotion.MaxUsage--;
                            if (promotion.MaxUsage == 0)
                            {
                                promotion.IsValid = false;
                            }
                            _promotionRepository.Update(promotion);
                            await _applicationUserPromotionCodeRepository.AddAsync(new ApplicationUserPromotionCode()
                            {
                                ApplicationUserId = user.Id,
                                PromotionCodeId = promotion.Id
                            });
                            await _promotionRepository.CommitAsync();
                            return Ok(new ReturnModelResponse
                            {
                                ReturnCode = 200,
                                ReturnMessage = "Promotion code applied successfully."
                            });
                        }
                        else
                        {
                            return BadRequest(new ReturnModelResponse
                            {
                                ReturnCode = 400,
                                ReturnMessage = "This promotion code is not applicable to any book in your cart."
                            });
                        }
                    }
                    else
                    {
                        return BadRequest(new ReturnModelResponse
                        {
                            ReturnCode = 400,
                            ReturnMessage = "This promotion code is no longer valid."
                        });
                    }
                }
                else
                {
                    return BadRequest(new ReturnModelResponse
                    {
                        ReturnCode = 400,
                        ReturnMessage = "Invalid promotion code."
                    });
                }
            }
            var totalAmount = cartItems.Sum(c => c.Price * c.Count);
            return Ok(cartItems);
        }
        [HttpPost("AddToCart")]
        public async Task<IActionResult> AddToCart(int bookId, int Count)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound(new ReturnModelResponse
                {
                    ReturnCode = 404,
                    ReturnMessage = "User not found."
                });
            }
            var book = await _bookRepository.GetOneAsync(b => b.Id == bookId);
            if (book == null)
            {
                return NotFound(new ReturnModelResponse
                {
                    ReturnCode = 404,
                    ReturnMessage = "Book Not Found"
                });
            }
            if (Count <= 0)
            {
                return BadRequest(new ReturnModelResponse
                {
                    ReturnCode = 400,
                    ReturnMessage = "Count cant be zero or less"
                });
            }
            var existingCartItem = await _cartRepository.GetOneAsync(c => c.ApplicationUserId == user.Id && c.BookId == bookId);
            if (existingCartItem != null)
            {
                existingCartItem.Count += Count;
                _cartRepository.Update(existingCartItem);
            }
            else
            {
                var cart = new Cart()
                {
                    ApplicationUserId = user.Id,
                    BookId = bookId,
                    Count = Count,
                    Price = book.Price
                };
                await _cartRepository.AddAsync(cart);
            }
            await _cartRepository.CommitAsync();
            return Ok(new ReturnModelResponse
            {
                ReturnCode = 200,
                ReturnMessage = "Book added to cart successfully."
            });
        }
        [HttpPost("IncrementBook/{bookId}")]
        public async Task<IActionResult> IncrementBook(int bookId)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound(new ReturnModelResponse
                {
                    ReturnCode = 404,
                    ReturnMessage = "User not found."
                });
            }
            var cartItems = await _cartRepository.GetOneAsync(c => c.ApplicationUserId == user.Id && c.BookId == bookId);
            if (cartItems != null)
            {
                cartItems.Count += 1;
                _cartRepository.Update(cartItems);
                await _cartRepository.CommitAsync();
            }
            return Ok(new ReturnModelResponse
            {
                ReturnCode = 200,
                ReturnMessage = "Book quantity incremented successfully."
            });
        }
        [HttpPost("DecrementBook/{bookId}")]
        public async Task<IActionResult> DecrementBook(int bookId)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound(new ReturnModelResponse
                {
                    ReturnCode = 404,
                    ReturnMessage = "User not found."
                });
            }
            var cartItems = await _cartRepository.GetOneAsync(c => c.ApplicationUserId == user.Id && c.BookId == bookId);
            if (cartItems != null)
            {
                if (cartItems.Count <= 1)
                {
                    _cartRepository.Delete(cartItems);
                    await _cartRepository.CommitAsync();
                    return Ok(new ReturnModelResponse
                    {
                        ReturnCode = 200,
                        ReturnMessage = "Book removed from cart successfully."
                    });
                }
                cartItems.Count--;
                _cartRepository.Update(cartItems);
                await _cartRepository.CommitAsync();
            }
            return Ok(new ReturnModelResponse
            {
                ReturnCode = 200,
                ReturnMessage = "Book quantity decremented successfully."
            });
        }
        [HttpDelete("{bookId}")]
        public async Task<IActionResult> DeleteBook(int bookId)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound(new ReturnModelResponse
                {
                    ReturnCode = 404,
                    ReturnMessage = "User not found."
                });
            }
            var cartItems = await _cartRepository.GetOneAsync(c => c.ApplicationUserId == user.Id && c.BookId == bookId);
            if (cartItems != null)
            {
                _cartRepository.Delete(cartItems);
                await _cartRepository.CommitAsync();
            }
            return Ok(new ReturnModelResponse
            {
                ReturnCode = 200,
                ReturnMessage = "Book removed from cart successfully."
            });
        }
        [HttpPost("Payment")]
        public async Task<IActionResult> Payment()
        {
            var options = new SessionCreateOptions
            {
                PaymentMethodTypes = new List<string> { "card" },
                LineItems = new List<SessionLineItemOptions>(),
                Mode = "payment",
                SuccessUrl = $"{Request.Scheme}://{Request.Host}/Customer/Checkout/success",
                CancelUrl = $"{Request.Scheme}://{Request.Host}/Customer/Checkout/cancel",
            };
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound(new ReturnModelResponse
                {
                    ReturnCode = 404,
                    ReturnMessage = "User not found."
                });
            }
            var cartItems = await _cartRepository.GetAllAsync(c => c.ApplicationUserId == user.Id, [b => b.Book]);
            if (cartItems is null)
            {
                return BadRequest(new ReturnModelResponse
                {
                    ReturnCode = 400,
                    ReturnMessage = "Your cart is empty."
                });
            }
            foreach (var item in cartItems)
            {
                var sessionLineItems = new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        Currency = "EGP",
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = item.Book.Title,
                            Description = item.Book.Description,
                        },
                        UnitAmount = (long)item.Price * 100,
                    },
                    Quantity = item.Count,
                };
                options.LineItems.Add(sessionLineItems);
            }
            var service = new SessionService();
            var session = service.Create(options);
            return Ok(new ReturnModelResponse
            {
                ReturnCode = 200,
                ReturnMessage = "Payment session created successfully. Session ID: " + session.Id
            });
        }
    }
}
