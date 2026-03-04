using GroqApiLibrary;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using MyFirstAIChatApp.Helpers;
using System.Text.Json.Nodes;

namespace MyFirstAIChatApp
{
	public class ChatApp : BackgroundService
	{
		private readonly IHostApplicationLifetime applicationLifetime;
		private readonly IConfiguration configuration;
		private static bool exitRequested = false;

		public ChatApp(IHostApplicationLifetime applicationLifetime, IConfiguration configuration)
		{
			this.applicationLifetime = applicationLifetime;
			this.configuration = configuration;
		}

		protected override async Task ExecuteAsync(CancellationToken stoppingToken)
		{
			Console.CancelKeyPress += (sender, e) =>
			{
				Console.WriteLine("\nCTRL+C detected. Exiting gracefully...");
				e.Cancel = true;
				applicationLifetime.StopApplication();
				exitRequested = true;
			};

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

			Console.WriteLine("system: You are an AI assistant that tries to answer the user's query.");
			JsonObject? response = await chatClient.CreateChatCompletionAsync(request);
			var message = GroqResponseHelper.GetLatestMessage(Convert.ToString(response));
			Console.WriteLine($"{message.Role}: {message.Content}");

			while (!stoppingToken.IsCancellationRequested)
			{
				Console.WriteLine("Prompt: ");
				string? userPrompt = Console.ReadLine();
				if (userPrompt is null || exitRequested)
					break;

				var userRequest = new JsonObject
				{
					["model"] = GroqModels.Llama33_70B,
					["messages"] = new JsonArray
					{
						new JsonObject
						{
							["role"] = Convert.ToString(ChatRole.User),
							["content"] = userPrompt
						}
					}
				};

				JsonObject? userResponse = await chatClient.CreateChatCompletionAsync(userRequest);
				message = GroqResponseHelper.GetLatestMessage(Convert.ToString(userResponse));
				Console.WriteLine($"{message.Role}: {message.Content}");
			}
		}
	}
}