using System;

namespace ArmSoft.SME.Trade.Common.Models.Directories;

public class EMarkReceiptApp : TradeDirectory
{
	public string CRN { get; set; }

	public Guid DeviceId { get; set; }

	public bool Enabled { get; set; }

	public string Token { get; set; }

	public string RefreshToken { get; set; }
}
