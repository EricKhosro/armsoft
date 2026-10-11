using ArmSoft.SME.Client;
using ArmSoft.SME.Client.Data;
using ArmSoft.SME.Trade.Client.Models;

namespace ArmSoft.SME.Trade.Client.Data.DropDowns;

public class PartnersFilter : DataProvider<Partner, PartnersFilter.Param>
{
	public class Param
	{
		public string PartType { get; set; } = string.Empty;

		public bool IsFilterBySuppliers { get; set; }

		public bool IsFilterByCustomers { get; set; }
	}

	public override string Name => "PartnersFilter";

	public PartnersFilter(TradeClient client)
		: base((ApiClient)client)
	{
	}
}
