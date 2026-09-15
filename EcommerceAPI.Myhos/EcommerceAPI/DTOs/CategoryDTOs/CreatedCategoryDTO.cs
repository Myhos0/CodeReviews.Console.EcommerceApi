using System.ComponentModel.DataAnnotations;

namespace EcommerceAPI.DTOs.CategoryDTOs;

public class CreatedCategoryDTO
{
    [Required]
    [StringLength(100, MinimumLength = 5)]
    public string Name { get; set; } = string.Empty;
    
    public string? Description { get; set; }
}