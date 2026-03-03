using Newtonsoft.Json;

namespace MyFirstAIChatApp.Models
{
	public class GroqResponse
	{
		[JsonProperty("id")]
		public string Id { get; set; }

		[JsonProperty("object")]
		public string Object { get; set; }

		[JsonProperty("created")]
		public long Created { get; set; }

		[JsonProperty("model")]
		public string Model { get; set; }

		[JsonProperty("choices")]
		public GroqChoice[] Choices { get; set; }

		[JsonProperty("usage")]
		public GroqUsage Usage { get; set; }

		[JsonProperty("system_fingerprint")]
		public string SystemFingerprint { get; set; }

		[JsonProperty("x_groq")]
		public XGroq XGroq { get; set; }

		[JsonProperty("service_tier")]
		public string ServiceTier { get; set; }
	}
}