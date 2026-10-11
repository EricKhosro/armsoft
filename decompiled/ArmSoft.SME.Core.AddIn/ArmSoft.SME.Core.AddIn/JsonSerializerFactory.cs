using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ArmSoft.SME.Core.AddIn;

public static class JsonSerializerFactory
{
	public static JsonSerializerOptions JsonOptions { get; private set; }

	static JsonSerializerFactory()
	{
		JsonOptions = new JsonSerializerOptions();
		FillJsonOptions(JsonOptions);
	}

	public static JsonSerializerOptions CloneJsonOptions()
	{
		JsonSerializerOptions jsonSerializerOptions = new JsonSerializerOptions();
		FillJsonOptions(jsonSerializerOptions);
		return jsonSerializerOptions;
	}

	public static void FillJsonOptions(JsonSerializerOptions options)
	{
		options.Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping;
		options.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
		options.PropertyNameCaseInsensitive = true;
	}
}
