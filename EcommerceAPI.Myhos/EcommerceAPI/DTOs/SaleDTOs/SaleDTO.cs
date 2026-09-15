using EcommerceAPI.Enums;

namespace EcommerceAPI.DTOs.SaleDTOs;

public class SaleDTO
{
    public int Id { get; set; }
    public DateTime SaleDate { get; set; }
    public decimal Total { get; set; }
    public SaleStatus Status { get; set; }
    public List<SaleDetailDTO>? Details { get; set; }
}