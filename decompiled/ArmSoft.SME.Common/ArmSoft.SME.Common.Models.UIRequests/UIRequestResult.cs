using System.Text.Json;
using ArmSoft.SME.Common.Models.Job;

namespace ArmSoft.SME.Common.Models.UIRequests;

public class UIRequestResult : JobProgressRequest
{
	public JsonElement Data { get; set; }
}
