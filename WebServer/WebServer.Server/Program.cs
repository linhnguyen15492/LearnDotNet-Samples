using WebServer.Server;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<Worker>();

builder.Services.AddSingleton(builder.Configuration.GetRequiredSection("Server").Get<WebServerOptions>() ?? new WebServerOptions());

var host = builder.Build();
host.Run();
