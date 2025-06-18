using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
//using BookReviewApp.Models;
using BookReviewApp.DTOs;
using BookReviewApp.API.Models.Identity;

[Route("api/[controller]")]
[ApiController]
[Authorize] // Requiere autenticación
public class ProfileController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;

    public ProfileController(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    // GET: api/profile
    [HttpGet]
    public async Task<ActionResult<ProfileDto>> GetProfile()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return NotFound("User not found.");
        }

        var profile = new ProfileDto(user.UserName, user.Email, user.ProfilePictureUrl);
        return Ok(profile);
    }

    // PUT: api/profile
    [HttpPut]
    public async Task<IActionResult> UpdateProfile([FromBody] UpdateProfileDto updateProfileDto)
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return NotFound("User not found.");
        }

        user.Email = updateProfileDto.Email;
        user.ProfilePictureUrl = updateProfileDto.ProfilePictureUrl;

        var result = await _userManager.UpdateAsync(user);

        if (result.Succeeded)
        {
            return NoContent();
        }

        return BadRequest(result.Errors);
    }
}

//using Microsoft.AspNetCore.Mvc;

//namespace BookReviewApp.Api.Controllers
//{
//    public class ProfileController : Controller
//    {
//        public IActionResult Index()
//        {
//            return View();
//        }
//    }
//}
