namespace ArmSoft.SME.SM.Common.Models.ACS;

public class ProductInfoForBarCode
{
	public string Code { get; set; }

	public string Name { get; set; }

	public string CPACode { get; set; }

	public ProductInfoForBarCode()
	{
	}

	public ProductInfoForBarCode(string code, string name, string cpaCode)
	{
		Code = code;
		Name = name;
		CPACode = cpaCode;
	}
}
