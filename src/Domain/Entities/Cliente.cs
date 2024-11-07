using Domain.ValueObjects;

namespace Domain.Entities
{
    public class Cliente
    {
        public Cliente()
        {
            
        }
        public Cliente(string nome, Email email, CPF cpf)
        {
            Nome = nome;
            Email = email;
            Cpf = cpf;
        }
        public long Id { get; private set; }
        public string Nome { get; private set; }
        public Email Email { get; private set; }
        public CPF Cpf { get; private set; }

        public bool ClienteValido()
        {
            if (!Cpf.EhValido()) 
                throw new Exception("CPF inválido");

            if (!Email.EhValido())
                throw new Exception("Email inválido");

            return true;
        }
    }
}
