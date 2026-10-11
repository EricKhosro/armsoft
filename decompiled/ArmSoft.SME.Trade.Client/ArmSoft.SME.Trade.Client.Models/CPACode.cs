using System;
using ArmSoft.SME.Client.Data;
using ArmSoft.SME.Common.Models;

namespace ArmSoft.SME.Trade.Client.Models;

public class CPACode : ExtendableRow, IRowWithKey, IMatchedUpdateKey
{
	public string Code { get; set; }

	public string Caption { get; set; }

	public string Type { get; set; }

	public string TypeName { get; set; }

	public DateOnly? MarkInitDate { get; set; }

	public object GetKey()
	{
		return Code;
	}

	public string GetCode()
	{
		return Code;
	}

	public string GetName()
	{
		return Caption;
	}

	public bool Matched(object key)
	{
		return key.ToString() == Code;
	}
}
