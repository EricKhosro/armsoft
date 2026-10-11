using System.Text.Json;
using ArmSoft.SME.Common.Enums;

namespace ArmSoft.SME.Common.Models.Job;

public class JobUIRequestResponse
{
	public UIRequestType Kind { get; set; }

	public JsonElement Value { get; set; }
}
