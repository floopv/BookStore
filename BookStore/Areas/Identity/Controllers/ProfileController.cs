using BookStore.DTOs.Request;
using BookStore.DTOs.Response;
using BookStore.Models;
using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace BookStore.Areas.Identity.Controllers
{
    [Route("[Area]/[controller]")]
    [ApiController]
    [Area("Identity")]
    public class ProfileController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public ProfileController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }
        [HttpGet]
        public async Task<IActionResult> GetInfo()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound(new ReturnModelResponse
                {
                    ReturnCode = 404 ,
                    ReturnMessage = "User not found."
                });
            }
            //var userVM = new ApplicationUserVM
            //{
            //    FullName = $"{user.FirstName} {user.LastName}",
            //    Address = user.Address,
            //    PhoneNumber = user.PhoneNumber,
            //    Email = user.Email
            //};
            //TypeAdapterConfig<ApplicationUser, ApplicationUserVM>.NewConfig()
            //    .Map(dest => dest.FullName, src => $"{src.FirstName} {src.LastName}");
            var userVM = user.Adapt<ApplicationUserResponse>();
            return Ok(userVM);
        }
        [HttpPut("UpdateProfile")]
        public async Task<IActionResult> UpdateProfile(ApplicationUserRequest applicationUserRequest)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound (new ReturnModelResponse
                {
                    ReturnCode = 404 ,
                    ReturnMessage = "User not found."
                });
            }
            user.FirstName = applicationUserRequest.FirstName;
            user.LastName = applicationUserRequest.LastName;
            user.Address = applicationUserRequest.Address;
            user.PhoneNumber = applicationUserRequest.PhoneNumber;
            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                string errors = string.Empty;
                foreach(var error in result.Errors)
                {
                    errors += $"{error.Description} \n";
                }
                return BadRequest (new ReturnModelResponse
                {
                    ReturnCode = 400 ,
                    ReturnMessage = errors
                });
            }
            else
            {
                return Ok(new ReturnModelResponse
                {
                    ReturnCode = 200 ,
                    ReturnMessage = "Profile Updated Successfully"
                });
            }
        }
    }
}
