using System.ComponentModel.DataAnnotations;

namespace EcommerceAPI.DTOs.SaleDTOs;

public class CreateSaleDetailDTO
{
    [Required]
    public int ProductId { get; set; }
    [Range(1, int.MaxValue, ErrorMessage = "Quantity must be greater than 0.")]
    public int Quantity { get; set; }
}