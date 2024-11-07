using Application.DTOs.Pagamentos;
using Domain.Entities;

namespace Application.UseCase.Pagamentos
{
    public interface IPagamentoUseCase
    {
        Task<Pagamento> GerarPagamento(Pedido pedido);
        Task<Pagamento> AtualizarPagamento(AtualizarPagamentoDto pagamento);
    }
}
