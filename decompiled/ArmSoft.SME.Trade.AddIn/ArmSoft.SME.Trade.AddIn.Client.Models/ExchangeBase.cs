using System.Runtime.InteropServices;

namespace ArmSoft.SME.Trade.AddIn.Client.Models;

[ComVisible(true)]
public class ExchangeBase
{
	public bool IsDraft { get; set; }

	public bool IsDeleted { get; set; }
}
