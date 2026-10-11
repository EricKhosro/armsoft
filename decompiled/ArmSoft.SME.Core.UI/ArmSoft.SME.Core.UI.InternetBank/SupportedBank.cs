using System.Windows.Media;
using ArmSoft.InternetBank.APIClient;

namespace ArmSoft.SME.Core.UI.InternetBank;

public class SupportedBank
{
	public string Code { get; }

	public Bank IBBankId { get; }

	public string Name { get; }

	public ImageSource IconSource
	{
		get
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			return ImageService.GetImage(Icon, isAppPathPreferred: false, "Bank/");
		}
	}

	public string Icon { get; }

	public string IBConnectionMode { get; set; }

	public bool IsConnected { get; set; }

	public bool IsNewAdded { get; set; }

	public SupportedBank(string code, Bank iBBankId, string name, string icon, string ibConnectionMode, bool isConnected, bool isNewAdded)
	{
		Code = code;
		IBBankId = iBBankId;
		Name = name;
		Icon = icon;
		IBConnectionMode = ibConnectionMode;
		IsConnected = isConnected;
		IsNewAdded = isNewAdded;
	}
}
