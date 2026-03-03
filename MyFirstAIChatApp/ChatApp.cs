using GroqApiLibrary;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System.Text.Json.Nodes;

namespace MyFirstAIChatApp
{
	public class ChatApp : BackgroundService
	{
		private readonly IHostApplicationLifetime applicationLifetime;
		private readonly IConfiguration configuration;

		public ChatApp(IHostApplicationLifetime applicationLifetime, IConfiguration configuration)
		{
			this.applicationLifetime = applicationLifetime;
			this.configuration = configuration;
		}

		protected override async Task ExecuteAsync(CancellationToken stoppingToken)
		{
			var apiKey = configuration["Chat:AI:ApiKey"];
			var chatClient = new GroqApiClient(apiKey!);

			var request = new JsonObject
			{
				["model"] = GroqModels.Llama33_70B,
				["messages"] = new JsonArray
				{
					new JsonObject
					{
						["role"] = "system",
						["content"] = "You are an AI assistant that tries to answer the user's query."
					}
				}
			};

			JsonObject? response = await chatClient.CreateChatCompletionAsync(request);
			Console.WriteLine($"Response: \n{response}");
			applicationLifetime.StopApplication();
		}
	}
}