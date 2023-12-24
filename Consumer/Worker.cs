using System.Text;
using System.Text.Json;
using Core;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Consumer;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;

    public Worker(ILogger<Worker> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Asynchronously executes the method.
    /// </summary>
    /// <param name="stoppingToken">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            var factory = new ConnectionFactory() { HostName = "localhost", UserName = "guest", Password = "guest" };
            
            using var connection = factory.CreateConnection();
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(
                    queue: "fila",
                    durable: false,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null);

                // essa função fica observando a fila e quando chega uma mensagem, ela é executada
                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (sender, eventArgs) =>
                {
                    var body = eventArgs.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);
                    var pedido = JsonSerializer.Deserialize<Pedido>(message);
                    
                    Console.WriteLine(pedido?.ToString());
                    _logger.LogInformation($"Mensagem recebida: {message}");
                };
                // Depois de consumir a mensagem, ela é excluída da fila
                channel.BasicConsume(queue: "fila", autoAck: true, consumer: consumer);
            }
        }
    }
}