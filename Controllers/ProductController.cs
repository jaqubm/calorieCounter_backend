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
public class ProductController(IProductRepository productRepository) : ControllerBase
{
    private readonly Mapper _mapper = new(new MapperConfiguration(c =>
    {
        c.CreateMap<ProductDto, Product>();
    })); 
    
    [HttpPost("Create")]
    public async Task<ActionResult<string>> CreateProduct([FromBody] ProductDto productDto)
    {
        var userId = await AuthHelper.GetUserIdFromGoogleJwtTokenAsync(HttpContext);
        var userDb = await productRepository.GetUserByIdAsync(userId);
        
        if (userDb is null) return Unauthorized();
        
        productDto.OwnerId = userDb.Id;
        
        if (productDto.ValuesPer <= 0) return Problem("Values per product must be greater than zero.");
        if (productDto.Energy <= 0) return Problem("Energy must be greater than zero.");
        if (productDto.Protein <= 0) return Problem("Protein must be greater than zero.");
        if (productDto.Fat <= 0) return Problem("Fat must be greater than zero.");
        if (productDto.Carbohydrates <= 0) return Problem("Carbohydrates must be greater than zero.");
        
        var product = _mapper.Map<Product>(productDto);
        
        await productRepository.AddEntityAsync(product);
        
        return await productRepository.SaveChangesAsync() ? Ok(product.Id) : Problem("Creating new product failed.");
    }

    [HttpPut("Update/{productId}")]
    public async Task<ActionResult<string>> UpdateProduct([FromRoute] string productId, [FromBody] ProductDto productDto)
    {
        var userId = await AuthHelper.GetUserIdFromGoogleJwtTokenAsync(HttpContext);
        var userDb = await productRepository.GetUserByIdAsync(userId);
        
        if (userDb is null) return Unauthorized();
        
        var productDb = await productRepository.GetProductByIdAsync(productId);
        
        if (productDb is null) return NotFound("Product not found.");
        
        if (productDb.OwnerId != userId) 
            return Unauthorized("User needs to be owner of the product in order to update it.");
        
        if (productDto.ValuesPer <= 0) return Problem("Values per product must be greater than zero.");
        if (productDto.Energy <= 0) return Problem("Energy must be greater than zero.");
        if (productDto.Protein <= 0) return Problem("Protein must be greater than zero.");
        if (productDto.Fat <= 0) return Problem("Fat must be greater than zero.");
        if (productDto.Carbohydrates <= 0) return Problem("Carbohydrates must be greater than zero.");
        
        _mapper.Map(productDto, productDb);
        
        productRepository.UpdateEntity(productDb);
        
        return await productRepository.SaveChangesAsync() ? Ok(productId) : Problem("Updating product failed.");
    }

    [HttpGet("Get/{productId}")]
    public async Task<ActionResult<Product>> GetProduct([FromRoute] string productId)
    {
        var productDb = await productRepository.GetProductByIdAsync(productId);
        
        if (productDb is null) return NotFound("Product not found.");
        
        return Ok(productDb);
    }
    
    [HttpGet("GetList")]
    public async Task<ActionResult<List<Product>>> GetListOfProducts()
    {
        var listOfProductsDb = await productRepository.GetProductsByNameAsync(string.Empty);
        
        return Ok(listOfProductsDb);
    }

    [HttpGet("Search/{productName}")]
    public async Task<ActionResult<List<Product>>> SearchForProduct([FromRoute] string productName)
    {
        var searchResultsOfProductsByNameDb = await productRepository.GetProductsByNameAsync(productName);
        
        return Ok(searchResultsOfProductsByNameDb);
    }

    [HttpDelete("Delete/{productId}")]
    public async Task<ActionResult> DeleteProduct([FromRoute] string productId)
    {
        var userId = await AuthHelper.GetUserIdFromGoogleJwtTokenAsync(HttpContext);
        var userDb = await productRepository.GetUserByIdAsync(userId);
        
        if (userDb is null) return Unauthorized();
        
        var productDb = await productRepository.GetProductByIdAsync(productId);
        
        if (productDb is null) return NotFound("Product not found.");
        if (productDb.OwnerId != userId) 
            return Unauthorized("User needs to be owner of the product in order to delete it.");
        
        productRepository.DeleteEntity(productDb);
        
        return await productRepository.SaveChangesAsync() ? Ok(productId) : Problem("Deleting product failed.");
    }
}