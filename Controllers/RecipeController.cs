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
        var userId = await AuthHelper.GetUserIdFromGoogleJwtTokenAsync(HttpContext);
        var recipeDb = await recipeRepository.GetRecipeByIdAsync(recipeId);

        if (recipeDb is null) return NotFound("Recipe not found.");

        var recipeDto = new RecipeDto
        {
            Id = recipeDb.Id,
            Name = recipeDb.Name,
            Instructions = recipeDb.Instructions,
            RecipeProducts = recipeDb.RecipeProducts.Select(rp => new RecipeProductDto
            {
                ProductId = rp.ProductId,
                ProductName = rp.Product.Name,
                Weight = rp.Weight,
                EnergyPerWeight = rp.Product.Energy * (rp.Weight / rp.Product.ValuesPer),
                ProteinPerWeight = rp.Product.Protein * (rp.Weight / rp.Product.ValuesPer),
                CarbohydratesPerWeight = rp.Product.Carbohydrates * (rp.Weight / rp.Product.ValuesPer),
                FatPerWeight = rp.Product.Fat * (rp.Weight / rp.Product.ValuesPer)
            }).ToList(),
            TotalWeight = recipeDb.RecipeProducts.Sum(rp => rp.Weight),
            TotalEnergy = recipeDb.RecipeProducts.Sum(rp => rp.Product.Energy * (rp.Weight / rp.Product.ValuesPer)),
            TotalProtein = recipeDb.RecipeProducts.Sum(rp => rp.Product.Protein * (rp.Weight / rp.Product.ValuesPer)),
            TotalCarbohydrates = recipeDb.RecipeProducts.Sum(rp => rp.Product.Carbohydrates * (rp.Weight / rp.Product.ValuesPer)),
            TotalFat = recipeDb.RecipeProducts.Sum(rp => rp.Product.Fat * (rp.Weight / rp.Product.ValuesPer)),
            IsOwner = recipeDb.OwnerId == userId
        };

        return Ok(recipeDto);
    }

    [HttpGet("GetListOfRecipes")]
    public async Task<ActionResult<List<RecipeDto>>> GetListOfRecipes()
    {
        var userId = await AuthHelper.GetUserIdFromGoogleJwtTokenAsync(HttpContext);
        var recipeListDb = await recipeRepository.GetRecipesAsync();

        var recipeListDto = recipeListDb.Select(recipe => new RecipeDto
        {
            Id = recipe.Id,
            Name = recipe.Name,
            Instructions = recipe.Instructions,
            RecipeProducts = recipe.RecipeProducts.Select(rp => new RecipeProductDto
            {
                ProductId = rp.ProductId,
                ProductName = rp.Product.Name,
                Weight = rp.Weight,
                EnergyPerWeight = rp.Product.Energy * (rp.Weight / rp.Product.ValuesPer),
                ProteinPerWeight = rp.Product.Protein * (rp.Weight / rp.Product.ValuesPer),
                CarbohydratesPerWeight = rp.Product.Carbohydrates * (rp.Weight / rp.Product.ValuesPer),
                FatPerWeight = rp.Product.Fat * (rp.Weight / rp.Product.ValuesPer)
            }).ToList(),
            TotalWeight = recipe.RecipeProducts.Sum(rp => rp.Weight),
            TotalEnergy = recipe.RecipeProducts.Sum(rp => rp.Product.Energy * (rp.Weight / rp.Product.ValuesPer)),
            TotalProtein = recipe.RecipeProducts.Sum(rp => rp.Product.Protein * (rp.Weight / rp.Product.ValuesPer)),
            TotalCarbohydrates = recipe.RecipeProducts.Sum(rp => rp.Product.Carbohydrates * (rp.Weight / rp.Product.ValuesPer)),
            TotalFat = recipe.RecipeProducts.Sum(rp => rp.Product.Fat * (rp.Weight / rp.Product.ValuesPer)),
            IsOwner = recipe.OwnerId == userId
        }).ToList();

        return Ok(recipeListDto);
    }

    [HttpGet("Search/{recipeName}")]
    public async Task<ActionResult<List<RecipeDto>>> SearchRecipes([FromRoute] string recipeName)
    {
        var userId = await AuthHelper.GetUserIdFromGoogleJwtTokenAsync(HttpContext);
        var recipeListDb = await recipeRepository.SearchRecipesByNameAsync(recipeName);

        var recipeListDto = recipeListDb.Select(recipe => new RecipeDto
        {
            Id = recipe.Id,
            Name = recipe.Name,
            Instructions = recipe.Instructions,
            RecipeProducts = recipe.RecipeProducts.Select(rp => new RecipeProductDto
            {
                ProductId = rp.ProductId,
                ProductName = rp.Product.Name,
                Weight = rp.Weight,
                EnergyPerWeight = rp.Product.Energy * (rp.Weight / rp.Product.ValuesPer),
                ProteinPerWeight = rp.Product.Protein * (rp.Weight / rp.Product.ValuesPer),
                CarbohydratesPerWeight = rp.Product.Carbohydrates * (rp.Weight / rp.Product.ValuesPer),
                FatPerWeight = rp.Product.Fat * (rp.Weight / rp.Product.ValuesPer)
            }).ToList(),
            TotalWeight = recipe.RecipeProducts.Sum(rp => rp.Weight),
            TotalEnergy = recipe.RecipeProducts.Sum(rp => rp.Product.Energy * (rp.Weight / rp.Product.ValuesPer)),
            TotalProtein = recipe.RecipeProducts.Sum(rp => rp.Product.Protein * (rp.Weight / rp.Product.ValuesPer)),
            TotalCarbohydrates = recipe.RecipeProducts.Sum(rp => rp.Product.Carbohydrates * (rp.Weight / rp.Product.ValuesPer)),
            TotalFat = recipe.RecipeProducts.Sum(rp => rp.Product.Fat * (rp.Weight / rp.Product.ValuesPer)),
            IsOwner = recipe.OwnerId == userId
        }).ToList();

        return Ok(recipeListDto);
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
