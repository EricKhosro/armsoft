using System;
using System.Collections.Generic;
using ArmSoft.SME.Common.Models;

namespace ArmSoft.SME.Trade.Common.Models.ASReport;

public class PartCashBankBonusTAccRepResult
{
	public class PartCashBankBonusTAccRow : IRowWithISN
	{
		public DateOnly OpDate { get; set; }

		public CommonConst.AccountingsTypes AccountType { get; set; }

		public int PartnerID { get; set; } = -1;

		public string CashBankCode { get; set; } = "";

		public string PartnerCode { get; set; } = "";

		public string PartnerCaption { get; set; } = "";

		public string CurrCode { get; set; } = "";

		public string ContractCode { get; set; } = "";

		public bool DbCr { get; set; }

		public decimal SummIn { get; set; }

		public decimal CurrSummIn { get; set; }

		public decimal SummOut { get; set; }

		public decimal CurrSummOut { get; set; }

		public Guid BaseISN { get; set; }

		public string Comment { get; set; } = "";

		public string Op { get; set; } = "";

		public string DocNum { get; set; } = "";

		public short DocType { get; set; }

		public string DocTypeCaption { get; set; } = "";

		public Guid GetDocISN()
		{
			return BaseISN;
		}

		public short GetDocType()
		{
			return DocType;
		}
	}

	public int PartnerID { get; set; } = -1;

	public string PartnerCode { get; set; } = "";

	public string PartnerCaption { get; set; } = "";

	public string CurrCode { get; set; } = "";

	public string ContractCode { get; set; } = "";

	public string CashBankCode { get; set; } = "";

	public string CashBankName { get; set; } = "";

	public decimal StartRemCurr { get; set; }

	public decimal StartRemNCC { get; set; }

	public decimal TurnOverInCurr { get; set; }

	public decimal TurnOverOutCurr { get; set; }

	public decimal TurnOverInNCC { get; set; }

	public decimal TurnOverOutNCC { get; set; }

	public decimal EndRemCurr { get; set; }

	public decimal EndRemNCC { get; set; }

	public List<PartCashBankBonusTAccRow> TAccountRows { get; set; }
}
