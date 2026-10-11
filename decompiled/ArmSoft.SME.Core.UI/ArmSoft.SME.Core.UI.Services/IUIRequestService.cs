using System;
using System.Collections.Generic;
using System.Text.Json;
using ArmSoft.SME.Common.Models.UIRequests;

namespace ArmSoft.SME.Core.UI.Services;

public interface IUIRequestService
{
	UIRequestResult ShowAndGetResult(UIRequestData uiRequestData, Guid guid, Dictionary<int, Func<JsonElement, JsonElement>> customUIRequest = null);
}
