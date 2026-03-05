using System.Text.Json.Nodes;

namespace MyFirstAIChatApp.Helpers
{
	public class GroqRequestHelper
	{
		public static JsonArray DeepCloneMessages(JsonArray messages)
		{
			return JsonNode.Parse(messages.ToJsonString())!.AsArray();
		}
	}
}