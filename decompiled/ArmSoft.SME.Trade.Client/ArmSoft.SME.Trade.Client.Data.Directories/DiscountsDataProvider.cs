using System;
using ArmSoft.SME.Client;
using ArmSoft.SME.Client.Data;
using ArmSoft.SME.Common.Models;

namespace ArmSoft.SME.Trade.Client.Data.Directories;

public class DiscountsDataProvider : DataProvider<DiscountsDataProvider.DataRow, DiscountsDataProvider.Param>
{
	public class DataRow : ExtendableRow, IRowWithKey, IMatchedUpdateKey
	{
		public string Code { get; set; }

		public string Caption { get; set; }

		public string DiscountGroupCode { get; set; }

		public string DiscountGroupCaption { get; set; }

		public DateOnly DateStart { get; set; }

		public DateOnly? DateEnd { get; set; }

		public bool IsActive { get; set; }

		public byte Priority { get; set; }

		public string UseType { get; set; }

		public string UseTypeCaption { get; set; }

		public string DiscountDocType { get; set; }

		public string DiscountDocTypeCaption { get; set; }

		public string SegmentCode { get; set; }

		public string SegmentCaption { get; set; }

		public string Description { get; set; }

		public byte[] TS { get; set; }

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
			return Code == key.ToString();
		}
	}

	public class Param
	{
	}

	public override string Name => "Discounts";

	public DiscountsDataProvider(TradeClient client)
		: base((ApiClient)client)
	{
	}
}
