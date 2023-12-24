using Core;
using Microsoft.AspNetCore.Mvc;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace Produtor.Controllers
{
    [ApiController]
    [Route("/Pedido")]
    public class PedidoController : ControllerBase
    {
        [HttpPost]
        public IActionResult Post()
        {
            var factory = new ConnectionFactory() { HostName = "localhost", UserName = "guest", Password = "guest" };
            
            // Criando uma conexão e um canal
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                // Declarando uma fila no servidor RabbitMQ
                channel.QueueDeclare(
                    queue: "minha-fila", // Nome da fila
                    durable: true,       // Se a fila deve ser durável (sobreviver a reinicializações do servidor RabbitMQ)
                    exclusive: false,    // Se a fila deve ser exclusiva (usada apenas por uma conexão)
                    autoDelete: false,   // Se a fila deve ser automaticamente excluída quando não estiver em uso
                    arguments: null
                );

                string message = JsonSerializer
                    .Serialize(new Pedido(1, new Usuario(1, "Carlos", "carlos@email.com"),
                        System.DateTime.Now));
                var body = Encoding.UTF8.GetBytes(message);

                channel.BasicPublish(
                    exchange: "",
                    routingKey: "minha-fila",
                    basicProperties: null,
                    body: body);
            }

            return Ok();
        }
    }
}