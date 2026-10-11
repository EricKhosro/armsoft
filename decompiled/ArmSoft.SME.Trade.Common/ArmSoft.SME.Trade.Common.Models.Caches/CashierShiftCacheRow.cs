using System;
using System.Text.Json;
using ArmSoft.SME.Common.Models.Caches;

namespace ArmSoft.SME.Trade.Common.Models.Caches;

public class CashierShiftCacheRow : CacheRowBase<CashierShiftCacheRow.RowKey>
{
	public class RowKey
	{
		public string CashDeskCode { get; set; }

		public string ShiftNumber { get; set; }

		public RowKey()
		{
		}

		public RowKey(string cashDeskCode, string shiftNumber)
		{
			CashDeskCode = cashDeskCode;
			ShiftNumber = shiftNumber;
		}

		public override int GetHashCode()
		{
			return CashDeskCode.GetHashCode() ^ ShiftNumber.GetHashCode();
		}

		public override bool Equals(object obj)
		{
			return Equals(obj as RowKey);
		}

		public bool Equals(RowKey obj)
		{
			if (obj != null && obj.CashDeskCode == CashDeskCode)
			{
				return obj.ShiftNumber == ShiftNumber;
			}
			return false;
		}
	}

	public string CashDesk { get; set; } = "";

	public string ShiftNumber { get; set; } = "";

	public DateTime StartDate { get; set; }

	public DateTime? EndDate { get; set; }

	public decimal InitialRemain { get; set; }

	public override string KeyJSON => JsonSerializer.Serialize(new RowKey
	{
		CashDeskCode = CashDesk,
		ShiftNumber = ShiftNumber
	});

	public bool Matched(object key)
	{
		RowKey rowKey = (RowKey)key;
		if (rowKey.ShiftNumber == ShiftNumber)
		{
			return rowKey.CashDeskCode == CashDesk;
		}
		return false;
	}

	public CashierShiftCacheRow()
	{
	}

	public CashierShiftCacheRow(string cashDesk, string shiftNumber, DateTime startDate, DateTime? endDate, decimal initialRemain, byte[] ts)
	{
		CashDesk = cashDesk;
		ShiftNumber = shiftNumber;
		StartDate = startDate;
		EndDate = endDate;
		InitialRemain = initialRemain;
		TS = ts;
	}

	public CashierShiftCacheRow Clone()
	{
		return new CashierShiftCacheRow
		{
			CashDesk = CashDesk,
			ShiftNumber = ShiftNumber,
			StartDate = StartDate,
			EndDate = EndDate,
			InitialRemain = InitialRemain,
			TS = TS
		};
	}
}
