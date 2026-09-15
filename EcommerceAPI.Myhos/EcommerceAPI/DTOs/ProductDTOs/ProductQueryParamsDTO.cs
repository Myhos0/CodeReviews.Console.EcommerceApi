namespace EcommerceAPI.DTOs.ProductDTOs;

public class ProductQueryParamsDTO
{
    public int PageNumber { get; set; }
    public int PageSize { get; set; }

    public string? Name { get; set; }
    public int? CategoryId { get; set; }
    public decimal? MinPrice { get; set; }
    public decimal? MaxPrice { get; set; }
    public bool? InStock { get; set; }

    public string? SortBy { get; set; } = "Id";
    public bool Descending { get; set; } = false;
}