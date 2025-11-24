using LojaVirtual.API.Entities;
using LojaVirtual.Business.Interfaces;
using Microsoft.Extensions.Configuration;
using System.Net;
using System.Net.Mail;

namespace LojaVirtual.Business.Services;

public class EmailService : IEmailService
{
	private readonly IConfiguration _configuration;

	public EmailService(IConfiguration configuration)
	{
		_configuration = configuration;
	}

	public async Task EnviarEmailBoasVindasAsync(string email, string nome)
	{
		var smtpHost = _configuration["Email:SmtpHost"] ?? "smtp.gmail.com";
		var smtpPort = int.Parse(_configuration["Email:SmtpPort"] ?? "587");
		var smtpUser = _configuration["Email:SmtpUser"] ?? "";
		var smtpPassword = _configuration["Email:SmtpPassword"] ?? "";
		var fromEmail = _configuration["Email:FromEmail"] ?? smtpUser;

		if (string.IsNullOrEmpty(smtpUser) || string.IsNullOrEmpty(smtpPassword))
		{
			Console.WriteLine($"📧 [SIMULADO] E-mail de boas-vindas enviado para {email}");
			Console.WriteLine($"   Nome: {nome}");
			Console.WriteLine($"   Configure Email:SmtpUser e Email:SmtpPassword no appsettings.json para envio real");
			return;
		}

		try
		{
			using (var client = new SmtpClient(smtpHost, smtpPort))
			{
				client.EnableSsl = true;
				client.Credentials = new NetworkCredential(smtpUser, smtpPassword);

				var mensagem = new MailMessage
				{
					From = new MailAddress(fromEmail, "Cyber Store"),
					Subject = "Bem-vindo à Cyber Store! 🎮",
					Body = $@"
<html>
<head>
	<style>
		body {{ font-family: Arial, sans-serif; line-height: 1.6; color: #333; }}
		.container {{ max-width: 600px; margin: 0 auto; padding: 20px; background: linear-gradient(135deg, #667eea 0%, #764ba2 100%); border-radius: 10px; }}
		.content {{ background: white; padding: 30px; border-radius: 8px; margin-top: 20px; }}
		.header {{ color: white; text-align: center; padding: 20px; }}
		h1 {{ color: #764ba2; margin-top: 0; }}
		.footer {{ text-align: center; color: white; margin-top: 20px; font-size: 12px; }}
		.button {{ display: inline-block; padding: 12px 30px; background: #764ba2; color: white; text-decoration: none; border-radius: 5px; margin: 20px 0; }}
	</style>
</head>
<body>
	<div class='container'>
		<div class='header'>
			<h1 style='margin: 0;'>🎮 Cyber Store</h1>
		</div>
		<div class='content'>
			<h1>Bem-vindo, {nome}!</h1>
			<p>Ficamos muito felizes em tê-lo conosco na <strong>Cyber Store</strong>!</p>
			<p>Estamos aqui para oferecer os melhores produtos gamer com qualidade e performance excepcionais.</p>
			<h3>O que você pode fazer agora:</h3>
			<ul>
				<li>✨ Explorar nossa ampla gama de produtos gamer</li>
				<li>🛒 Adicionar itens ao seu carrinho</li>
				<li>⚡ Aproveitar nossas ofertas especiais</li>
				<li>🎯 Receber atualizações sobre novos lançamentos</li>
			</ul>
			<p>Se você tiver alguma dúvida, nossa equipe está sempre pronta para ajudar!</p>
			<p style='margin-top: 30px;'>
				<a href='#' class='button'>Explorar Produtos</a>
			</p>
			<p style='margin-top: 30px; color: #666; font-size: 14px;'>
				Atenciosamente,<br>
				<strong>Equipe Cyber Store</strong>
			</p>
		</div>
		<div class='footer'>
			<p>© 2025 Cyber Store. Todos os direitos reservados.</p>
		</div>
	</div>
</body>
</html>",
					IsBodyHtml = true
				};

				mensagem.To.Add(email);

				await client.SendMailAsync(mensagem);
				Console.WriteLine($"✅ E-mail de boas-vindas enviado para {email}");
			}
		}
		catch (Exception ex)
		{
			Console.WriteLine($"❌ Erro ao enviar e-mail: {ex.Message}");
			throw;
		}
	}

	public async Task EnviarEmailLoginAsync(string email, string nome)
	{
		var smtpHost = _configuration["Email:SmtpHost"] ?? "smtp.gmail.com";
		var smtpPort = int.Parse(_configuration["Email:SmtpPort"] ?? "587");
		var smtpUser = _configuration["Email:SmtpUser"] ?? "";
		var smtpPassword = _configuration["Email:SmtpPassword"] ?? "";
		var fromEmail = _configuration["Email:FromEmail"] ?? smtpUser;

		if (string.IsNullOrEmpty(smtpUser) || string.IsNullOrEmpty(smtpPassword))
		{
			Console.WriteLine($"📧 [SIMULADO] E-mail de login enviado para {email}");
			Console.WriteLine($"   Nome: {nome}");
			Console.WriteLine($"   Horário: {DateTime.Now:dd/MM/yyyy HH:mm:ss}");
			Console.WriteLine($"   Configure Email:SmtpUser e Email:SmtpPassword no appsettings.json para envio real");
			return;
		}

		try
		{
			using (var client = new SmtpClient(smtpHost, smtpPort))
			{
				client.EnableSsl = true;
				client.Credentials = new NetworkCredential(smtpUser, smtpPassword);

				var mensagem = new MailMessage
				{
					From = new MailAddress(fromEmail, "Cyber Store"),
					Subject = "Login realizado na Cyber Store 🔐",
					Body = $@"
<html>
<head>
	<style>
		body {{ font-family: Arial, sans-serif; line-height: 1.6; color: #333; }}
		.container {{ max-width: 600px; margin: 0 auto; padding: 20px; background: linear-gradient(135deg, #667eea 0%, #764ba2 100%); border-radius: 10px; }}
		.content {{ background: white; padding: 30px; border-radius: 8px; margin-top: 20px; }}
		.header {{ color: white; text-align: center; padding: 20px; }}
		h1 {{ color: #764ba2; margin-top: 0; }}
		.footer {{ text-align: center; color: white; margin-top: 20px; font-size: 12px; }}
		.info-box {{ background: #f0f9ff; border-left: 4px solid #764ba2; padding: 15px; margin: 20px 0; }}
		.alert-box {{ background: #fef3c7; border-left: 4px solid #f59e0b; padding: 15px; margin: 20px 0; }}
	</style>
</head>
<body>
	<div class='container'>
		<div class='header'>
			<h1 style='margin: 0;'>🎮 Cyber Store</h1>
		</div>
		<div class='content'>
			<h1>Login Realizado</h1>
			<p>Olá, <strong>{nome}</strong>!</p>
			<p>Você acabou de fazer login na sua conta da <strong>Cyber Store</strong>.</p>
			
			<div class='info-box'>
				<p><strong>📅 Data e Hora:</strong> {DateTime.Now:dd/MM/yyyy HH:mm:ss}</p>
				<p><strong>🌐 IP:</strong> Detectado automaticamente pelo sistema</p>
			</div>

			<p>Se você não reconhece este acesso, recomendamos que você:</p>
			<ul>
				<li>🔒 Altere sua senha imediatamente</li>
				<li>🔐 Ative a autenticação de dois fatores (se disponível)</li>
				<li>📧 Entre em contato conosco se tiver dúvidas</li>
			</ul>

			<div class='alert-box'>
				<p><strong>⚠️ Importante:</strong> Nunca compartilhe suas credenciais de acesso com terceiros.</p>
			</div>

			<p style='margin-top: 30px; color: #666; font-size: 14px;'>
				Atenciosamente,<br>
				<strong>Equipe Cyber Store</strong>
			</p>
		</div>
		<div class='footer'>
			<p>© 2025 Cyber Store. Todos os direitos reservados.</p>
		</div>
	</div>
</body>
</html>",
					IsBodyHtml = true
				};

				mensagem.To.Add(email);

				await client.SendMailAsync(mensagem);
				Console.WriteLine($"✅ E-mail de login enviado para {email}");
			}
		}
		catch (Exception ex)
		{
			Console.WriteLine($"❌ Erro ao enviar e-mail de login: {ex.Message}");
		}
	}

	public async Task EnviarEmailConfirmacaoCompraAsync(string email, string nome, Pedido pedido)
	{
		var smtpHost = _configuration["Email:SmtpHost"] ?? "smtp.gmail.com";
		var smtpPort = int.Parse(_configuration["Email:SmtpPort"] ?? "587");
		var smtpUser = _configuration["Email:SmtpUser"] ?? "";
		var smtpPassword = _configuration["Email:SmtpPassword"] ?? "";
		var fromEmail = _configuration["Email:FromEmail"] ?? smtpUser;
		
		if (string.IsNullOrEmpty(smtpUser) || string.IsNullOrEmpty(smtpPassword))
		{
			Console.WriteLine($"📧 [SIMULADO] E-mail de confirmação de compra enviado para {email}");
			Console.WriteLine($"   Pedido #{pedido.Id} - Total: R$ {pedido.ValorTotal:F2}");
			Console.WriteLine($"   Configure Email:SmtpUser e Email:SmtpPassword no appsettings.json para envio real");
			return;
		}

		try
		{
			using (var client = new SmtpClient(smtpHost, smtpPort))
			{
				client.EnableSsl = true;
				client.Credentials = new NetworkCredential(smtpUser, smtpPassword);

				var itensHtml = string.Join("", pedido.Itens.Select(item => $@"
					<tr>
						<td style='padding: 10px; border-bottom: 1px solid #eee;'>{item.Produto?.Nome ?? "Produto"}</td>
						<td style='padding: 10px; border-bottom: 1px solid #eee; text-align: center;'>{item.Quantidade}</td>
						<td style='padding: 10px; border-bottom: 1px solid #eee; text-align: right;'>R$ {item.PrecoUnitario:F2}</td>
						<td style='padding: 10px; border-bottom: 1px solid #eee; text-align: right;'>R$ {item.Subtotal:F2}</td>
					</tr>
				"));

				var mensagem = new MailMessage
				{
					From = new MailAddress(fromEmail, "Cyber Store"),
					Subject = $"Compra Confirmada - Pedido #{pedido.Id} 🎮",
					Body = $@"
<html>
<head>
	<style>
		body {{ font-family: Arial, sans-serif; line-height: 1.6; color: #333; }}
		.container {{ max-width: 600px; margin: 0 auto; padding: 20px; background: linear-gradient(135deg, #667eea 0%, #764ba2 100%); border-radius: 10px; }}
		.content {{ background: white; padding: 30px; border-radius: 8px; margin-top: 20px; }}
		.header {{ color: white; text-align: center; padding: 20px; }}
		h1 {{ color: #764ba2; margin-top: 0; }}
		.footer {{ text-align: center; color: white; margin-top: 20px; font-size: 12px; }}
		.success-badge {{ background: #10b981; color: white; padding: 10px 20px; border-radius: 5px; display: inline-block; margin: 10px 0; }}
		table {{ width: 100%; border-collapse: collapse; margin: 20px 0; }}
		th {{ background: #764ba2; color: white; padding: 12px; text-align: left; }}
		.total-row {{ font-weight: bold; font-size: 18px; background: #f3f4f6; }}
	</style>
</head>
<body>
	<div class='container'>
		<div class='header'>
			<h1 style='margin: 0;'>🎮 Cyber Store</h1>
		</div>
		<div class='content'>
			<h1>Compra Confirmada!</h1>
			<div class='success-badge'>✅ Sua compra foi bem-sucedida!</div>
			<p>Olá, <strong>{nome}</strong>!</p>
			<p>Obrigado por comprar na <strong>Cyber Store</strong>! Sua compra foi confirmada com sucesso.</p>
			
			<h3>Detalhes do Pedido:</h3>
			<p><strong>Número do Pedido:</strong> #{pedido.Id}</p>
			<p><strong>Data:</strong> {pedido.DataPedido:dd/MM/yyyy HH:mm}</p>
			<p><strong>Método de Pagamento:</strong> {pedido.MetodoPagamento}</p>
			<p><strong>Banco:</strong> {pedido.Banco}</p>
			<p><strong>Status:</strong> {pedido.Status}</p>

			<h3>Itens do Pedido:</h3>
			<table>
				<thead>
					<tr>
						<th>Produto</th>
						<th style='text-align: center;'>Qtd</th>
						<th style='text-align: right;'>Preço Unit.</th>
						<th style='text-align: right;'>Subtotal</th>
					</tr>
				</thead>
				<tbody>
					{itensHtml}
					<tr class='total-row'>
						<td colspan='3' style='padding: 15px; text-align: right;'><strong>Total:</strong></td>
						<td style='padding: 15px; text-align: right;'><strong>R$ {pedido.ValorTotal:F2}</strong></td>
					</tr>
				</tbody>
			</table>

			<p style='margin-top: 30px; padding: 15px; background: #f0f9ff; border-left: 4px solid #764ba2;'>
				<strong>📦 Envio:</strong> Seus produtos serão enviados em breve. Você receberá um e-mail com o código de rastreamento assim que o pedido for despachado.
			</p>

			<p style='margin-top: 30px; color: #666; font-size: 14px;'>
				Se você tiver alguma dúvida sobre seu pedido, nossa equipe está pronta para ajudar!<br>
				<br>
				Atenciosamente,<br>
				<strong>Equipe Cyber Store</strong>
			</p>
		</div>
		<div class='footer'>
			<p>© 2025 Cyber Store. Todos os direitos reservados.</p>
		</div>
	</div>
</body>
</html>",
					IsBodyHtml = true
				};

				mensagem.To.Add(email);

				await client.SendMailAsync(mensagem);
				Console.WriteLine($"✅ E-mail de confirmação de compra enviado para {email}");
			}
		}
		catch (Exception ex)
		{
			Console.WriteLine($"❌ Erro ao enviar e-mail de confirmação: {ex.Message}");
		}
	}
}

