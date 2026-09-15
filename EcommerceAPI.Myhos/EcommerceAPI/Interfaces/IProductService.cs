using EcommerceAPI.DTOs;
using EcommerceAPI.DTOs.ProductDTOs;
using EcommerceAPI.Models;

namespace EcommerceAPI.Interfaces;

public interface IProductService
{
    public Task<PagedResultDTO<ProductDTO>> GetProductsAsync(ProductQueryParamsDTO productQuery);
    public Task<ProductDTO> GetProductByIdAsync(int id);
    public Task<Product> CreateProductAsync(CreateProductDTO createProductDTO);
    public Task UpdateProductAsync(int id,UpdateProductDTO updateProductDTO);
}