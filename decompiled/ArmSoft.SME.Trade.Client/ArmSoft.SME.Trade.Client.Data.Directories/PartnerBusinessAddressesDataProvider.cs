using ArmSoft.SME.Client;
using ArmSoft.SME.Client.Data;

namespace ArmSoft.SME.Trade.Client.Data.Directories;

public class PartnerBusinessAddressesDataProvider : DataProvider<PartnerBusinessAddressesDataProvider.DataRow, PartnerBusinessAddressesDataProvider.Param>
{
	public class DataRow : ExtendableRow
	{
		public int PartnerId { get; set; }

		public string PartenrCode { get; set; }

		public string PartnerName { get; set; }

		public string BusinessAddress { get; set; }

		public bool IsDefault { get; set; }
	}

	public class Param : ExtendableParameter
	{
		public string PartnerCode { get; set; }
	}

	public override string Name => "PartnerBusinessAddresses";

	public PartnerBusinessAddressesDataProvider(TradeClient client)
		: base((ApiClient)client)
	{
	}
}
