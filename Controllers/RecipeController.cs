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
        c.CreateMap<Recipe, RecipeDto>();
        c.CreateMap<RecipeProduct, RecipeProductDto>();
    }));

    [HttpPost("Create")]
    public async Task<ActionResult<string>> CreateRecipe([FromBody] RecipeCreatorDto recipeCreatorDto)
    {
        var userId = await AuthHelper.GetUserIdFromGoogleJwtTokenAsync(HttpContext);

        var recipe = new Recipe(recipeCreatorDto.Name, recipeCreatorDto.Instructions, userId);

        foreach (var product in recipeCreatorDto.ProductsList)
        {
            var productDb = await recipeRepository.GetProductByIdAsync(product.ProductId);
            if (productDb is null) return NotFound("Product used in Recipe not found.");
            recipe.RecipeProducts.Add(new RecipeProduct(recipe.Id, product.ProductId, product.Weight));
        }

        await recipeRepository.AddEntityAsync(recipe);

        return await recipeRepository.SaveChangesAsync() ? Ok(recipe.Id) : Problem("Creating new recipe failed.");
    }

    [HttpPut("Update/{recipeId}")]
    public async Task<ActionResult<string>> UpdateRecipe([FromRoute] string recipeId, [FromBody] RecipeCreatorDto recipeCreatorDto)
    {
        var userId = await AuthHelper.GetUserIdFromGoogleJwtTokenAsync(HttpContext);

        var recipeDb = await recipeRepository.GetRecipeByIdAsync(recipeId);

        if (recipeDb is null) return NotFound("Recipe not found.");
        if (recipeDb.OwnerId != userId)
            return Unauthorized("User needs to be the owner of the recipe to update it.");

        recipeDb.Name = recipeCreatorDto.Name;
        recipeDb.Instructions = recipeCreatorDto.Instructions;

        recipeDb.RecipeProducts.Clear();
        foreach (var product in recipeCreatorDto.ProductsList)
        {
            var productDb = await recipeRepository.GetProductByIdAsync(product.ProductId);
            if (productDb is null) return NotFound("Product used in Recipe not found.");
            recipeDb.RecipeProducts.Add(new RecipeProduct(recipeDb.Id, product.ProductId, product.Weight));
        }

        recipeRepository.UpdateEntity(recipeDb);

        return await recipeRepository.SaveChangesAsync() ? Ok(recipeId) : Problem("Updating recipe failed.");
    }

    [HttpGet("Get/{recipeId}")]
    public async Task<ActionResult<RecipeDto>> GetRecipe([FromRoute] string recipeId)
    {
        var recipeDb = await recipeRepository.GetRecipeByIdAsync(recipeId);

        if (recipeDb is null) return NotFound("Recipe not found.");
        
        var recipe = _mapper.Map<RecipeDto>(recipeDb);

        return Ok(recipe);
    }

    [HttpGet("GetListOfRecipes")]
    public async Task<ActionResult<List<RecipeDto>>> GetListOfRecipes()
    {
        var recipeListDb = await recipeRepository.GetRecipesAsync();
        var recipeList = _mapper.Map<List<RecipeDto>>(recipeListDb);

        return Ok(recipeList);
    }

    [HttpGet("Search/{recipeName}")]
    public async Task<ActionResult<List<RecipeDto>>> SearchRecipes([FromRoute] string recipeName)
    {
        var recipeListDb = await recipeRepository.SearchRecipesByNameAsync(recipeName);
        var recipeList = _mapper.Map<List<RecipeDto>>(recipeListDb);

        return Ok(recipeList);
    }

    [HttpDelete("Delete/{recipeId}")]
    public async Task<ActionResult> DeleteRecipe([FromRoute] string recipeId)
    {
        var userId = await AuthHelper.GetUserIdFromGoogleJwtTokenAsync(HttpContext);

        var recipeDb = await recipeRepository.GetRecipeByIdAsync(recipeId);

        if (recipeDb is null) return NotFound("Recipe not found.");
        if (recipeDb.OwnerId != userId)
            return Unauthorized("User needs to be the owner of the recipe to delete it.");

        recipeRepository.DeleteEntity(recipeDb);

        return await recipeRepository.SaveChangesAsync() ? Ok(recipeId) : Problem("Deleting recipe failed.");
    }
}
