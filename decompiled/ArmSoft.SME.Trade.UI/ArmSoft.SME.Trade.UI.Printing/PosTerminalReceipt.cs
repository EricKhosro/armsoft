using System.ComponentModel;
using ArmSoft.SME.Common.Models.Caches;
using ArmSoft.SME.Common.Printing;
using ArmSoft.SME.Core.UI;
using ArmSoft.SME.Core.UI.Printing;
using ArmSoft.SME.Trade.Common.Models.PrintableElements;

namespace ArmSoft.SME.Trade.UI.Printing;

[PrintModel(107)]
public class PosTerminalReceipt : TradeDocumentPrintWrapper
{
	private readonly ArmSoft.SME.Trade.Common.Models.PrintableElements.PosTerminalReceipt _doc;

	[PrintDataMember("Company")]
	public CompanyRequisites Company { get; set; }

	[PrintDataMember("PoleDisplayOptionsHeader")]
	public byte[] HeaderImage { get; set; }

	[PrintDataMember("Check")]
	public string Receipt { get; set; }

	public PosTerminalReceipt()
	{
	}

	public PosTerminalReceipt(ArmSoft.SME.Trade.Common.Models.PrintableElements.PosTerminalReceipt doc, TemplatesCacheRow template)
		: base(template)
	{
		_doc = doc;
	}

	public override DocumentPrintWrapper ConstructTemplate()
	{
		Company = new CompanyRequisites(Settings.ParametersService);
		HeaderImage = _doc.HeaderImage;
		Receipt = _doc.Receipt;
		return this;
	}

	public override string InitXtraReportDataSource(ref object dataSource)
	{
		string result = string.Empty;
		BindingList<PosTerminalReceipt> bindingList = new BindingList<PosTerminalReceipt>();
		if (this != null)
		{
			bindingList.Add(this);
			result = _doc.Caption;
		}
		dataSource = bindingList;
		return result;
	}
}
