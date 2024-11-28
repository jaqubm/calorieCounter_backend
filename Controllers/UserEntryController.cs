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
public class UserEntriesController(IUserEntryRepository userEntryRepository, IProductRepository productRepository, IRecipeRepository recipeRepository) : ControllerBase
{
    private readonly Mapper _mapper = new(new MapperConfiguration(c =>
    {
        c.CreateMap<UserEntry, UserEntryDto>()
            .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product!.Name))
            .ForMember(dest => dest.RecipeName, opt => opt.MapFrom(src => src.Recipe!.Name));
    }));

    [HttpGet("Get")]
    public async Task<ActionResult<List<UserEntryDto>>> GetUserEntries([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
    {
        var userId = await AuthHelper.GetUserIdFromGoogleJwtTokenAsync(HttpContext);

        var userEntriesDb = await userEntryRepository.GetUserEntriesByUserIdAsync(userId);

        var filteredEntries = userEntriesDb
            .Where(entry => entry.Date >= startDate && entry.Date <= endDate)
            .Select(entry => _mapper.Map<UserEntryDto>(entry))
            .ToList();

        return Ok(filteredEntries);
    }

    [HttpPost("Add")]
    public async Task<ActionResult<string>> AddUserEntry([FromBody] UserEntryCreatorDto userEntryCreatorDto)
    {
        var userId = await AuthHelper.GetUserIdFromGoogleJwtTokenAsync(HttpContext);

        if (userEntryCreatorDto.EntryType == "Product" && string.IsNullOrEmpty(userEntryCreatorDto.ProductId))
            return BadRequest("ProductId is required for Product entries.");
        if (userEntryCreatorDto.EntryType == "Recipe" && string.IsNullOrEmpty(userEntryCreatorDto.RecipeId))
            return BadRequest("RecipeId is required for Recipe entries.");

        string? productId = null;
        string? recipeId = null;

        if (userEntryCreatorDto.EntryType == "Product")
        {
            productId = userEntryCreatorDto.ProductId;
            var productDb = await productRepository.GetProductByIdAsync(productId);
            if (productDb is null) return NotFound("Product not found.");
        }
        else if (userEntryCreatorDto.EntryType == "Recipe")
        {
            recipeId = userEntryCreatorDto.RecipeId;
            var recipeDb = await recipeRepository.GetRecipeByIdAsync(recipeId);
            if (recipeDb is null) return NotFound("Recipe not found.");
        }

        var userEntry = new UserEntry(
            userId,
            userEntryCreatorDto.EntryType,
            userEntryCreatorDto.Date,
            userEntryCreatorDto.MealType,
            productId,
            recipeId,
            userEntryCreatorDto.Weight
        );

        await userEntryRepository.AddEntityAsync(userEntry);

        return await userEntryRepository.SaveChangesAsync() ? Ok(userEntry.Id) : Problem("Failed to add user entry.");
    }

    [HttpDelete("Delete/{entryId}")]
    public async Task<ActionResult> DeleteUserEntry([FromRoute] string entryId)
    {
        var userId = await AuthHelper.GetUserIdFromGoogleJwtTokenAsync(HttpContext);

        var userEntry = await userEntryRepository.GetUserEntryByIdAsync(entryId);
        if (userEntry is null) return NotFound("User entry not found.");
        if (userEntry.UserId != userId) return Unauthorized("You are not authorized to delete this entry.");

        userEntryRepository.DeleteEntity(userEntry);

        return await userEntryRepository.SaveChangesAsync() ? Ok() : Problem("Failed to delete user entry.");
    }
}
