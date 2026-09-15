using EcommerceAPI.Data;
using EcommerceAPI.DTOs;
using EcommerceAPI.DTOs.ProductDTOs;
using EcommerceAPI.Exceptions;
using EcommerceAPI.Interfaces;
using EcommerceAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace EcommerceAPI.Services;

public class ProductService(EcommerceContextDB contextDB, ILogger<ProductService> logger) : IProductService
{
    private readonly EcommerceContextDB _contextDB = contextDB;
    private readonly ILogger<ProductService> _logger = logger;

    public async Task<Product> CreateProductAsync(CreateProductDTO createProductDTO)
    {
        var categoryExists = await _contextDB.Categories.AnyAsync(c => c.Id == createProductDTO.CategoryId);

        if (!categoryExists)
        {
            _logger.LogWarning("Attempted to create product with non-existent category id {CategoryId}",
                createProductDTO.CategoryId);

            throw new EcommerceException(EcommerceErrors.NotFound("Product", $"Category with id {createProductDTO.CategoryId} was not found."));
        }

        Product product = new Product()
        {
            Name = createProductDTO.Name == string.Empty ? "No name found": createProductDTO.Name,
            Description = createProductDTO.Description,
            Price = createProductDTO.Price,
            Stock = createProductDTO.Stock,
            CategoryId = createProductDTO.CategoryId,
        };

        _contextDB.Add(product);
        await _contextDB.SaveChangesAsync();

        _logger.LogInformation("Product created : {ProductName} (Id : {ProductId})",
            product.Name, product.Id);

        return product;
    }

    public async Task<ProductDTO> GetProductByIdAsync(int id)
    {
        var product = await _contextDB.Products
            .AsNoTracking()
            .Include(c => c.Category)
            .Where(c => c.Id == id)
            .Select(c => new ProductDTO
            {
                Id = c.Id,
                Name = c.Name,
                Price = c.Price,
                Stock = c.Stock,
                Description = c.Description,
                CategoryName = c.Category.Name
            }).FirstOrDefaultAsync();

        if (product == null)
        {
            _logger.LogWarning("Product with id {ProductId} was not found", id);

            throw new EcommerceException(EcommerceErrors.NotFound("Product",$"Product with id {id} was not found"));
        }

        _logger.LogInformation("Retrieved product with id : {ProductId}", product.Id);

        return product;
    }

    public async Task<PagedResultDTO<ProductDTO>> GetProductsAsync(ProductQueryParamsDTO queryParams)
    {
        var query = _contextDB.Products.AsNoTracking().AsQueryable();

        if (!string.IsNullOrWhiteSpace(queryParams.Name))
            query = query.Where(p => p.Name.Contains(queryParams.Name));

        if (queryParams.CategoryId.HasValue)
            query = query.Where(p => p.CategoryId == queryParams.CategoryId.Value);

        if (queryParams.MinPrice.HasValue)
            query = query.Where(p => p.Price >= queryParams.MinPrice.Value);

        if (queryParams.MaxPrice.HasValue)
            query = query.Where(p => p.Price <= queryParams.MaxPrice.Value);

        if (queryParams.InStock == true)
            query = query.Where(p => p.Stock > 0);

        query = queryParams.SortBy?.ToLower() switch
        {
            "name" => queryParams.Descending ? query.OrderByDescending(p => p.Name) : query.OrderBy(p => p.Name),
            "price" => queryParams.Descending ? query.OrderByDescending(p => p.Price) : query.OrderBy(p => p.Price),
            "stock" => queryParams.Descending ? query.OrderByDescending(p => p.Stock) : query.OrderBy(p => p.Stock),
            _ => queryParams.Descending ? query.OrderByDescending(p => p.Id) : query.OrderBy(p => p.Id)
        };

        var totalCount = await query.CountAsync();

        var products = await query
            .Skip((queryParams.PageNumber - 1) * queryParams.PageSize)
            .Take(queryParams.PageSize)
            .Select(p => new ProductDTO
            {
                Id = p.Id,
                Name = p.Name,
                Price = p.Price,
                Stock = p.Stock,
                CategoryName = p.Category.Name
            })
            .ToListAsync();

        _logger.LogInformation("Retrieved {Count} product", products.Count);

        return new PagedResultDTO<ProductDTO>
        {
            items = products,
            PageNumber = queryParams.PageNumber,
            PageSize = queryParams.PageSize,
            TotalCount = totalCount
        };
    }

    public async Task UpdateProductAsync(int id, UpdateProductDTO updateProductDTO)
    {
        var product = _contextDB.Products.Find(id);

        if (product == null)
        {
            _logger.LogWarning("Attempted to update non-existent category with id {ProductId}", id);
            throw new EcommerceException(EcommerceErrors.NotFound("Product",$"Product with id {id} was not found."));
        }

        if (updateProductDTO.Name != null)
            product.Name = updateProductDTO.Name;

        if (updateProductDTO.Description != null)
            product.Description = updateProductDTO.Description;

        if (updateProductDTO.Stock != null)
            product.Stock = (int)updateProductDTO.Stock;

        if (updateProductDTO.CategoryId != null)
        {
            var categoryExists = await _contextDB.Categories
                .AnyAsync(c => c.Id == updateProductDTO.CategoryId);

            if (!categoryExists)
            {
                _logger.LogWarning("Attempted to assign non-existent category id {CategoryId} to product {ProductId}",
                    updateProductDTO.CategoryId, id);
                throw new EcommerceException(EcommerceErrors.NotFound("Product",$"Category with id {updateProductDTO.CategoryId} was not found."));
            }

            product.CategoryId = (int)updateProductDTO.CategoryId;
        }

        await _contextDB.SaveChangesAsync();

        _logger.LogInformation("Product updated: {ProductName} (Id: {ProductId})",
            product.Name, product.Id);
    }
}