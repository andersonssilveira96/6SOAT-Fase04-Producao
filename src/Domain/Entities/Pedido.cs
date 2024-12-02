using Domain.Enums;
using System.Text.Json.Serialization;

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

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public StatusEnum Status { get; private set; }
    }
}
