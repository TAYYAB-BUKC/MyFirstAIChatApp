using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

var host = Host.CreateApplicationBuilder(args);

host.Configuration.AddUserSecrets<Program>();

var endpoint = host.Configuration["Chat:AI:Endpoint"] ?? throw new InvalidOperationException("Missing configuration: Endpoint. See the README for details");

var apiKey = host.Configuration["Chat:AI:ApiKey"] ?? throw new InvalidOperationException("Missing configuration: ApiKey. See the README for details");

var app = host.Build();

Console.WriteLine($"{endpoint} - {apiKey}");