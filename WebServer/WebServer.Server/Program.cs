using WebServer.SDK;
using WebServer.Server;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<Worker>();

builder.Services.AddSingleton(builder.Configuration.GetRequiredSection("Server").Get<WebServerOptions>() ?? new WebServerOptions());
builder.Services.AddSingleton<IRequestReaderFactory>(services => new RequestReaderFactory(services.GetRequiredService<ILoggerFactory>()));
builder.Services.AddSingleton<IResponseWriterFactory>(services => new ResponseWriterFactory(services.GetRequiredService<ILoggerFactory>()));

var host = builder.Build();
host.Run();
