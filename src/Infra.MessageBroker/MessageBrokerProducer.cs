using Domain.Producer;
using RabbitMQ.Client;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Text;
using Application.DTOs.Pedido;

namespace Infra.MessageBroker
{
    public class MessageBrokerProducer : IMessageBrokerProducer
    {
        public async Task SendMessageAsync<T>(T message)
        {
            var factory = new ConnectionFactory
            {
                HostName = "rabbitmq-service"
            };

            var connection = await factory.CreateConnectionAsync();

            using var channel = await connection.CreateChannelAsync();

            await channel.QueueDeclareAsync("pedidos-atualizados", exclusive: false);

            JsonSerializerOptions options = new()
            {
                ReferenceHandler = ReferenceHandler.IgnoreCycles,
                WriteIndented = 
                true
            };

            string json = JsonSerializer.Serialize(message, options);

            Console.WriteLine($"Pedido: {json}");

            var body = Encoding.UTF8.GetBytes(json);

            await channel.BasicPublishAsync(exchange: "", routingKey: "pedidos-atualizados", body: body);
        }
    }
}
