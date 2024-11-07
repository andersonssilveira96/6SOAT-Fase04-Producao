using Application.DTOs.Pagamentos;
using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces;
using Domain.Repositories;

namespace Application.UseCase.Pagamentos
{
    public class PagamentoUseCase : IPagamentoUseCase
    {
        private readonly IPagamentoGatewayService _gatewayService;
        private readonly IPagamentoRepository _pagamentoRepository;
        private readonly IPedidoRepository _pedidoRepository;
        public PagamentoUseCase(IPagamentoGatewayService gatewayService, IPagamentoRepository pagamentoRepository, IPedidoRepository pedidoRepository)
        {
            _gatewayService = gatewayService;
            _pagamentoRepository = pagamentoRepository;
            _pedidoRepository = pedidoRepository;
        }

        public async Task<Pagamento> AtualizarPagamento(AtualizarPagamentoDto atualizarPagamentoDto)
        {
            var pagamento = await _pagamentoRepository.ObterPorPedidoId(atualizarPagamentoDto.NumeroPedido);

            if (pagamento is null) throw new Exception("Número de pagamento inválido");

            pagamento.AtualizarStatus(atualizarPagamentoDto.Aprovado);

            await _pagamentoRepository.Atualizar(pagamento);
          
            var pedido = pagamento.Pedido;
            pedido.AtualizarStatus(pagamento.PagamentoAprovado() ? StatusEnum.EmPreparacao : StatusEnum.Cancelado);

            await _pedidoRepository.Atualizar(pedido);

            return pagamento;
        }

        public async Task<Pagamento> GerarPagamento(Pedido pedido)
        {
            var pagamento = await _gatewayService.EnviarPagamento(pedido);

            await _pagamentoRepository.Inserir(pagamento);

            return pagamento;
        }
    }
}
