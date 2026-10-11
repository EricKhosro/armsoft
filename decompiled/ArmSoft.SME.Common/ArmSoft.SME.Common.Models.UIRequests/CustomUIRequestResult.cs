using System.Text.Json;

namespace ArmSoft.SME.Common.Models.UIRequests;

public class CustomUIRequestResult<T> : UIRequestResultBase
{
	public T Result { get; set; }

	public override void Deserialize(JsonElement jsonElement)
	{
		CustomUIRequestResult<T> customUIRequestResult = jsonElement.Deserialize<CustomUIRequestResult<T>>(JsonSerializerOptions);
		Result = customUIRequestResult.Result;
		UIRequestResultState = customUIRequestResult.UIRequestResultState;
	}
}
