using Newtonsoft.Json;
using OllamaSharp.Models.Chat;

namespace MyFirstAIChatApp.Models
{
	public class GroqChoice
	{
		[JsonProperty("index")]
		public int Index { get; set; }

		[JsonProperty("message")]
		public GroqMessage Message { get; set; }
	}
}