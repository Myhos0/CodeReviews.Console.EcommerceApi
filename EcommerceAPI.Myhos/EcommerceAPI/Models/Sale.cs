using EcommerceAPI.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EcommerceAPI.Models;

public class Sale
{
    [Key]
    public int Id { get; set; }

    public DateTime SaleDate { get; set; } = DateTime.Now;

    [Column(TypeName = "decimal(18,2)")]
    public decimal Total { get; set; }
    public SaleStatus Status { get; set; } = SaleStatus.Completed;
    public ICollection<SaleDetail> SaleDetails { get; set; } = new List<SaleDetail>();
}