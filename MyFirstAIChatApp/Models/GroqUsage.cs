using Newtonsoft.Json;

namespace MyFirstAIChatApp.Models
{
	public class GroqUsage
	{
		[JsonProperty("queue_time")]
		public double QueueTime { get; set; }

		[JsonProperty("prompt_tokens")]
		public int PromptTokens { get; set; }

		[JsonProperty("prompt_time")]
		public double PromptTime { get; set; }

		[JsonProperty("completion_tokens")]
		public int CompletionTokens { get; set; }

		[JsonProperty("completion_time")]
		public double CompletionTime { get; set; }

		[JsonProperty("total_tokens")]
		public int TotalTokens { get; set; }

		[JsonProperty("total_time")]
		public double TotalTime { get; set; }
	}
}