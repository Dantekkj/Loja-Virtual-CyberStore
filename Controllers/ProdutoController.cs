using LojaVirtual.API.Entities;
using LojaVirtual.Business.DTOs;
using LojaVirtual.Business.Interfaces;
using LojaVirtual.Business.Services;
using Microsoft.AspNetCore.Mvc;

namespace LojaVirtual.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProdutoController : ControllerBase
{
	private readonly IProdutoService _produtoService;

	public ProdutoController(IProdutoService produtoService)
	{
		_produtoService = produtoService;
	}

	[HttpGet]
	public ActionResult<IEnumerable<ProdutoDTO>> ObterTodos()
	{
		var produtos = _produtoService.ObterTodos();
		var dtos = produtos.Select(p => ToDTO(p)).ToList();
		return Ok(dtos);
	}

	[HttpGet("{id:int}")]
	public ActionResult<ProdutoDTO> ObterPorId(int id)
	{
		var produto = _produtoService.ObterPorId(id);
		if (produto == null)
			return NotFound(new { mensagem = "Produto n�o encontrado." });

		return Ok(ToDTO(produto));
	}

	[HttpGet("buscar")]
	public ActionResult<IEnumerable<ProdutoDTO>> Buscar([FromQuery] string nome)
	{
		var produtos = _produtoService.BuscarProdutos(nome);
		var dtos = produtos.Select(p => ToDTO(p)).ToList();
		return Ok(dtos);
	}

	[HttpPost]
	public ActionResult Adicionar([FromBody] ProdutoDTO produtoDto)
	{
		if (produtoDto == null)
			return BadRequest(new { mensagem = "Dados do produto inv�lidos." });

		var produtoEntity = ToEntity(produtoDto);
		_produtoService.Adicionar(produtoEntity);
		return CreatedAtAction(nameof(ObterPorId), new { id = produtoEntity.Id }, ToDTO(produtoEntity));
	}

	[HttpPut("{id:int}")]
	public ActionResult Atualizar(int id, [FromBody] ProdutoDTO produtoDto)
	{
		if (produtoDto == null || produtoDto.Id != id)
			return BadRequest(new { mensagem = "ID inv�lido." });

		var produtoEntity = ToEntity(produtoDto);
		var atualizado = _produtoService.Atualizar(produtoEntity);
		if (!atualizado)
			return NotFound(new { mensagem = "Produto n�o encontrado para atualiza��o." });

		return NoContent();
	}

	[HttpDelete("{id:int}")]
	public ActionResult Remover(int id)
	{
		var removido = _produtoService.Remover(id);
		if (!removido)
			return NotFound(new { mensagem = "Produto n�o encontrado para exclus�o." });

		return NoContent();
	}

	[HttpPost("upload-imagem")]
	public async Task<ActionResult<object>> UploadImagem(IFormFile arquivo)
	{
		if (arquivo == null || arquivo.Length == 0)
			return BadRequest(new { mensagem = "Arquivo não fornecido." });

		var extensoesPermitidas = new[] { ".jpg", ".jpeg", ".png", ".gif", ".webp" };
		var extensao = Path.GetExtension(arquivo.FileName).ToLowerInvariant();
		if (!extensoesPermitidas.Contains(extensao))
			return BadRequest(new { mensagem = "Formato de arquivo não permitido. Use: jpg, jpeg, png, gif ou webp." });

		if (arquivo.Length > 5 * 1024 * 1024)
			return BadRequest(new { mensagem = "Arquivo muito grande. Tamanho máximo: 5MB." });

		try
		{
			var nomeArquivo = $"{Guid.NewGuid()}{extensao}";
			var caminhoPasta = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img");
			
			if (!Directory.Exists(caminhoPasta))
				Directory.CreateDirectory(caminhoPasta);

			var caminhoCompleto = Path.Combine(caminhoPasta, nomeArquivo);

			using (var stream = new FileStream(caminhoCompleto, FileMode.Create))
			{
				await arquivo.CopyToAsync(stream);
			}

			var urlImagem = $"/img/{nomeArquivo}";
			return Ok(new { urlImagem, mensagem = "Imagem enviada com sucesso." });
		}
		catch (Exception ex)
		{
			return StatusCode(500, new { mensagem = $"Erro ao fazer upload: {ex.Message}" });
		}
	}
	private static ProdutoDTO ToDTO(Produto p) =>
		new ProdutoDTO 
		{ 
			Id = p.Id, 
			Nome = p.Nome, 
			Preco = p.Preco,
			Descricao = p.Descricao ?? string.Empty,
			Estoque = p.Estoque,
			ImagemUrl = p.ImagemUrl ?? string.Empty
		};

	private static Produto ToEntity(ProdutoDTO dto) =>
		new Produto 
		{ 
			Id = dto.Id, 
			Nome = dto.Nome, 
			Preco = dto.Preco,
			Descricao = dto.Descricao,
			Estoque = dto.Estoque,
			ImagemUrl = dto.ImagemUrl
		};
}