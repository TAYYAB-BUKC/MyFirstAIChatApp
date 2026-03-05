using GroqApiLibrary;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using MyFirstAIChatApp.Helpers;
using System.Text.Json.Nodes;

namespace MyFirstAIChatApp
{
	public partial class WebChatApp(IHostApplicationLifetime applicationLifetime, IConfiguration configuration, HttpClient httpClient) : BackgroundService
	{
		private static bool exitRequested = false;
		private JsonArray history = [];

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

			history = new JsonArray
			{
				new JsonObject
				{
					["role"] = "system",
					["content"] = "You are an AI assistant that tries to answer the user's query."
				}
			};

			Console.WriteLine("system: You are an AI assistant that tries to answer the user's query.");

			var initialRequest = new JsonObject
			{
				["model"] = GroqModels.Llama33_70B,
				["messages"] = history
			};

			JsonObject? response = await chatClient.CreateChatCompletionAsync(initialRequest);
			var message = GroqResponseHelper.GetLatestMessage(Convert.ToString(response));
			history.Add(new JsonObject
			{
				["role"] = message.Role,
				["content"] = message.Content
			});
			Console.WriteLine($"{message.Role}: {message.Content}");

			while (!stoppingToken.IsCancellationRequested)
			{
				Console.WriteLine("Prompt: ");
				string? userPrompt = Console.ReadLine();
				if (userPrompt is null || exitRequested)
					break;

				history.Add(new JsonObject
				{
					["role"] = Convert.ToString(ChatRole.User),
					["content"] = userPrompt
				});

				var userRequest = new JsonObject
				{
					["model"] = GroqModels.Llama33_70B,
					["messages"] = GroqRequestHelper.DeepCloneMessages(history)
				};

				JsonObject? userResponse = await chatClient.CreateChatCompletionAsync(userRequest);
				message = GroqResponseHelper.GetLatestMessage(Convert.ToString(userResponse));

				history.Add(new JsonObject
				{
					["role"] = message.Role,
					["content"] = message.Content
				});

				Console.WriteLine($"{message.Role}: {message.Content}");
			}
		}
	}
}