using TakingElano.API;
using Serilog;
using TakingElano.CrossCutting.Logging;
using TakingElano.Infrastructure.Messaging;

var builder = WebApplication.CreateBuilder(args);

 // Serilog
LogConfiguration.ConfigureLogging();
builder.Logging.ClearProviders();
builder.Logging.AddSerilog();     

// Configurar RabbitMQ
var rabbitMqConfig = new RabbitMqConfiguration();
builder.Configuration.GetSection("RabbitMq").Bind(rabbitMqConfig);
builder.Services.AddSingleton(rabbitMqConfig);
builder.Services.AddSingleton<RabbitMqPublisher>();

var startup = new Startup(builder.Configuration);
startup.ConfigureServices(builder.Services);

var app = builder.Build();
startup.Configure(app, app.Environment);

app.Run();

