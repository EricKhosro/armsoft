using System.Text.Json;
using ArmSoft.SME.Common.Enums;

namespace ArmSoft.SME.Common.Models.UIRequests;

public class MessageBoxResult : UIRequestResultBase
{
	public MessageBoxRequestResult UIRequestResult { get; set; }

	public override void Deserialize(JsonElement jsonElement)
	{
		MessageBoxResult messageBoxResult = jsonElement.Deserialize<MessageBoxResult>(JsonSerializerOptions);
		UIRequestResult = messageBoxResult.UIRequestResult;
		UIRequestResultState = messageBoxResult.UIRequestResultState;
		RememberTheAnswer = messageBoxResult.RememberTheAnswer;
	}
}
