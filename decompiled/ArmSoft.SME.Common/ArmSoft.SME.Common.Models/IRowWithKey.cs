namespace ArmSoft.SME.Common.Models;

public interface IRowWithKey
{
	object GetKey();

	string GetCode();

	string GetName();
}
