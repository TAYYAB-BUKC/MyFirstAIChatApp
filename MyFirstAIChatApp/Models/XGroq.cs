using Newtonsoft.Json;

namespace MyFirstAIChatApp.Models
{
	public class XGroq
	{
		[JsonProperty("id")]
		public string Id { get; set; }

		[JsonProperty("seed")]
		public int Seed { get; set; }
	}
}