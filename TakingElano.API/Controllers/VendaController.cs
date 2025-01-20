using Microsoft.AspNetCore.Mvc;
using TakingElano.Application.DTOs;
using TakingElano.Application.Interfaces;

namespace TakingElanoApi.Controllers;

[ApiController]
[Route("api/vendas")]
public class VendasController : ControllerBase
{
    private readonly IVendaService _vendaService;

    public VendasController(IVendaService vendaService)
    {
        _vendaService = vendaService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var vendas = await _vendaService.ObterTodasVendasAsync();
        return Ok(vendas);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var venda = await _vendaService.ObterVendaPorIdAsync(id);
        if (venda == null) return NotFound();
        return Ok(venda);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] VendaDto vendaDto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        await _vendaService.CriarVendaAsync(vendaDto);
        return CreatedAtAction(nameof(GetById), new { id = vendaDto.Id }, vendaDto);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] VendaDto vendaDto)
    {
        if (id != vendaDto.Id) return BadRequest("O ID na URL e no corpo não coincidem.");
        if (!ModelState.IsValid) return BadRequest(ModelState);

        await _vendaService.AtualizarVendaAsync(vendaDto);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _vendaService.ExcluirVendaAsync(id);
        return NoContent();
    }
}
