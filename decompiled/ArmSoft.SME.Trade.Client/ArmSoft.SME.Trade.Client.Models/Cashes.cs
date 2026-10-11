using System.Collections.Generic;
using ArmSoft.SME.Client.Data;
using ArmSoft.SME.Common.Models;

namespace ArmSoft.SME.Trade.Client.Models;

public class Cashes : ExtendableRow, IRowWithKey, IMatchedUpdateKey
{
	public string Code { get; set; }

	public string Caption { get; set; }

	public bool IsDefault { get; set; }

	public string NextNumIn { get; set; }

	public string NextNumOut { get; set; }

	public string NextMTBillNum { get; set; }

	public string NextMTBillReturnNum { get; set; }

	public string NextCashierShiftNum { get; set; }

	public bool IsECR { get; set; }

	public string ECRSN { get; set; }

	public string ECRCrn { get; set; }

	public string ECRIP { get; set; }

	public string ECRPort { get; set; }

	public string ECRPassword { get; set; }

	public string ECROperator { get; set; }

	public string ECROperatorPassword { get; set; }

	public string ECRDepWithVAT { get; set; }

	public string ECRDepWithOutVAT { get; set; }

	public string ECRPrintMode { get; set; }

	public string CashSummLimitType { get; set; }

	public bool ShowECRTracking { get; set; }

	public bool B2BPublish { get; set; }

	public Dictionary<int, NonCashInfo> NonCashInfoDict { get; set; }

	public List<NonCashInfo> NonCashInfoList { get; set; }

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
		return Code == (string)key;
	}
}
