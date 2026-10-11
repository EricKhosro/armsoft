using System;
using System.ComponentModel.DataAnnotations;
using ArmSoft.SME.Client;
using ArmSoft.SME.Client.Data;

namespace ArmSoft.SME.Trade.Client.Data.Reports;

public class PriceListDataProvider : DataProvider<PriceListDataProvider.DataRow, PriceListDataProvider.Param>
{
	public class DataRow : ExtendableRow
	{
		public int MTID { get; set; } = -1;

		public string MTCode { get; set; }

		public string Caption { get; set; }

		public string Unit { get; set; }

		public string UnitCaption { get; set; }

		public string Group { get; set; }

		public string GroupCaption { get; set; }

		public string Descr { get; set; }

		public string FullCaption { get; set; }

		public string Country { get; set; }

		public string Producer { get; set; }

		public string BarCode { get; set; }

		public byte[] Image { get; set; }

		public bool ImageExists { get; set; }

		public string PricesStr { get; set; }

		public string ImgStr => MTID + "," + Unit.Trim();
	}

	public class Param
	{
		[Required]
		public DateOnly PriceListDate { get; set; }

		public string PriceTypes { get; set; }

		public string Type { get; set; }

		public string Group { get; set; }

		public string MTCode { get; set; }

		public string ItemsShowMode { get; set; }

		public bool ShowAvailables { get; set; }
	}

	public override string Name => "PriceList";

	public PriceListDataProvider(TradeClient client)
		: base((ApiClient)client)
	{
	}
}
