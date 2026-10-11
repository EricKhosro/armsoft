using ArmSoft.SME.Client.Data;
using ArmSoft.SME.Common.Models;

namespace ArmSoft.SME.Trade.Client.Models;

public class Currency : ExtendableRow, IRowWithKey, IMatchedUpdateKey
{
	public string Curr { get; set; }

	public string Caption { get; set; }

	public string UnitName { get; set; }

	public string CentName { get; set; }

	public object GetKey()
	{
		return Curr;
	}

	public string GetCode()
	{
		return Curr;
	}

	public string GetName()
	{
		return Caption;
	}

	public bool Matched(object key)
	{
		return key.ToString() == Curr;
	}
}
