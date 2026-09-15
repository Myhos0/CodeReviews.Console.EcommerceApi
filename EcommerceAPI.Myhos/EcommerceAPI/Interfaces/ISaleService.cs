using EcommerceAPI.DTOs;
using EcommerceAPI.DTOs.SaleDTOs;
using EcommerceAPI.Enums;

namespace EcommerceAPI.Interfaces;

public interface ISaleService
{
    public Task<SaleDTO> CreateSaleAsync(CreateSaleDTO createSaleDTO);
    public Task<PagedResultDTO<SaleDTO>> GetSalesAsync(int pageNumber, int PageSize);
    public Task<SaleDTO> GetSaleByIdAsync(int id);
    public Task UpdateSaleStatusAsync(int id, SaleStatus newStatus);
}