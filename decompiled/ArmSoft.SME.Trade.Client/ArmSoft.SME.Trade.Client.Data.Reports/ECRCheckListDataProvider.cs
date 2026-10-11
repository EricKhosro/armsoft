using System;
using ArmSoft.SME.Client;
using ArmSoft.SME.Client.Data;
using ArmSoft.SME.Common.Models;
using ArmSoft.SME.Trade.Common.SmartEnums;

namespace ArmSoft.SME.Trade.Client.Data.Reports;

public class ECRCheckListDataProvider : DataProvider<ECRCheckListDataProvider.DataRow, ECRCheckListDataProvider.Param>
{
	public class DataRow : ExtendableRow, IMatchedUpdateKey, IRowWithISN
	{
		public Guid ISN { get; set; }

		public DateOnly DocDate { get; set; }

		public string DocNum { get; set; } = string.Empty;

		public string ECRCheckNum { get; set; } = string.Empty;

		public string CurrCode { get; set; } = string.Empty;

		public byte DocState { get; set; }

		public string DocStateCaption { get; set; } = string.Empty;

		public string DocStorage { get; set; } = string.Empty;

		public string Info { get; set; } = string.Empty;

		public string Comment { get; set; } = string.Empty;

		public string PartCode { get; set; } = string.Empty;

		public string PartName { get; set; } = string.Empty;

		public string PartTaxCode { get; set; } = string.Empty;

		public string EmplCode { get; set; } = string.Empty;

		public string EmplName { get; set; } = string.Empty;

		public string UserName { get; set; } = string.Empty;

		public DateTime ChangeDate { get; set; }

		public string DocOwner { get; set; } = string.Empty;

		public string Storage { get; set; } = string.Empty;

		public bool Canceled { get; set; }

		public string RowDescription { get; set; } = string.Empty;

		public string ItemType { get; set; } = string.Empty;

		public string ItemCode { get; set; } = string.Empty;

		public string ItemName { get; set; } = string.Empty;

		public string BarCode { get; set; } = string.Empty;

		public string UnitCode { get; set; } = string.Empty;

		public string UnitBrief { get; set; } = string.Empty;

		public decimal Quantity { get; set; }

		public decimal Price { get; set; }

		public decimal Discount { get; set; }

		public decimal DiscountPrice { get; set; }

		public decimal UsedBonus { get; set; }

		public decimal AccumulatedBonus { get; set; }

		public decimal DiscountSumma { get; set; }

		public decimal Summa { get; set; }

		public bool VAT { get; set; }

		public string Cashdesk { get; set; } = string.Empty;

		public string CashierShiftNumber { get; set; } = string.Empty;

		public Guid GetDocISN()
		{
			return ISN;
		}

		public short GetDocType()
		{
			return DocumentTypeSE.MTBill.Value;
		}

		public bool Matched(object Key)
		{
			return (Guid)Key == ISN;
		}
	}

	public class Param
	{
		public DateOnly StartDate { get; set; }

		public DateOnly EndDate { get; set; }

		public string CashCode { get; set; } = string.Empty;

		public string PartnerCode { get; set; } = string.Empty;

		public string EmployeeCode { get; set; } = string.Empty;

		public string DocState { get; set; } = string.Empty;
	}

	public override string Name => "ECRCheckList";

	public ECRCheckListDataProvider(TradeClient client)
		: base((ApiClient)client)
	{
	}
}
