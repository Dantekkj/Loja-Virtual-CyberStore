using LojaVirtual.Business.DTOs;
using LojaVirtual.Business.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LojaVirtual.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ComputadorController : ControllerBase
{
	private readonly IComputadorService _computadorService;

	public ComputadorController(IComputadorService computadorService)
	{
		_computadorService = computadorService;
	}

	[HttpGet]
	public ActionResult<List<ComputadorDTO>> Get([FromQuery] ComputadorFiltroDTO filtro)
	{
		List<ComputadorDTO> lista = _computadorService.ObterTodos();
		return Ok(lista);
	}

	[HttpGet("{id}")]
	public ActionResult<ComputadorDTO> Get(int id)
	{
		var comp = _computadorService.ObterPorId(id);
		if (comp == null) return NotFound();
		return comp;
	}

	[HttpPost]
	public IActionResult Post([FromBody] ComputadorDTO computador)
	{
		_computadorService.Adicionar(computador);
		return CreatedAtAction(nameof(Get), new { id = computador.Id }, computador);
	}

	[HttpPut("{id}")]
	public IActionResult Put(int id, [FromBody] ComputadorDTO computador)
	{
		computador.Id = id;
		var atualizado = _computadorService.Atualizar(computador);
		if (!atualizado) return NotFound();
		return NoContent();
	}

	[HttpDelete("{id}")]
	public IActionResult Delete(int id)
	{
		var removido = _computadorService.Remover(id);
		if (!removido) return NotFound();
		return NoContent();
	}
}