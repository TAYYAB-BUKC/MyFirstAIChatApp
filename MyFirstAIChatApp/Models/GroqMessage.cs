using Newtonsoft.Json;

namespace MyFirstAIChatApp.Models
{
	public class GroqMessage
	{
		[JsonProperty("role")]
		public string Role { get; set; }

		[JsonProperty("content")]
		public string Content { get; set; }
	}
}