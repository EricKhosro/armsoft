using System;
using ArmSoft.SME.Client;
using ArmSoft.SME.Client.Data;
using ArmSoft.SME.Common.Models;
using ArmSoft.SME.Trade.Common.Models.Caches;

namespace ArmSoft.SME.Trade.Client.Data.Directories;

public class CashierShiftDataProvider : DataProvider<CashierShiftDataProvider.DataRow, CashierShiftDataProvider.Param>
{
	public class DataRow : ExtendableRow, IRowWithKey, IMatchedUpdateKey
	{
		public string CashDesk { get; set; }

		public string ShiftNumber { get; set; }

		public DateTime StartDate { get; set; }

		public DateTime? EndDate { get; set; }

		public decimal InitialRemain { get; set; }

		public object GetKey()
		{
			return new CashierShiftCacheRow.RowKey(CashDesk, ShiftNumber);
		}

		public string GetCode()
		{
			return "";
		}

		public string GetName()
		{
			return "";
		}

		public bool Matched(object key)
		{
			CashierShiftCacheRow.RowKey rowKey = (CashierShiftCacheRow.RowKey)key;
			if (rowKey.ShiftNumber == ShiftNumber)
			{
				return rowKey.CashDeskCode == CashDesk;
			}
			return false;
		}
	}

	public class Param
	{
		public bool CheckUserAccess { get; set; }

		public string CashdeskCode { get; set; }
	}

	public override string Name => "CashierShift";

	public CashierShiftDataProvider(TradeClient client)
		: base((ApiClient)client)
	{
	}
}
