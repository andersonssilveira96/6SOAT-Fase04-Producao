using Application.DTOs;
using Application.DTOs.Pedido;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using Domain.Producer;
using Domain.Repositories;

namespace Application.UseCase.Pedidos
{
    public class PedidoUseCase : IPedidoUseCase
    {
        private readonly IPedidoRepository _repository;
        private readonly IMapper _mapper;
        private readonly IMessageBrokerProducer _messageBrokerProducer;
        public PedidoUseCase(IPedidoRepository repository, IMapper mapper, IMessageBrokerProducer messageBrokerProducer)
        {
            _repository = repository;         
            _mapper = mapper;
            _messageBrokerProducer = messageBrokerProducer;
        }
        public async Task<PedidoDto> AtualizarStatus(long id, int status)
        {
            var pedido = await _repository.ObterPorId(id);

            if (pedido is null)
                throw new Exception($"PedidoId {id} inválido");

            if (!Enum.IsDefined(typeof(StatusEnum), status))
                throw new Exception($"Status {status} inválido");

            if (pedido.Status > (StatusEnum)status)
                throw new Exception($"Status não pode retroceder");

            pedido.AtualizarStatus((StatusEnum)status);

            await _messageBrokerProducer.SendMessageAsync(pedido);

            return _mapper.Map<PedidoDto>(await _repository.Atualizar(pedido));
        }

        public async Task<Result<object>> Inserir(PedidoDto pedidoDto)
        {           
            var pedido = new Pedido(pedidoDto.Id, StatusEnum.Recebido);
            await _repository.Inserir(pedido);     
   
            return new Result<object> { Mensagem = "Pedido cadastrado com sucesso" };
        }

        public async Task<IEnumerable<PedidoDto>> Listar()
        {
            var listaPedidos = await _repository.ListarPedidos();
          
            return _mapper.Map<IEnumerable<PedidoDto>>(listaPedidos);
        }
    }
}
