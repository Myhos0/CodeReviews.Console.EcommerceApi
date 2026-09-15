using EcommerceAPI.DTOs.SaleDTOs;
using EcommerceAPI.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SalesController(ISaleService saleService) : ControllerBase
{
    private readonly ISaleService _saleService = saleService;

    [HttpGet()]
    public async Task<IActionResult> GetSales([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10) 
    {
        if (pageNumber < 1) pageNumber = 1;
        if (pageSize < 1 || pageSize > 100) pageSize = 10;

        var result = await _saleService.GetSalesAsync(pageNumber, pageSize);

        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetSale(int id) 
    {
        var result = await _saleService.GetSaleByIdAsync(id);

        if(result == null) return NotFound();
    
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> CreateSale([FromBody] CreateSaleDTO createSaleDTO) 
    {
        var result = await _saleService.CreateSaleAsync(createSaleDTO);

        return CreatedAtAction(nameof(GetSale),new { id = result.Id},result);
    }

    [HttpPatch("{id}")]
    public async Task<IActionResult> UpdateSaleStatusAsync(int id, [FromBody] UpdateSaleDTO updateSaleDTO)  
    {
        await _saleService.UpdateSaleStatusAsync(id,updateSaleDTO.Status);

        return NoContent();
    }
}