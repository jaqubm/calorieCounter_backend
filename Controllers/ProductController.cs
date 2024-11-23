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
        c.CreateMap<Product, ProductDto>();
        c.CreateMap<ProductCreatorDto, Product>();
    }));

    [HttpPost("Create")]
    public async Task<ActionResult<string>> CreateProduct([FromBody] ProductCreatorDto productCreatorDto)
    {
        var userId = await AuthHelper.GetUserIdFromGoogleJwtTokenAsync(HttpContext);
        var userDb = await productRepository.GetUserByIdAsync(userId);

        if (userDb is null) return Unauthorized();

        if (productCreatorDto.ValuesPer <= 0) return Problem("Values per product must be greater than zero.");
        if (productCreatorDto.Energy <= 0) return Problem("Energy must be greater than zero.");
        if (productCreatorDto.Protein <= 0) return Problem("Protein must be greater than zero.");
        if (productCreatorDto.Fat <= 0) return Problem("Fat must be greater than zero.");
        if (productCreatorDto.Carbohydrates <= 0) return Problem("Carbohydrates must be greater than zero.");

        var product = _mapper.Map<Product>(productCreatorDto);
        product.OwnerId = userDb.Id;

        await productRepository.AddEntityAsync(product);

        return await productRepository.SaveChangesAsync() ? Ok(product.Id) : Problem("Creating new product failed.");
    }

    [HttpPut("Update/{productId}")]
    public async Task<ActionResult<string>> UpdateProduct([FromRoute] string productId, [FromBody] ProductCreatorDto productCreatorDto)
    {
        var userId = await AuthHelper.GetUserIdFromGoogleJwtTokenAsync(HttpContext);
        var userDb = await productRepository.GetUserByIdAsync(userId);

        if (userDb is null) return Unauthorized();

        var productDb = await productRepository.GetProductByIdAsync(productId);

        if (productDb is null) return NotFound("Product not found.");

        if (productDb.OwnerId != userId)
            return Unauthorized("User needs to be owner of the product in order to update it.");

        if (productCreatorDto.ValuesPer <= 0) return Problem("Values per product must be greater than zero.");
        if (productCreatorDto.Energy <= 0) return Problem("Energy must be greater than zero.");
        if (productCreatorDto.Protein <= 0) return Problem("Protein must be greater than zero.");
        if (productCreatorDto.Fat <= 0) return Problem("Fat must be greater than zero.");
        if (productCreatorDto.Carbohydrates <= 0) return Problem("Carbohydrates must be greater than zero.");

        _mapper.Map(productCreatorDto, productDb);
        productDb.OwnerId = productDb.OwnerId;

        productRepository.UpdateEntity(productDb);

        return await productRepository.SaveChangesAsync() ? Ok(productId) : Problem("Updating product failed.");
    }

    [HttpGet("Get/{productId}")]
    public async Task<ActionResult<ProductDto>> GetProduct([FromRoute] string productId)
    {
        var userId = await AuthHelper.GetUserIdFromGoogleJwtTokenAsync(HttpContext);

        var productDb = await productRepository.GetProductByIdAsync(productId);
        if (productDb is null)
            return NotFound("Product not found.");

        var productDto = new ProductDto
        {
            Id = productDb.Id,
            Name = productDb.Name,
            ValuesPer = productDb.ValuesPer,
            Energy = productDb.Energy,
            Protein = productDb.Protein,
            Carbohydrates = productDb.Carbohydrates,
            Fat = productDb.Fat,
            IsOwner = productDb.OwnerId == userId
        };

        return Ok(productDto);
    }

    [HttpGet("GetList")]
    public async Task<ActionResult<List<ProductDto>>> GetListOfProducts()
    {
        var userId = await AuthHelper.GetUserIdFromGoogleJwtTokenAsync(HttpContext);

        var productListDb = await productRepository.GetProductsByNameAsync(string.Empty);
        var productListDto = productListDb.Select(product => new ProductDto
        {
            Id = product.Id,
            Name = product.Name,
            ValuesPer = product.ValuesPer,
            Energy = product.Energy,
            Protein = product.Protein,
            Carbohydrates = product.Carbohydrates,
            Fat = product.Fat,
            IsOwner = product.OwnerId == userId
        }).ToList();

        return Ok(productListDto);
    }

    [HttpGet("Search/{productName}")]
    public async Task<ActionResult<List<ProductDto>>> SearchForProduct([FromRoute] string productName)
    {
        var userId = await AuthHelper.GetUserIdFromGoogleJwtTokenAsync(HttpContext);

        var searchResultsOfProductsDb = await productRepository.GetProductsByNameAsync(productName);
        var searchResultsOfProductsDto = searchResultsOfProductsDb.Select(product => new ProductDto
        {
            Id = product.Id,
            Name = product.Name,
            ValuesPer = product.ValuesPer,
            Energy = product.Energy,
            Protein = product.Protein,
            Carbohydrates = product.Carbohydrates,
            Fat = product.Fat,
            IsOwner = product.OwnerId == userId
        }).ToList();

        return Ok(searchResultsOfProductsDto);
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