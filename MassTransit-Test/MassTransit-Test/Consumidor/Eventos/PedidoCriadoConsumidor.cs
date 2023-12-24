using Core;
using MassTransit;

namespace Consumidor.Eventos;

/// <summary>
/// Represents a consumer for the PedidoCriado consumer.
/// </summary>
public class PedidoCriadoConsumidor : IConsumer<Pedido>
{
    /// <summary>
    /// Consume method for consuming messages of type Pedido.
    /// </summary>
    /// <param name="context">The ConsumeContext that contains the message to be consumed.</param>
    /// <returns>A Task that represents the asynchronous consume operation.</returns>
    public Task Consume(ConsumeContext<Pedido> context)
    {
        Console.WriteLine($"Pedido criado: {context.Message}");
        return Task.CompletedTask;
    }
}