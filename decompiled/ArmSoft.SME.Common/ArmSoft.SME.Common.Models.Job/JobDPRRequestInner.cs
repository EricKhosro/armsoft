using System.Text.Json;

namespace ArmSoft.SME.Common.Models.Job;

public class JobDPRRequestInner : JobUIRequestInner
{
	public JsonElement Request { get; set; }
}
