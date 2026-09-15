using EcommerceAPI.Data;
using EcommerceAPI.DTOs;
using EcommerceAPI.DTOs.SaleDTOs;
using EcommerceAPI.Enums;
using EcommerceAPI.Exceptions;
using EcommerceAPI.Interfaces;
using EcommerceAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace EcommerceAPI.Services;

public class SaleService(EcommerceContextDB contextDB, ILogger<SaleService> logger) : ISaleService
{
    private readonly EcommerceContextDB _contextDB = contextDB;
    private readonly ILogger<SaleService> _logger = logger;

    public async Task<SaleDTO> CreateSaleAsync(CreateSaleDTO createSaleDTO)
    {
        var productsIds = createSaleDTO.Products.Select(p => p.ProductId)
             .Distinct()
             .ToList();

        var products = await _contextDB.Products
            .Where(p => productsIds.Contains(p.Id))
            .ToListAsync();

        var missingIds = productsIds.Except(products.Select(p => p.Id)).ToList();
        if (missingIds.Any())
        {
            _logger.LogWarning("Attempted to create sale with non-existent product ids: {Ids}",
                string.Join(",", missingIds));
            throw new EcommerceException(EcommerceErrors.NotFound("Sale", $"Product(s) with id(s) {string.Join(", ", missingIds)} were not found."));
        }

        foreach (var item in createSaleDTO.Products)
        {
            var product = products.First(p => p.Id == item.ProductId);

            if (product.Stock < item.Quantity)
            {
                _logger.LogWarning("Insufficient stock for product {ProductId}. Requested: {Requested}, Available: {Available}",
                product.Id, item.Quantity, product.Stock);
                throw new EcommerceException(EcommerceErrors.BusinessRule(
                    "Sale",
                    $"Insufficient stock for product '{product.Name}'." +
                    $" Requested: {item.Quantity}, Available: {product.Stock}."));
            }
        }

        var sale = new Sale
        {
            SaleDate = DateTime.UtcNow,
            Status = SaleStatus.Completed,
            SaleDetails = new List<SaleDetail>()
        };

        decimal total = 0;

        foreach (var item in createSaleDTO.Products)
        {
            var product = products.First(p => p.Id == item.ProductId);
            var subtotal = item.Quantity * product.Price;

            sale.SaleDetails.Add(new SaleDetail
            {
                ProductId = product.Id,
                Quantity = item.Quantity,
                UnitPrice = product.Price,
                Subtotal = subtotal
            });

            product.Stock -= item.Quantity;

            total += subtotal;
        }

        sale.Total = total;

        _contextDB.Sales.Add(sale);
        await _contextDB.SaveChangesAsync();


        _logger.LogInformation("Sale created: Id {SaleId}, Total {Total}", sale.Id, sale.Total);

        return new SaleDTO
        {
            Id = sale.Id,
            SaleDate = sale.SaleDate,
            Total = sale.Total,
            Status = sale.Status,
            Details = sale.SaleDetails.Select(d => new SaleDetailDTO
            {
                Id = d.ProductId,
                ProductName = products.First(p => p.Id == d.ProductId).Name,
                Quantity = d.Quantity,
                UnitPrice = d.UnitPrice,
                Subtotal = d.Subtotal
            }).ToList()
        };
    }

    public async Task<SaleDTO> GetSaleByIdAsync(int id)
    {
        var sale = await _contextDB.Sales
            .Include(s => s.SaleDetails)
            .Where(s => s.Id == id)
            .Select(s => new SaleDTO
            {
                Id = s.Id,
                SaleDate = s.SaleDate,
                Total = s.Total,
                Status = s.Status,
                Details = s.SaleDetails.Select(sd => new SaleDetailDTO
                {
                    Id = sd.Id,
                    ProductName = sd.Product.Name,
                    Quantity = sd.Quantity,
                    UnitPrice = sd.UnitPrice,
                    Subtotal = sd.Subtotal,
                }).ToList(),
            }).FirstOrDefaultAsync();

        if (sale == null)
        {
            _logger.LogWarning("Sale with id {SaleId} was not found", id);
            throw new EcommerceException(EcommerceErrors.NotFound("Sale", $"Sale with id {id} was not found."));
        }

        _logger.LogInformation("Retrieve sale with Id: {SaleId}", sale.Id);

        return sale;
    }

    public async Task<PagedResultDTO<SaleDTO>> GetSalesAsync(int pageNumber, int PageSize)
    {
        var query = _contextDB.Sales.AsQueryable();

        var totalCount = await query.CountAsync();

        var sales = await query
            .OrderBy(s => s.Id)
            .Skip((pageNumber - 1) * PageSize)
            .Take(PageSize)
            .Select(s => new SaleDTO
            {
                Id = s.Id,
                SaleDate = s.SaleDate,
                Total = s.Total,
                Status = s.Status,
                Details = s.SaleDetails.Select(sd => new SaleDetailDTO
                {
                    Id = sd.Id,
                    ProductName = sd.Product.Name,
                    Quantity = sd.Quantity,
                    UnitPrice = sd.UnitPrice,
                    Subtotal = sd.Subtotal,
                }).ToList(),
            }).ToListAsync();

        _logger.LogInformation("Retrieved {Sales} Sales", sales.Count);

        return new PagedResultDTO<SaleDTO>
        {
            items = sales,
            PageNumber = pageNumber,
            PageSize = PageSize,
            TotalCount = totalCount
        };
    }

    public async Task UpdateSaleStatusAsync(int id, SaleStatus newStatus)
    {
        var sale = _contextDB.Sales.Find(id);

        if (sale == null)
        {
            _logger.LogWarning("Sale with id {SaleId} was not found", id);
            throw new EcommerceException(EcommerceErrors.NotFound("Sale", $"Sale with id {id} was not found."));
        }

        sale.Status = newStatus;
        await _contextDB.SaveChangesAsync();

        _logger.LogInformation("Sale status updated:(Id: {SaleId})", sale.Status);

    }
}