using calorieCounter_backend.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace calorieCounter_backend.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController(IUserRepository userRepository) : ControllerBase
{
    // Code HERE
}