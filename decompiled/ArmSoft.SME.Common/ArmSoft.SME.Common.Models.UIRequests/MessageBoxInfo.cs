using System.Collections.Generic;
using ArmSoft.SME.Common.Enums;

namespace ArmSoft.SME.Common.Models.UIRequests;

public class MessageBoxInfo : UIRequestSourceBase
{
	public string Prompt { get; set; }

	public string Title { get; set; }

	public MessageBoxButtons Buttons { get; set; }

	public MessageBoxIconType Icon { get; set; }

	public MessageBoxDefaultButton DefaultButton { get; set; }

	public List<WarningModel> Warnings { get; set; }
}
