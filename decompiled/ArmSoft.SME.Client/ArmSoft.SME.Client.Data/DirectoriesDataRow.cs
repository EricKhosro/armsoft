using ArmSoft.SME.Common.Models;

namespace ArmSoft.SME.Client.Data;

public class DirectoriesDataRow : ExtendableRow, IMatchedUpdateKey, IRowWithKey
{
	public int Id { get; set; }

	public string Code { get; set; }

	public string Name { get; set; }

	public bool IsClosed { get; set; }

	public virtual string GetName()
	{
		return Name;
	}

	public virtual string GetCode()
	{
		return Code;
	}

	public virtual object GetKey()
	{
		if (Id <= 0)
		{
			return Code;
		}
		return Id;
	}

	public virtual bool Matched(object key)
	{
		if (Id > 0)
		{
			return (int)key == Id;
		}
		return (string)key == Code;
	}
}
