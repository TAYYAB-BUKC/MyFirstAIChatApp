using GroqApiLibrary;
using HtmlAgilityPack;
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
					["content"] = summarizationPrompt,
				}
			};

			string url = "https://dometrain.com/course/from-zero-to-hero-working-with-null-in-csharp/";
			string data = await httpClient.GetStringAsync(url);

			var document = new HtmlDocument();
			document.LoadHtml(data);
			var paragraphNodes = document.DocumentNode.SelectNodes(@"/html/body/div[2]/section[1]/div/div/div[1]/div[3]/div/div[1]");
			var paragraphData = string.Empty;
			if (paragraphNodes is not null)
			{
				paragraphData = String.Join(" ", paragraphNodes.Select(node => node.InnerText.Trim()));
			}

			history.Add(new JsonObject
			{
				["role"] = ChatRole.User.ToString(),
				["content"] = paragraphData
			});

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

			Console.WriteLine("-----------------------------------");
			Console.WriteLine($"{message.Role}: {message.Content}");
			Console.WriteLine("-----------------------------------");
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