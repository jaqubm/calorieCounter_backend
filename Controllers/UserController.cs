using calorieCounter_backend.Models;
using calorieCounter_backend.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace calorieCounter_backend.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController(IUserRepository _userRepository) : ControllerBase
{
    [HttpPost("add")]
    public IActionResult AddUser([FromBody] User newUser)
    {
        if (newUser == null || string.IsNullOrEmpty(newUser.Email) || string.IsNullOrEmpty(newUser.Name))
        {
            return BadRequest("Invalid user data.");
        }

        _userRepository.AddEntity(newUser);

        if (_userRepository.SaveChanges())
        {
            return Ok("User created successfully.");
        }
        else
        {
            return StatusCode(500, "An error occurred while saving the user.");
        }
    }

    [HttpPut("update/{id}")]
    public IActionResult UpdateUser(int id, [FromBody] User updatedUser)
    {
        if (updatedUser == null || string.IsNullOrEmpty(updatedUser.Email) || string.IsNullOrEmpty(updatedUser.Name))
        {
            return BadRequest("Invalid user data. Email and Name are required.");
        }

        if (id != updatedUser.Id)
        {
            return BadRequest("User ID mismatch.");
        }

        _userRepository.UpdateEntity(updatedUser);

        if (_userRepository.SaveChanges())
        {
            return Ok("User updated successfully.");
        }
        else
        {
            return StatusCode(500, "An error occurred while updating the user.");
        }
    }
}