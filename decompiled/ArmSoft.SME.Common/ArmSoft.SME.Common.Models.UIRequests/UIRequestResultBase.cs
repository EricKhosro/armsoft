using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Unicode;
using ArmSoft.SME.Common.Enums;

namespace ArmSoft.SME.Common.Models.UIRequests;

public abstract class UIRequestResultBase
{
	protected readonly JsonSerializerOptions JsonSerializerOptions;

	public UIRequestResultState UIRequestResultState { get; set; }

	public bool RememberTheAnswer { get; set; }

	protected UIRequestResultBase()
	{
		JsonSerializerOptions = new JsonSerializerOptions
		{
			Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.Latin1Supplement, UnicodeRanges.Cyrillic, UnicodeRanges.Armenian),
			DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
			PropertyNameCaseInsensitive = true
		};
	}

	public abstract void Deserialize(JsonElement jsonElement);
}
