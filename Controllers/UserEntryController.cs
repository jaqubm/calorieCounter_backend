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
    [HttpGet("Get")]
    public async Task<ActionResult<List<UserEntryDto>>> GetUserEntries([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
    {
        var userId = await AuthHelper.GetUserIdFromGoogleJwtTokenAsync(HttpContext);

        var userEntriesDb = await userEntryRepository.GetUserEntriesByUserIdAsync(userId);

        var filteredEntries = userEntriesDb
            .Where(entry => entry.Date >= startDate && entry.Date <= endDate)
            .Select(entry =>
            {
                var entryDto = new UserEntryDto
                {
                    Id = entry.Id,
                    EntryType = entry.EntryType,
                    EntryId = entry.EntryType == "Product" ? entry.ProductId! : entry.RecipeId!,
                    EntryName = entry.EntryType == "Product" ? entry.Product?.Name : entry.Recipe?.Name,
                    Date = entry.Date,
                    MealType = entry.MealType,
                    Weight = entry.Weight
                };

                return entryDto;
            })
            .ToList();

        return Ok(filteredEntries);
    }

    [HttpPost("Add")]
    public async Task<ActionResult<string>> AddUserEntry([FromBody] UserEntryCreatorDto userEntryCreatorDto)
    {
        var userId = await AuthHelper.GetUserIdFromGoogleJwtTokenAsync(HttpContext);

        if (string.IsNullOrEmpty(userEntryCreatorDto.EntryType))
            return BadRequest("EntryType is required and must be either 'Product' or 'Recipe'.");
        if (string.IsNullOrEmpty(userEntryCreatorDto.Id))
            return BadRequest("Id is required.");

        string? productId = null;
        string? recipeId = null;

        if (userEntryCreatorDto.EntryType == "Product")
        {
            productId = userEntryCreatorDto.Id;
            var productDb = await productRepository.GetProductByIdAsync(productId);
            if (productDb is null) return NotFound("Product not found.");
        }
        else if (userEntryCreatorDto.EntryType == "Recipe")
        {
            recipeId = userEntryCreatorDto.Id;
            var recipeDb = await recipeRepository.GetRecipeByIdAsync(recipeId);
            if (recipeDb is null) return NotFound("Recipe not found.");
        }
        else
        {
            return BadRequest("Invalid EntryType. Must be 'Product' or 'Recipe'.");
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
