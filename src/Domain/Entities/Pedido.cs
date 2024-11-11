using Domain.Enums;

namespace Domain.Entities
{
    public class Pedido
    {
        public Pedido(long id, StatusEnum status)
        {
            Id = id;
            Status = status;
        }
        public void AtualizarStatus(StatusEnum status) => Status = status;
        public long Id { get; private set; }    
        public StatusEnum Status { get; private set; }
    }
}
