using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace ArmSoft.Cloud.Client.Directories;

[JsonConverter(typeof(StringEnumConverter))]
public enum IndexerExecutionStatus
{
	TransientFailure,
	Success,
	InProgress,
	Reset
}
