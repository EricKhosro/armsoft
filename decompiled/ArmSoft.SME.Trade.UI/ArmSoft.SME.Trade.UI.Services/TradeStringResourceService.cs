using ArmSoft.SME.Core.UI.Services;
using ArmSoft.SME.Trade.Common;

namespace ArmSoft.SME.Trade.UI.Services;

public class TradeStringResourceService : StringResourceService
{
	public override string GetStringResource(string captionId)
	{
		if (string.IsNullOrWhiteSpace(captionId))
		{
			return "";
		}
		return ((string)typeof(Resources).GetProperty(captionId)?.GetValue(captionId)) ?? base.GetStringResource(captionId);
	}
}
