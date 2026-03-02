using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MyFirstAIChatApp;
using OllamaSharp;

var host = Host.CreateApplicationBuilder(args);

host.Configuration.AddUserSecrets<Program>();

var endpoint = host.Configuration["Chat:AI:Endpoint"] ?? throw new InvalidOperationException("Missing configuration: Endpoint. See the README for details");

var apiKey = host.Configuration["Chat:AI:ApiKey"] ?? throw new InvalidOperationException("Missing configuration: ApiKey. See the README for details");

var ollamaEndpoint = host.Configuration["Chat:AI:OllamaEndpoint"] ?? throw new InvalidOperationException("Missing configuration: ApiKey. See the README for details");
var uri = new Uri(ollamaEndpoint);
var ollama = new OllamaApiClient(uri);
host.Services.AddChatClient(ollama);

host.Services.AddHostedService<ChatApp>();

var app = host.Build();

Console.WriteLine($"{endpoint} - {apiKey}");

await app.RunAsync();