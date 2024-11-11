using Application.DTOs;
using Application.DTOs.Pedido;
using Domain.Enums;

namespace Application.UseCase.Pedidos
{
    public interface IPedidoUseCase
    {
        Task<Result<object>> Inserir(PedidoDto pedidoDto);       
        Task<PedidoDto> AtualizarStatus(long id, int status);
        Task<IEnumerable<PedidoDto>> Listar();
    }
}
