using System;
using ArmSoft.SME.Common.Models.UIRequests;

namespace ArmSoft.SME.Client.UIRequest;

public class UIRequestEventArgs
{
	public Guid Guid { get; set; }

	public UIRequestData UIRequestData { get; set; }

	public UIRequestResult UIRequestResult { get; set; }
}
