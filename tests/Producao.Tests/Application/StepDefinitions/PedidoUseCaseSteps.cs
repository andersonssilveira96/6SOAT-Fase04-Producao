using Application.DTOs;
using Application.DTOs.Pedido;
using Application.UseCase.Pedidos;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using Domain.Producer;
using Domain.Repositories;
using Moq;

namespace Producao.Tests.Application.StepDefinitions
{
    [Binding]
    public class PedidoUseCaseSteps
    {
        private readonly Mock<IPedidoRepository> _mockRepository = new();
        private readonly IMapper _mapper;
        private readonly Mock<IMessageBrokerProducer> _mockMessageBrokerProducer = new();
        private PedidoUseCase _pedidoUseCase;
        private Pedido _pedido;
        private PedidoDto _pedidoDto;
        private Exception _exception;
        private Result<object> _result;
        private IEnumerable<PedidoDto> _listaPedidos;

        public PedidoUseCaseSteps()
        {
            // Configurar o AutoMapper
            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<PedidoProfile>(); // Adiciona o perfil criado para mapeamento
            });

            _mapper = mapperConfig.CreateMapper();

            _pedidoUseCase = new PedidoUseCase(_mockRepository.Object, _mapper, _mockMessageBrokerProducer.Object);

            _mockRepository.Setup(repo => repo.Atualizar(It.IsAny<Pedido>()))
               .ReturnsAsync((Pedido pedidoAtualizado) => pedidoAtualizado);
        }

        [Given(@"um pedido existente com ID ""(.*)"" e status ""(.*)""")]
        public void DadoUmPedidoExistenteComIDEStatus(long id, string status)
        {
            _pedido = new Pedido(id, Enum.Parse<StatusEnum>(status));
            _mockRepository.Setup(repo => repo.ObterPorId(id)).ReturnsAsync(_pedido);
        }

        [When(@"eu atualizar o status do pedido para ""(.*)""")]
        public async Task QuandoEuAtualizarOStatusDoPedidoPara(string status)
        {
            try
            {
                var novoStatus = (int)Enum.Parse<StatusEnum>(status);
                _pedidoDto = await _pedidoUseCase.AtualizarStatus(_pedido.Id, novoStatus);
            }
            catch (Exception ex)
            {
                _exception = ex;
            }
        }

        [Then(@"o status do pedido deve ser atualizado para ""(.*)""")]
        public void EntaoOStatusDoPedidoDeveSerAtualizadoPara(string status)
        {
            Assert.NotNull(_pedidoDto);
            Assert.Equal(status.ToLower(), _pedidoDto.Status.ToLower());
        }

        [Then(@"uma exceçãoo deve ser lançada com a mensagem ""(.*)""")]
        public void EntaoUmaExcecaoDeveSerLancadaComAMensagem(string mensagem)
        {
            Assert.NotNull(_exception);
            Assert.Equal(mensagem, _exception.Message);
        }

        [Given(@"um pedido com ID ""(.*)"" está pronto para ser cadastrado")]
        public void DadoUmPedidoComIDEstaProntoParaSerCadastrado(long id)
        {
            _pedidoDto = new PedidoDto { Id = id };
        }

        [When(@"eu inserir o pedido")]
        public async Task QuandoEuInserirOPedido()
        {
            _result = await _pedidoUseCase.Inserir(_pedidoDto);
        }

        [Then(@"uma mensagem de sucesso deve ser retornada como ""(.*)""")]
        public void EntaoUmaMensagemDeSucessoDeveSerRetornadaComo(string mensagem)
        {
            Assert.NotNull(_result);
            Assert.Equal(mensagem, _result.Mensagem);
        }

        [Given(@"existem pedidos no sistema")]
        public void DadoExistemPedidosNoSistema()
        {
            var pedidos = new List<Pedido>
            {
                new Pedido(1, StatusEnum.Recebido),
                new Pedido(2, StatusEnum.Finalizado)
            };
            _mockRepository.Setup(repo => repo.ListarPedidos()).ReturnsAsync(pedidos);           
        }

        [When(@"eu listar os pedidos")]
        public async Task QuandoEuListarOsPedidos()
        {
            _listaPedidos = await _pedidoUseCase.Listar();
        }

        [Then(@"a lista de pedidos deve ser retornada")]
        public void EntaoAListaDePedidosDeveSerRetornada()
        {
            Assert.NotNull(_listaPedidos);
            Assert.Equal(2, _listaPedidos.Count());
        }

        [When(@"eu tentar atualizar o status do pedido para ""(.*)""")]
        public async Task WhenEuTentarAtualizarOStatusDoPedidoPara(string status)
        {
            try
            {
                // Converter o status para enumeração
                var novoStatus = (int)Enum.Parse<StatusEnum>(status);

                // Executar o método de atualização
                _pedidoDto = await _pedidoUseCase.AtualizarStatus(_pedido.Id, novoStatus);
            }
            catch (Exception ex)
            {
                // Capturar a exceção para validação posterior
                _exception = ex;
            }
        }
    }

    public class PedidoProfile : Profile
    {
        public PedidoProfile()
        {
            CreateMap<PedidoDto, Pedido>()
                .ConstructUsing(dto => new Pedido(dto.Id, Enum.Parse<StatusEnum>(dto.Status))) 
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status)).ReverseMap();
        }
    }
}
