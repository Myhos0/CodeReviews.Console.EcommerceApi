using EcommerceAPI.Data;
using EcommerceAPI.DTOs.CategoryDTOs;
using EcommerceAPI.Exceptions;
using EcommerceAPI.Interfaces;
using EcommerceAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace EcommerceAPI.Services;

public class CategoryService(EcommerceContextDB contextDB, ILogger<CategoryService> logger) : ICategoryService
{
    private readonly EcommerceContextDB _contextDB = contextDB;
    private readonly ILogger<CategoryService> _logger = logger;

    public async Task<Category> CreateCategoryAsync(CreatedCategoryDTO createCategoryDTO)
    {
        Category category = new()
        {
            Name = createCategoryDTO.Name == string.Empty ? "No name found": createCategoryDTO.Name,
            Description = createCategoryDTO.Description,
        };

        _contextDB.Add(category);
        await _contextDB.SaveChangesAsync();

        _logger.LogInformation("Category created: {CategoryName} (Id : {CategoryId})",
            category.Name, category.Id);

        return category;
    }

    public async Task<List<CategoryDTO>> GetCategoriesAsync()
    {
        var categories = _contextDB.Categories
            .Include(c => c.Products)
            .Select(c => new CategoryDTO
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description ?? string.Empty,
                Products = c.Products.Select(p => new CategoryProductDTO
                {
                    Id = p.Id,
                    Name = p.Name,
                    Price = p.Price,
                    Stock = p.Stock,
                }).ToList(),
            }).ToList();

        _logger.LogInformation("Retrieved {Count} categories", categories.Count);

        return categories;
    }

    public async Task<CategoryDTO> GetCategoryByIdAsync(int id)
    {
        var category = await _contextDB.Categories
            .Include(c => c.Products)
            .Where(c => c.Id == id)
            .Select(c => new CategoryDTO
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description ?? string.Empty,
                Products = c.Products.Select(p => new CategoryProductDTO
                {
                    Id = p.Id,
                    Name = p.Name,
                    Price = p.Price,
                    Stock = p.Stock,
                }).ToList()
            }).FirstOrDefaultAsync();

        if (category == null)
        {
            _logger.LogWarning("Category with id {CategoryId} was not found", id);
            throw new EcommerceException(EcommerceErrors.NotFound("Category", $"Category with id {id} was not found."));
        }

        _logger.LogInformation("Retrieved category with id: {CategoryId}.", category.Id);

        return category;
    }

    public async Task UpdateCategoryAsync(int id, UpdatedCategoryDTO updatedCategoryDTO)
    {
        Category? category = await _contextDB.Categories.FindAsync(id);

        if (category == null)
        {
            _logger.LogWarning("Attempted to update non-existent category with id {CategoryId}", id);
            throw new EcommerceException(EcommerceErrors.NotFound("Category", $"Category with id {id} was not found."));
        }

        if (updatedCategoryDTO.Name != null)
            category.Name = updatedCategoryDTO.Name;

        if (updatedCategoryDTO.Description != null)
            category.Description = updatedCategoryDTO.Description;

        await _contextDB.SaveChangesAsync();

        _logger.LogInformation("Category updated: {CategoryName} (Id: {CategoryId})", category.Name, category.Id);

    }
}