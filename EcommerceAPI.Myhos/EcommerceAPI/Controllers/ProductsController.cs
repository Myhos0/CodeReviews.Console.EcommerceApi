using EcommerceAPI.DTOs.ProductDTOs;
using EcommerceAPI.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController (IProductService productService) : ControllerBase
{
    private readonly IProductService _productService = productService;

    [HttpGet()]
    public async Task<IActionResult> GetProducts([FromQuery] ProductQueryParamsDTO queryParams) 
    {
        if (queryParams.PageNumber < 1) queryParams.PageNumber = 1;
        if (queryParams.PageSize < 1 || queryParams.PageSize > 100) queryParams.PageSize = 10;

        var result = await _productService.GetProductsAsync(queryParams);

        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetProductById(int id) 
    {
        var result = await _productService.GetProductByIdAsync(id);

        return Ok(result);
    }

    [HttpPost()]
    public async Task<IActionResult> CreateProduct(CreateProductDTO createProductDTO) 
    {
        var result = await _productService.CreateProductAsync(createProductDTO);

        return CreatedAtAction(nameof(GetProductById), new { id = result.Id},result);
    }

    [HttpPatch("{id}")]
    public async Task<IActionResult> UpdateProduct(int id, [FromBody] UpdateProductDTO updateProductDTO) 
    {
        await _productService.UpdateProductAsync(id, updateProductDTO);

        return NoContent();
    }
}