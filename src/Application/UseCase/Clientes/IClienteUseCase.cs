using Application.DTOs;
using Application.DTOs.Clientes;
using Domain.Entities;

namespace Application.UseCase.Clientes
{
    public interface IClienteUseCase
    {
        Task<ClienteDto> Obter(string cpf);
        Task<Result> Cadastrar(CadastrarClienteDto cliente);
    }
}
