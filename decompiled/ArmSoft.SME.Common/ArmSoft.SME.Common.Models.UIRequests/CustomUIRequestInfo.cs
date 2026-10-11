using System.Text.Json;

namespace ArmSoft.SME.Common.Models.UIRequests;

public class CustomUIRequestInfo : UIRequestSourceBase
{
	public JsonElement Data { get; set; }
}
