using Application.DTOs;
using Application.DTOs.Pedido;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using Domain.Repositories;

namespace Application.UseCase.Pedidos
{
    public class PedidoUseCase : IPedidoUseCase
    {
        private readonly IPedidoRepository _repository;
        private readonly IMapper _mapper;
        public PedidoUseCase(IPedidoRepository repository,IMapper mapper)
        {
            _repository = repository;         
            _mapper = mapper;
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

            return _mapper.Map<PedidoDto>(await _repository.Atualizar(pedido));
        }

        public async Task<Result<object>> Inserir(CadastrarPedidoDto pedidoDto)
        {           
            var pedido = new Pedido();
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
