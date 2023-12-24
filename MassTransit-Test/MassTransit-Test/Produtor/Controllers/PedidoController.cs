using Core;
using MassTransit;
using Microsoft.AspNetCore.Mvc;

namespace Produtor.Controllers;

[ApiController]
[Route("/Pedido")]
public class PedidoController : ControllerBase
{
    private readonly IBus _bus;
    private readonly IConfiguration _configuration;
    
    public PedidoController(
        IBus bus,
        IConfiguration configuration)
    {
        _bus = bus;
        _configuration = configuration;
    }
    
    [HttpPost]
    public async Task<IActionResult> Post()
    {
        var nomeFila = _configuration
            .GetSection("MassTransit")["NomeFila"] ?? string.Empty;
        
        var endpoint = await _bus
            .GetSendEndpoint(
            new Uri($"queue:{nomeFila}"));

        await endpoint.Send(new Pedido(
            1,
            new Usuario(5, "Rods", "rods@hotmail.com"),
            DateTime.Now));
        
        return Ok();
    }
}