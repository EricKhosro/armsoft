namespace ArmSoft.SME.Common.MetaData;

public interface IValidate
{
	bool Validate(object value, DataItem di, out string message);
}
