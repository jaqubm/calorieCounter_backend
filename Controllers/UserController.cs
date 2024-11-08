using AutoMapper;
using calorieCounter_backend.Dtos;
using calorieCounter_backend.Helpers;
using calorieCounter_backend.Models;
using calorieCounter_backend.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace calorieCounter_backend.Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
public class UserController(IUserRepository userRepository) : ControllerBase
{
    private readonly Mapper _mapper = new(new MapperConfiguration(c =>
    {
        c.CreateMap<User, UserDto>();
        c.CreateMap<UserDto, User>();
    })); 
    
    [HttpGet("Get")]
    public async Task<ActionResult<UserDto>> GetUser()
    {
        var userId = await AuthHelper.GetUserIdFromGoogleJwtTokenAsync(HttpContext);
        var userDb = await userRepository.GetUserByIdAsync(userId);
        
        if (userDb is null)
            return NotFound("User not found.");
        
        var user = _mapper.Map<UserDto>(userDb);
        
        return Ok(user);
    }
    
    [HttpPost("UpdateUser")]
    public async Task<ActionResult> UpdateUser([FromBody] UserDto userDto)
    {
        var userId = await AuthHelper.GetUserIdFromGoogleJwtTokenAsync(HttpContext);
        var userDb = await userRepository.GetUserByIdAsync(userId);
        
        if (userDb is null) return Unauthorized();

        _mapper.Map(userDto, userDb);

        userRepository.UpdateEntity(userDb);

        return await userRepository.SaveChangesAsync() ? Ok() : BadRequest("Failed to update user.");
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