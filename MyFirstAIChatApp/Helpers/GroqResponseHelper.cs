using MyFirstAIChatApp.Models;
using Newtonsoft.Json;

namespace MyFirstAIChatApp.Helpers
{
	public class GroqResponseHelper
	{
		public static GroqMessage GetLatestMessage(string jsonString)
		{
			GroqMessage latestMessage = null!;
			try
			{
				GroqResponse response = JsonConvert.DeserializeObject<GroqResponse>(jsonString);

				if (response != null && response.Choices != null && response.Choices.Length > 0)
				{
					latestMessage = response.Choices[0].Message;
				}
				else
				{
					Console.WriteLine("No message found in the response.");
				}
			}
			catch (JsonException ex)
			{
				Console.Error.WriteLine($"JSON Deserialization Error: {ex.Message}");
			}

			return latestMessage;
		}
	}
}