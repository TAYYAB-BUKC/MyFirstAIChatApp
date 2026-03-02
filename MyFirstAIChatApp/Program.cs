using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MyFirstAIChatApp;

var host = Host.CreateApplicationBuilder(args);

host.Configuration.AddUserSecrets<Program>();

var endpoint = host.Configuration["Chat:AI:Endpoint"] ?? throw new InvalidOperationException("Missing configuration: Endpoint. See the README for details");

var apiKey = host.Configuration["Chat:AI:ApiKey"] ?? throw new InvalidOperationException("Missing configuration: ApiKey. See the README for details");

host.Services.AddHostedService<ChatApp>();

var app = host.Build();

Console.WriteLine($"{endpoint} - {apiKey}");

await app.RunAsync();