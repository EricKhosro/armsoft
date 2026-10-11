using System.Runtime.InteropServices;

namespace ArmSoft.SME.Trade.AddIn.Client.Models;

[ComVisible(true)]
[Guid("B324BAFB-5B20-4590-AE7C-65B8C1161602")]
[ClassInterface(ClassInterfaceType.AutoDual)]
public class BonusSpecificationRow
{
	public string PartnerCode { get; set; } = string.Empty;

	public string ContractCode { get; set; } = string.Empty;

	public decimal Bonus { get; set; }
}
