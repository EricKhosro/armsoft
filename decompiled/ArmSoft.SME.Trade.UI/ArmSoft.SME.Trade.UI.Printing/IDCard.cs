using System.ComponentModel;
using ArmSoft.SME.Common.Printing;

namespace ArmSoft.SME.Trade.UI.Printing;

[PrintModel(102)]
public class IDCard : TradeDocumentPrintWrapper
{
	public class IDCardRow : CodeCaption
	{
		[PrintDataMember("BarCode")]
		public string BarCode { get; set; }

		[PrintDataMember("Contract")]
		public ContractInfo Contract { get; set; }

		[PrintDataMember("Partner")]
		public PartnerInfo Partner { get; set; }

		[PrintDataMember("Company")]
		public CompanyRequisites Company { get; set; }
	}

	[PrintDataMember("List")]
	public BindingList<IDCardRow> List { get; set; }
}
