using Application.DTOs;
using Application.DTOs.Clientes;
using Domain.Repositories;
using AutoMapper;
using Domain.Entities;
using Domain.ValueObjects;

namespace Application.UseCase.Clientes
{
    public class ClienteUseCase : IClienteUseCase
    {
        private readonly IClienteRepository _repository;
        private readonly IMapper _mapper;

        public ClienteUseCase(IClienteRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        public async Task<Result> Cadastrar(CadastrarClienteDto cadastrarClienteDto)
        {
            var cliente = new Cliente(cadastrarClienteDto.Nome, new Email(cadastrarClienteDto.Email), new CPF(cadastrarClienteDto.Cpf));
            if (cliente.ClienteValido())
            {
                if (_repository.ValidaCliente(cliente.Cpf.Numero))
                    throw new Exception("Cliente já cadastrado");

                await _repository.Inserir(cliente);
            }

            return new Result() { Mensagem = "Cliente cadastrado com sucesso" };
        }

        public async Task<ClienteDto> Obter(string cpf)
        {
            var cpfValueObject = new CPF(cpf);
            if (!cpfValueObject.EhValido())
                throw new Exception("CPF inválido");

            return _mapper.Map<ClienteDto>(await _repository.ObterPorCPF(cpfValueObject.Numero));
        }
    }
}