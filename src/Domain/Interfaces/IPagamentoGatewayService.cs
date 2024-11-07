using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IPagamentoGatewayService
    {
        Task<Pagamento> EnviarPagamento(Pedido pedido);
    }
}
