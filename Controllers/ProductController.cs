using AutoMapper;
using calorieCounter_backend.Dtos;
using calorieCounter_backend.Models;
using calorieCounter_backend.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace calorieCounter_backend.Controllers;

[ApiController]
[Route("[controller]")]
public class ProductController(IProductRepository productRepository, IUserRepository userRepository) : ControllerBase
{
    private readonly Mapper _mapper = new(new MapperConfiguration(c =>
    {
        c.CreateMap<ProductDto, Product>();
    })); 
    
    [HttpPost("Create")]
    public ActionResult<string> CreateProduct([FromBody] ProductDto productDto)
    {
        if (productDto.OwnerEmail is not null)
        {
            var userDb = userRepository.GetUserByEmail(productDto.OwnerEmail);
            
            if (userDb is null) return NotFound("User who tries to create new product does not exist.");
        }
        
        if (productDto.ValuesPer <= 0) return Problem("Values per product must be greater than zero.");
        if (productDto.Energy <= 0) return Problem("Energy must be greater than zero.");
        if (productDto.Protein <= 0) return Problem("Protein must be greater than zero.");
        if (productDto.Fat <= 0) return Problem("Fat must be greater than zero.");
        if (productDto.Carbohydrates <= 0) return Problem("Carbohydrates must be greater than zero.");
        
        var product = _mapper.Map<Product>(productDto);
        
        productRepository.AddEntity(product);
        
        return productRepository.SaveChanges() ? Ok(product.Id) : Problem("Creating new product failed.");
    }

    [HttpPut("Update/{productId}")]
    public ActionResult<string> UpdateProduct([FromRoute] string productId, [FromBody] ProductDto productDto)
    {
        var productDb = productRepository.GetProductById(productId);
        
        if (productDb is null) return NotFound("Product not found.");
        
        if (productDto.OwnerEmail is null || productDb.OwnerEmail is null) 
            return Unauthorized("User needs to be owner of the product in order to update it.");
        
        if (productDto.OwnerEmail != productDb.OwnerEmail) 
            return Unauthorized("User needs to be owner of the product in order to update it.");
        
        if (productDto.ValuesPer <= 0) return Problem("Values per product must be greater than zero.");
        if (productDto.Energy <= 0) return Problem("Energy must be greater than zero.");
        if (productDto.Protein <= 0) return Problem("Protein must be greater than zero.");
        if (productDto.Fat <= 0) return Problem("Fat must be greater than zero.");
        if (productDto.Carbohydrates <= 0) return Problem("Carbohydrates must be greater than zero.");
        
        _mapper.Map(productDto, productDb);
        
        productRepository.UpdateEntity(productDb);
        
        return productRepository.SaveChanges() ? Ok(productId) : Problem("Updating product failed.");
    }

    [HttpGet("Get/{productId}")]
    public ActionResult<Product> GetProduct([FromRoute] string productId)
    {
        var productDb = productRepository.GetProductById(productId);
        
        if (productDb is null) return NotFound("Product not found.");
        
        return Ok(productDb);
    }
    
    [HttpGet("GetList")]
    public ActionResult<List<Product>> GetListOfProducts()
    {
        var listOfProductsDb = productRepository.GetProductsByName(string.Empty);
        
        return Ok(listOfProductsDb);
    }

    [HttpGet("Search/{productName}")]
    public ActionResult<List<Product>> SearchForProduct([FromRoute] string productName)
    {
        var listOfProductsDb = productRepository.GetProductsByName(productName);
        
        return Ok(listOfProductsDb);
    }

    [HttpDelete("Delete/{productId}")]
    public ActionResult DeleteProduct([FromRoute] string productId)
    {
        var productDb = productRepository.GetProductById(productId);
        
        if (productDb is null) return NotFound("Product not found.");
        
        productRepository.DeleteEntity(productDb);
        
        return productRepository.SaveChanges() ? Ok(productId) : Problem("Deleting product failed.");
    }
}