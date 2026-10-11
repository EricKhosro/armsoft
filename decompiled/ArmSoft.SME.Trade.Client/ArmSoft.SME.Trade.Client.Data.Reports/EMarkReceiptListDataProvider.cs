using System;
using ArmSoft.SME.Client;
using ArmSoft.SME.Client.Data;
using ArmSoft.SME.Common.Models;
using ArmSoft.SME.Trade.Common.SmartEnums;

namespace ArmSoft.SME.Trade.Client.Data.Reports;

public class EMarkReceiptListDataProvider : DataProvider<EMarkReceiptListDataProvider.DataRow, EMarkReceiptListDataProvider.Param>
{
	public class DataRow : ExtendableRow, IMatchedUpdateKey, IRowWithISN
	{
		public Guid ISN { get; set; }

		public DateOnly DocDate { get; set; }

		public string DocNum { get; set; }

		public string EMarkReceiptNumber { get; set; }

		public string Storage { get; set; }

		public string StorageName { get; set; }

		public byte DocState { get; set; }

		public string DocStateName { get; set; }

		public string Comment { get; set; }

		public string UserName { get; set; }

		public DateTime ChangeDate { get; set; }

		public string DocOwner { get; set; }

		public string ItemCode { get; set; }

		public string ItemName { get; set; }

		public string UnitCode { get; set; }

		public string UnitBrief { get; set; }

		public decimal Quantity { get; set; }

		public Guid GetDocISN()
		{
			return ISN;
		}

		public short GetDocType()
		{
			return DocumentTypeSE.EMarkReceipt.Value;
		}

		public bool Matched(object key)
		{
			return (Guid)key == ISN;
		}
	}

	public class Param
	{
		public DateOnly StartDate { get; set; }

		public DateOnly EndDate { get; set; }

		public string MTCode { get; set; }

		public string DocState { get; set; }
	}

	public override string Name => "EMarkReceiptList";

	public EMarkReceiptListDataProvider(TradeClient client)
		: base((ApiClient)client)
	{
	}
}
