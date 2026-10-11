namespace ArmSoft.InternetBank.Web.API.Common.Models;

public class ForeignNameAndAddressCode
{
	public string Code { get; set; }

	public string Name { get; set; }

	public ForeignNameAndAddressCode(string code, string name)
	{
		Code = code;
		Name = name;
	}
}
