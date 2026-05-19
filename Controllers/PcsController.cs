using Czwiczenie7.Data;
using Czwiczenie7.DTOs;
using Czwiczenie7.Modele;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Czwiczenie7.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PcsController : ControllerBase
{
    private readonly AppDbContext _context;
    public PcsController(AppDbContext context)
    {
        _context = context;
    }
  
    [HttpGet("{id}/components")]
    public async Task<IActionResult> GetPcComponents(int id)
    {
        var pcExists = await _context.PCs.AnyAsync(p => p.Id == id);
        if (!pcExists)
        {
            return NotFound();
        }
        var pcDetails = await _context.PCs
            .Where(p => p.Id == id)
            .Select(p => new PcDetailsResponseDto
            {
                Id = p.Id,
                Name = p.Name,
                Weight = p.Weight,
                Warranty = p.Warranty,
                CreatedAt = p.CreatedAt,
                Stock = p.Stock,
                Components = p.PCComponents.Select(pcc => new PcComponentResponseDto
                {
                    Amount = pcc.Amount,
                    Component = new ComponentDetailsDto
                    {
                        Code = pcc.Component.Code,
                        Name = pcc.Component.Name,
                        Description = pcc.Component.Description,
                        Manufacturer = new ManufacturerDto
                        {
                            Id = pcc.Component.Manufacturer.Id,
                            Abbreviation = pcc.Component.Manufacturer.Abbreviation,
                            FullName = pcc.Component.Manufacturer.FullName,
                            FoundationDate = pcc.Component.Manufacturer.FoundationDate.ToString("yyyy-MM-dd")
                        },
                        Type = new TypeDto
                        {
                            Id = pcc.Component.Type.Id,
                            Abbreviation = pcc.Component.Type.Abbreviation,
                            Name = pcc.Component.Type.Name
                        }
                    }
                }).ToList()
            })
            .FirstOrDefaultAsync();

        return Ok(pcDetails);
    }
    
    [HttpGet]
    public async Task<IActionResult> GetPcs()
    {
        var pcs = await _context.PCs
            .Select(pc => new PcResponseDto
            {
                Id = pc.Id,
                Name = pc.Name,
                Weight = pc.Weight,
                Warranty = pc.Warranty,
                CreatedAt = pc.CreatedAt,
                Stock = pc.Stock
            })
            .ToListAsync();
        return Ok(pcs);
    }
    
    [HttpPost]
    public async Task<IActionResult> AddPc([FromBody] PcPostRequestDto request)
    {
        var newPc = new PC
        {
            Name = request.Name,
            Weight = request.Weight,
            Warranty = request.Warranty,
            CreatedAt = request.CreatedAt,
            Stock = request.Stock
        };
        
        _context.PCs.Add(newPc);
        await _context.SaveChangesAsync(); 
        var response = new PcResponseDto
        {
            Id = newPc.Id,
            Name = newPc.Name,
            Weight = newPc.Weight,
            Warranty = newPc.Warranty,
            CreatedAt = newPc.CreatedAt,
            Stock = newPc.Stock
        };
        return StatusCode(StatusCodes.Status201Created, response);
    }
    
    // PUT: api/pcs/{id}
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdatePc(int id, [FromBody] PcPostRequestDto request)
    {
        var pc = await _context.PCs.FindAsync(id);
        
        if (pc == null)
        {
            return NotFound();
        }
        
        pc.Name = request.Name;
        pc.Weight = request.Weight;
        pc.Warranty = request.Warranty;
        pc.CreatedAt = request.CreatedAt;
        pc.Stock = request.Stock;

        await _context.SaveChangesAsync();
        var response = new PcResponseDto
        {
            Id = pc.Id,
            Name = pc.Name,
            Weight = pc.Weight,
            Warranty = pc.Warranty,
            CreatedAt = pc.CreatedAt,
            Stock = pc.Stock
        };
        return Ok(response);
    }
    
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePc(int id)
    {
        var pc = await _context.PCs.FindAsync(id);
        
        if (pc == null)
        {
            return NotFound();
        }
        
        _context.PCs.Remove(pc);
        await _context.SaveChangesAsync();
        return NoContent(); 
    }
}