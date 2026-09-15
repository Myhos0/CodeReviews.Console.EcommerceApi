using EcommerceAPI.DTOs.CategoryDTOs;
using EcommerceAPI.Models;

namespace EcommerceAPI.Interfaces;

public interface ICategoryService
{
    public Task<List<CategoryDTO>> GetCategoriesAsync();
    public Task<CategoryDTO> GetCategoryByIdAsync(int id);
    public Task<Category> CreateCategoryAsync(CreatedCategoryDTO createCategoryDTO);
    public Task UpdateCategoryAsync(int id,UpdatedCategoryDTO updatedCategoryDTO);
}