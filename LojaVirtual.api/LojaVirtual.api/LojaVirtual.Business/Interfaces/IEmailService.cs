using LojaVirtual.API.Entities;

namespace LojaVirtual.Business.Interfaces;

public interface IEmailService
{
	Task EnviarEmailBoasVindasAsync(string email, string nome);
	Task EnviarEmailLoginAsync(string email, string nome);
	Task EnviarEmailConfirmacaoCompraAsync(string email, string nome, Pedido pedido);
}

