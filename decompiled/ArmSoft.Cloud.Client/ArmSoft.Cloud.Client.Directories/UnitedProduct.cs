namespace ArmSoft.Cloud.Client.Directories;

public class UnitedProduct
{
	public string Name { get; set; }

	public string FullName { get; set; }

	public string Barcode { get; set; }

	public string Classifier { get; set; }

	public bool IsPopular { get; set; }

	public bool IsQuantitative { get; set; }

	public bool? IsVatTaxable { get; set; }

	public string Description { get; set; }
}
