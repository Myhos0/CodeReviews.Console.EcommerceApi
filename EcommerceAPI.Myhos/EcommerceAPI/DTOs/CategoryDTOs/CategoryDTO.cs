namespace EcommerceAPI.DTOs.CategoryDTOs;

public class CategoryDTO
{
    public int Id { get; set; }
    public string? Name { get; set; } 
    public string? Description { get; set; }
    public List<CategoryProductDTO>? Products { get; set; }
}