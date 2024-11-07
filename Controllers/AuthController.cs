using calorieCounter_backend.Helpers;
using calorieCounter_backend.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace calorieCounter_backend.Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
public class AuthController(IAuthRepository authRepository) : ControllerBase
{
    [HttpPost("SignIn")]
    public async Task<IActionResult> SignIn()
    {
        var userId = await AuthHelper.GetUserIdFromGoogleJwtTokenAsync(HttpContext);
        var userWithGivenIdExists = await authRepository.CheckIfUserExistsByIdAsync(userId);
        if (userWithGivenIdExists) return Ok();
        
        var user = await AuthHelper.CreateNewUserFromGoogleJwtTokenAsync(HttpContext);
        await authRepository.AddEntityAsync(user);
        
        return await authRepository.SaveChangesAsync() ? Ok() : Problem("Failed to add user to database.");
    }
}