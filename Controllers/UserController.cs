using calorieCounter_backend.Helpers;
using calorieCounter_backend.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace calorieCounter_backend.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController(IUserRepository userRepository) : ControllerBase
{
    [HttpPost("SignIn")]
    public async Task<ActionResult> SignIn([FromBody] string accessToken)
    {
        if (string.IsNullOrEmpty(accessToken))
            return Unauthorized("Access token is required.");

        try
        {
            var user = await AuthHelper.GetUserFromGoogleToken(accessToken);

            if (userRepository.UserAlreadyExist(user.Email)) return Ok();

            userRepository.AddEntity(user);

            return userRepository.SaveChanges() ? Ok() : Unauthorized("Failed to add user to database.");
        }
        catch (Exception e)
        {
            return Unauthorized(e.Message);
        }
    }

    [HttpPost("UpdateUser")]
    public async Task<ActionResult> UpdateUser([FromBody] string accessToken)
    {
        if (string.IsNullOrEmpty(accessToken))
            return Unauthorized("Access token is required.");

        try
        {
            var updatedUser = await AuthHelper.GetUserFromGoogleToken(accessToken);

            var existingUser = userRepository.GetUserByEmail(updatedUser.Email);

            if (existingUser == null)
                return NotFound("User not found.");

            existingUser.Name = updatedUser.Name;

            userRepository.UpdateEntity(existingUser);

            return userRepository.SaveChanges() ? Ok("User updated successfully.") : BadRequest("Failed to update user.");
        }
        catch (Exception e)
        {
            return Unauthorized(e.Message);
        }
    }

    [HttpDelete("DeleteUser")]
    public async Task<ActionResult> DeleteUser([FromBody] string accessToken)
    {
        if (string.IsNullOrEmpty(accessToken))
            return Unauthorized("Access token is required.");

        try
        {
            var userToDelete = await AuthHelper.GetUserFromGoogleToken(accessToken);

            var existingUser = userRepository.GetUserByEmail(userToDelete.Email);

            if (existingUser == null)
                return NotFound("User not found.");

            userRepository.DeleteEntity(existingUser);

            return userRepository.SaveChanges() ? Ok("User deleted successfully.") : BadRequest("Failed to delete user.");
        }
        catch (Exception e)
        {
            return Unauthorized(e.Message);
        }
    }

}