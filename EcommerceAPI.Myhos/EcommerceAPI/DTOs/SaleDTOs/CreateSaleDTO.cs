using System.ComponentModel.DataAnnotations;

namespace EcommerceAPI.DTOs.SaleDTOs;

public class CreateSaleDTO
{
    [Required, MinLength(1, ErrorMessage = "A sale must include at least one product.")]
    public List<CreateSaleDetailDTO> Products { get; set; } = new();
}