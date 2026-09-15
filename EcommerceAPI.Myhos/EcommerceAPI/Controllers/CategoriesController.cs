using EcommerceAPI.DTOs.CategoryDTOs;
using EcommerceAPI.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CategoriesController(ICategoryService categoryService) : ControllerBase
{
    private readonly ICategoryService _categoryService = categoryService;

    [HttpGet()]
    public async Task<IActionResult> GetCategories()
    {
        var result = await _categoryService.GetCategoriesAsync();

        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetCategoryById(int id)
    {
        var result = await _categoryService.GetCategoryByIdAsync(id);

        return Ok(result);
    }

    [HttpPost()]
    public async Task<IActionResult> CreatedCategory(CreatedCategoryDTO createdCategoryDTO)
    {
        var result = await _categoryService.CreateCategoryAsync(createdCategoryDTO);

        return CreatedAtAction(nameof(GetCategoryById), new { id = result.Id }, result);
    }

    [HttpPatch("{id}")]
    public async Task<IActionResult> UpdateCategory(int id, [FromBody] UpdatedCategoryDTO updatedCategoryDTO)
    {
        await _categoryService.UpdateCategoryAsync(id, updatedCategoryDTO);
        return NoContent();
    }
}