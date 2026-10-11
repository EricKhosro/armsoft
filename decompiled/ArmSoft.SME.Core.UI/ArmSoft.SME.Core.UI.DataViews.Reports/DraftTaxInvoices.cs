using ArmSoft.SME.Core.UI.DataViewsData.Reports;
using ArmSoft.SME.Core.UI.Documents;
using ArmSoft.TaxService.Enums;
using DevExpress.Data;

namespace ArmSoft.SME.Core.UI.DataViews.Reports;

[DataView(15)]
public class DraftTaxInvoices : TaxBase
{
	public override TaxDocType TaxDocType => TaxDocType.Invoice;

	public override Status TaxDocStatus => Status.Draft;

	public override string HelpId => "e-Invoicing_DraftTaxInvoice";

	public DraftTaxInvoices()
	{
		DataProvider = new DraftTaxInvoicesDataProvider();
		AddParameters();
		FillColumnsFromDataProvider();
	}

	public override void BeforeLoadData(DataViewPanel dvp)
	{
		base.BeforeLoadData(dvp);
		dvp.Set_ColumnVisible("SubmitDate", value: false);
		dvp.Set_ColumnVisible("TaxSerialNumber", value: false);
		dvp.Set_ColumnVisible("SupplierTin", value: false);
		dvp.Set_ColumnVisible("SupplierName", value: false);
		dvp.Set_ColumnVisible("Status", value: false);
		dvp.Set_ColumnVisible("CoSignDate", value: false);
		dvp.Set_ColumnVisible("SubStatusName", value: false);
		dvp.Set_ColumnVisible("SupplierTaxRegimeCaption", value: false);
		dvp.Set_ColumnVisible("SupplierTaxRegimeStart", value: false);
		dvp.Set_ColumnVisible("FinalUse", value: false);
		dvp.Set_ColumnVisible("CancelledDate", value: false);
		dvp.Set_ColumnVisible("CancelResponseDate", value: false);
		dvp.Set_ColumnVisible("ReceivedOrSubmitted", value: false);
		if (!dvp.IsSorted)
		{
			dvp.Set_ColumnSortOrder("CreationDate", ColumnSortOrder.Descending);
		}
	}
}
