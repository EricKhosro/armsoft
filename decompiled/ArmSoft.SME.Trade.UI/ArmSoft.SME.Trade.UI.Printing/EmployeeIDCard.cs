using System.ComponentModel;
using ArmSoft.SME.Common.Printing;

namespace ArmSoft.SME.Trade.UI.Printing;

public class EmployeeIDCard : TradeDocumentPrintWrapper
{
	public class EmployeeIDCardRow : CodeCaption
	{
		[PrintDataMember("BarCode")]
		public string BarCode { get; set; }

		[PrintDataMember("Employee")]
		public EmployeeInfo Employee { get; set; }

		[PrintDataMember("Company")]
		public CompanyRequisites Company { get; set; }
	}

	[PrintDataMember("List")]
	public BindingList<EmployeeIDCardRow> List { get; set; }
}
