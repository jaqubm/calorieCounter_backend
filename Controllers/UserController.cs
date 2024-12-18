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
        c.CreateMap<UserUpdateDto, User>();
        c.CreateMap<UserUpdateGoalsDto, User>();
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
    
    [HttpPut("UpdateUser")]
    public async Task<ActionResult> UpdateUser([FromBody] UserUpdateDto userUpdateDto)
    {
        var userId = await AuthHelper.GetUserIdFromGoogleJwtTokenAsync(HttpContext);
        var userDb = await userRepository.GetUserByIdAsync(userId);
        
        if (userDb is null) return Unauthorized();

        _mapper.Map(userUpdateDto, userDb);

        userRepository.UpdateEntity(userDb);

        return await userRepository.SaveChangesAsync() ? Ok() : BadRequest("Failed to update user.");
    }

    [HttpPut("UpdateGoals")]
    public async Task<ActionResult> UpdateUserGoals([FromBody] UserUpdateGoalsDto userUpdateGoalsDto)
    {
        var userId = await AuthHelper.GetUserIdFromGoogleJwtTokenAsync(HttpContext);
        var userDb = await userRepository.GetUserByIdAsync(userId);
        
        if (userDb is null) return Unauthorized();

        _mapper.Map(userUpdateGoalsDto, userDb);

        userRepository.UpdateEntity(userDb);

        return await userRepository.SaveChangesAsync() ? Ok() : BadRequest("Failed to update user goals.");
    }

    [HttpDelete("DeleteUser")]
    public async Task<ActionResult> DeleteUser()
    {
        var userId = await AuthHelper.GetUserIdFromGoogleJwtTokenAsync(HttpContext);
        var userDb = await userRepository.GetUserByIdAsync(userId);
        
        if (userDb is null) return NotFound();
        
        userRepository.DeleteEntity(userDb);

        return await userRepository.SaveChangesAsync() ? Ok() : BadRequest("Failed to delete user.");
    }
}