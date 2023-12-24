using MassTransit;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();
ConfigureMassTransit(builder);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();
app.UseHttpsRedirection();
app.Run();

void ConfigureMassTransit(WebApplicationBuilder webApplicationBuilder)
{
    var config = webApplicationBuilder.Configuration;
    var servidor = config.GetSection("MassTransit")["Servidor"]?? string.Empty;
    var user = config.GetSection("MassTransit")["Usuario"]?? string.Empty;
    var pass = config.GetSection("MassTransit")["Senha"]?? string.Empty;

    webApplicationBuilder.Services.AddMassTransit(x =>
    {
        x.UsingRabbitMq((context, cfg) =>
        {
            cfg.Host(servidor, "/", h =>
            {
                h.Username(user);
                h.Password(pass);
            });
            
            cfg.ConfigureEndpoints(context);
        });
    });
}
