using LojaVirtual.API.Entities;
using LojaVirtual.Business.DTOs;
using LojaVirtual.Business.Interfaces;
using System.Text.RegularExpressions;

namespace LojaVirtual.Business.Services;

public class PedidoService : IPedidoService
{
	private readonly IPedidoRepository _pedidoRepository;
	private readonly ICarrinhoService _carrinhoService;
	private readonly IProdutoService _produtoService;
	private readonly IUsuarioRepository _usuarioRepository;
	private readonly IEmailService _emailService;

	public PedidoService(
		IPedidoRepository pedidoRepository,
		ICarrinhoService carrinhoService,
		IProdutoService produtoService,
		IUsuarioRepository usuarioRepository,
		IEmailService emailService)
	{
		_pedidoRepository = pedidoRepository;
		_carrinhoService = carrinhoService;
		_produtoService = produtoService;
		_usuarioRepository = usuarioRepository;
		_emailService = emailService;
	}

	public async Task<PedidoDTO> CriarPedidoAsync(int usuarioId, PagamentoDTO pagamentoDTO)
	{
		ValidarPagamento(pagamentoDTO);

		var itensCarrinho = _carrinhoService.ObterItensCarrinho();
		if (itensCarrinho == null || !itensCarrinho.Any())
			throw new InvalidOperationException("Carrinho está vazio.");

		var usuario = await _usuarioRepository.ObterPorIdAsync(usuarioId);
		if (usuario == null)
			throw new InvalidOperationException("Usuário não encontrado.");

		var pedido = new Pedido
		{
			UsuarioId = usuarioId,
			DataPedido = DateTime.Now,
			ValorTotal = _carrinhoService.CalcularTotal(),
			Status = "Confirmado",
			MetodoPagamento = pagamentoDTO.MetodoPagamento,
			Banco = pagamentoDTO.Banco,
			NumeroCartao = ObterUltimosDigitos(pagamentoDTO.NumeroCartao)
		};

		foreach (var itemCarrinho in itensCarrinho)
		{
			var produto = _produtoService.ObterPorId(itemCarrinho.Id);
			if (produto == null) continue;

			var pedidoItem = new PedidoItem
			{
				ProdutoId = produto.Id,
				Quantidade = itemCarrinho.Quantidade,
				PrecoUnitario = produto.Preco,
				Subtotal = produto.Preco * itemCarrinho.Quantidade
			};

			pedido.Itens.Add(pedidoItem);
		}

		pedido = await _pedidoRepository.CriarAsync(pedido);

		foreach (var item in itensCarrinho.ToList())
		{
			_carrinhoService.RemoverItem(item.Id);
		}

		try
		{
			await _emailService.EnviarEmailConfirmacaoCompraAsync(usuario.Email, usuario.Nome, pedido);
		}
		catch (Exception ex)
		{
			Console.WriteLine($"⚠️ Erro ao enviar e-mail de confirmação: {ex.Message}");
		}

		return ConverterParaDTO(pedido);
	}

	public async Task<PedidoDTO?> ObterPedidoPorIdAsync(int pedidoId)
	{
		var pedido = await _pedidoRepository.ObterPorIdAsync(pedidoId);
		if (pedido == null) return null;

		return ConverterParaDTO(pedido);
	}

	public async Task<List<PedidoDTO>> ObterPedidosPorUsuarioAsync(int usuarioId)
	{
		var pedidos = await _pedidoRepository.ObterPorUsuarioIdAsync(usuarioId);
		return pedidos.Select(ConverterParaDTO).ToList();
	}

	private void ValidarPagamento(PagamentoDTO pagamentoDTO)
	{
		if (string.IsNullOrWhiteSpace(pagamentoDTO.MetodoPagamento))
			throw new ArgumentException("Método de pagamento é obrigatório.");

		if (string.IsNullOrWhiteSpace(pagamentoDTO.Banco))
			throw new ArgumentException("Banco é obrigatório.");

		if (string.IsNullOrWhiteSpace(pagamentoDTO.NumeroCartao))
			throw new ArgumentException("Número do cartão é obrigatório.");

		var numeroLimpo = Regex.Replace(pagamentoDTO.NumeroCartao, @"\s+", "");
		if (!Regex.IsMatch(numeroLimpo, @"^\d{13,19}$"))
			throw new ArgumentException("Número do cartão inválido.");

		if (string.IsNullOrWhiteSpace(pagamentoDTO.NomeTitular))
			throw new ArgumentException("Nome do titular é obrigatório.");

		if (string.IsNullOrWhiteSpace(pagamentoDTO.Validade))
			throw new ArgumentException("Validade é obrigatória.");

		if (string.IsNullOrWhiteSpace(pagamentoDTO.CVV))
			throw new ArgumentException("CVV é obrigatório.");

		if (!Regex.IsMatch(pagamentoDTO.CVV, @"^\d{3,4}$"))
			throw new ArgumentException("CVV inválido.");
	}

	private string ObterUltimosDigitos(string numeroCartao)
	{
		var numeroLimpo = Regex.Replace(numeroCartao, @"\s+", "");
		if (numeroLimpo.Length >= 4)
			return numeroLimpo.Substring(numeroLimpo.Length - 4);
		return numeroLimpo;
	}

	private PedidoDTO ConverterParaDTO(Pedido pedido)
	{
		return new PedidoDTO
		{
			Id = pedido.Id,
			DataPedido = pedido.DataPedido,
			ValorTotal = pedido.ValorTotal,
			Status = pedido.Status,
			MetodoPagamento = pedido.MetodoPagamento,
			Banco = pedido.Banco,
			Itens = pedido.Itens.Select(i => new PedidoItemDTO
			{
				ProdutoId = i.ProdutoId,
				NomeProduto = i.Produto?.Nome ?? "Produto",
				Quantidade = i.Quantidade,
				PrecoUnitario = i.PrecoUnitario,
				Subtotal = i.Subtotal
			}).ToList()
		};
	}
}

