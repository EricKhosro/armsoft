using System.Text.Json;

namespace ArmSoft.SME.Common.Models.Job;

public class JobDPRRequest : JobUIRequest
{
	public JsonElement Request { get; set; }
}
