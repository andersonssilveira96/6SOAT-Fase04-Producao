using Domain.Entities;
using Domain.Enums;
using Infra.Data.Context;
using Infra.Data.Repositories;
using Microsoft.EntityFrameworkCore;

public class PedidoRepositoryTests
{
    private readonly DbContextOptions<TechChallengeContext> _options;
    private readonly TechChallengeContext _context;
    private readonly PedidoRepository _pedidoRepository;

    public PedidoRepositoryTests()
    {
        _options = new DbContextOptionsBuilder<TechChallengeContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new TechChallengeContext(_options);
        _pedidoRepository = new PedidoRepository(_context);
    }

    [Fact]
    public async Task Inserir_DeveAdicionarPedidoAoContexto()
    {
        // Arrange
        var pedido = new Pedido(1, StatusEnum.Recebido);

        // Act
        var result = await _pedidoRepository.Inserir(pedido);

        // Assert
        var pedidoSalvo = await _context.Pedido.FirstOrDefaultAsync(p => p.Id == pedido.Id);
        Assert.NotNull(pedidoSalvo);
        Assert.Equal(pedido.Id, pedidoSalvo.Id);
        Assert.Equal(pedido.Status, pedidoSalvo.Status);
    }

    [Fact]
    public async Task Inserir_DeveLancarExcecaoQuandoPedidoForNulo()
    {
        // Arrange
        Pedido pedido = null;

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(() => _pedidoRepository.Inserir(pedido));
    }

    [Fact]
    public async Task Atualizar_DeveAtualizarStatusDoPedido()
    {
        // Arrange
        var pedido = new Pedido(1, StatusEnum.Recebido);
        _context.Pedido.Add(pedido);
        await _context.SaveChangesAsync();

        pedido.AtualizarStatus(StatusEnum.Finalizado);

        // Act
        var result = await _pedidoRepository.Atualizar(pedido);

        // Assert
        var pedidoAtualizado = await _context.Pedido.FirstOrDefaultAsync(p => p.Id == pedido.Id);
        Assert.NotNull(pedidoAtualizado);
        Assert.Equal(StatusEnum.Finalizado, pedidoAtualizado.Status);
    }

    [Fact]
    public async Task ListarPedidos_DeveRetornarTodosOsPedidos()
    {
        // Arrange
        var pedidos = new List<Pedido>
        {
            new Pedido(1, StatusEnum.Recebido),
            new Pedido(2, StatusEnum.Finalizado)
        };
        _context.Pedido.AddRange(pedidos);
        await _context.SaveChangesAsync();

        // Act
        var result = await _pedidoRepository.ListarPedidos();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
        Assert.Contains(result, p => p.Id == 1 && p.Status == StatusEnum.Recebido);
        Assert.Contains(result, p => p.Id == 2 && p.Status == StatusEnum.Finalizado);
    }

    [Fact]
    public async Task ObterPorId_DeveRetornarPedidoQuandoIdForValido()
    {
        // Arrange
        var pedido = new Pedido(1, StatusEnum.Recebido);
        _context.Pedido.Add(pedido);
        await _context.SaveChangesAsync();

        // Act
        var result = await _pedidoRepository.ObterPorId(1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
        Assert.Equal(StatusEnum.Recebido, result.Status);
    }

    [Fact]
    public async Task ObterPorId_DeveRetornarNuloQuandoPedidoNaoExistir()
    {
        // Act
        var result = await _pedidoRepository.ObterPorId(999);

        // Assert
        Assert.Null(result);
    }
}
