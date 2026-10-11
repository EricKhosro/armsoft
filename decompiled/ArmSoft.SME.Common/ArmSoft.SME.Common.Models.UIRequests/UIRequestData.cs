using System.Text.Json;

namespace ArmSoft.SME.Common.Models.UIRequests;

public class UIRequestData
{
	public int UIRequestType { get; set; }

	public int SecondsToShow { get; set; }

	public JsonElement Data { get; set; }

	public int? Id { get; set; }

	public bool SupportRememberTheAnswer { get; set; }
}
