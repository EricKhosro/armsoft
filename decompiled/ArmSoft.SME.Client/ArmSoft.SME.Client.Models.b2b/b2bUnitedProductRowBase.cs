using ArmSoft.SME.Client.Data;

namespace ArmSoft.SME.Client.Models.b2b;

public class b2bUnitedProductRowBase : ExtendableRow
{
	public string Barcode { get; set; }

	public string Name { get; set; }

	public string FullName { get; set; }

	public string CPACode { get; set; }

	public string CPACaption { get; set; }

	public string Description { get; set; }

	public bool IsWeight { get; set; }

	public bool IsVatTaxable { get; set; }
}
