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
public class RecipeController(IRecipeRepository recipeRepository) : ControllerBase
{
    private readonly Mapper _mapper = new(new MapperConfiguration(c =>
    {
        c.CreateMap<RecipeDto, Recipe>()
            .ForMember(r => r.RecipeProducts, opt => opt.Ignore());
        c.CreateMap<RecipeProductDto, RecipeProduct>();
    }));

    [HttpPost("Create")]
    public async Task<ActionResult<string>> CreateRecipe([FromBody] RecipeDto recipeDto)
    {
        var userId = await AuthHelper.GetUserIdFromGoogleJwtTokenAsync(HttpContext);
        var userDb = await recipeRepository.GetUserByIdAsync(userId);

        if (userDb is null) return Unauthorized();

        var recipe = new Recipe(recipeDto.Name, recipeDto.Instructions, userDb.Id);

        foreach (var recipeProductDto in recipeDto.RecipeProducts)
        {
            var recipeProduct = _mapper.Map<RecipeProduct>(recipeProductDto);
            recipeProduct.RecipeId = recipe.Id;
            recipe.RecipeProducts.Add(recipeProduct);
        }

        await recipeRepository.AddEntityAsync(recipe);

        return await recipeRepository.SaveChangesAsync() ? Ok(recipe.Id) : Problem("Creating new recipe failed.");
    }

    [HttpPut("Update/{recipeId}")]
    public async Task<ActionResult<string>> UpdateRecipe([FromRoute] string recipeId, [FromBody] RecipeDto recipeDto)
    {
        var userId = await AuthHelper.GetUserIdFromGoogleJwtTokenAsync(HttpContext);
        var userDb = await recipeRepository.GetUserByIdAsync(userId);

        if (userDb is null) return Unauthorized();

        var recipeDb = await recipeRepository.GetRecipeByIdAsync(recipeId);

        if (recipeDb is null) return NotFound("Recipe not found.");
        if (recipeDb.OwnerId != userId)
            return Unauthorized("User needs to be the owner of the recipe to update it.");

        recipeDb.Name = recipeDto.Name;
        recipeDb.Instructions = recipeDto.Instructions;

        recipeDb.RecipeProducts.Clear();
        foreach (var recipeProductDto in recipeDto.RecipeProducts)
        {
            var recipeProduct = _mapper.Map<RecipeProduct>(recipeProductDto);
            recipeProduct.RecipeId = recipeDb.Id;
            recipeDb.RecipeProducts.Add(recipeProduct);
        }

        recipeRepository.UpdateEntity(recipeDb);

        return await recipeRepository.SaveChangesAsync() ? Ok(recipeId) : Problem("Updating recipe failed.");
    }

    [HttpGet("Get/{recipeId}")]
    public async Task<ActionResult<Recipe>> GetRecipe([FromRoute] string recipeId)
    {
        var recipeDb = await recipeRepository.GetRecipeByIdAsync(recipeId);

        if (recipeDb is null) return NotFound("Recipe not found.");

        return Ok(recipeDb);
    }

    [HttpGet("GetListOfRecipes")]
    public async Task<ActionResult<List<Recipe>>> GetListOfRecipes()
    {
        var recipes = await recipeRepository.GetRecipesAsync();

        return Ok(recipes);
    }

    [HttpGet("Search/{recipeName}")]
    public async Task<ActionResult<List<Recipe>>> SearchRecipes([FromRoute] string recipeName)
    {
        var recipes = await recipeRepository.SearchRecipesByNameAsync(recipeName);

        return Ok(recipes);
    }

    [HttpDelete("Delete/{recipeId}")]
    public async Task<ActionResult> DeleteRecipe([FromRoute] string recipeId)
    {
        var userId = await AuthHelper.GetUserIdFromGoogleJwtTokenAsync(HttpContext);
        var userDb = await recipeRepository.GetUserByIdAsync(userId);

        if (userDb is null) return Unauthorized();

        var recipeDb = await recipeRepository.GetRecipeByIdAsync(recipeId);

        if (recipeDb is null) return NotFound("Recipe not found.");
        if (recipeDb.OwnerId != userId)
            return Unauthorized("User needs to be the owner of the recipe to delete it.");

        recipeRepository.DeleteEntity(recipeDb);

        return await recipeRepository.SaveChangesAsync() ? Ok(recipeId) : Problem("Deleting recipe failed.");
    }
}
