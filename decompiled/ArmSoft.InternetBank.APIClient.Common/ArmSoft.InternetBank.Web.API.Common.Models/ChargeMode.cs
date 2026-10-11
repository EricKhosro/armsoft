namespace ArmSoft.InternetBank.Web.API.Common.Models;

public class ChargeMode
{
	public string Code { get; set; }

	public string Name { get; set; }

	public ChargeMode(string code, string name)
	{
		Code = code;
		Name = name;
	}
}
