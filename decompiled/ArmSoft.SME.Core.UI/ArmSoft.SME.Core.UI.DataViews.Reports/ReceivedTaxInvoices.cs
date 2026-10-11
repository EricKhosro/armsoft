using ArmSoft.SME.Core.UI.DataViewsData.Reports;
using ArmSoft.TaxService.Enums;
using DevExpress.Data;

namespace ArmSoft.SME.Core.UI.DataViews.Reports;

public class ReceivedTaxInvoices : TaxBase
{
	public override TaxDocType TaxDocType => TaxDocType.Invoice;

	public override Status TaxDocStatus => Status.Approved;

	public override string HelpId => "e-Invoicing_ReceivedTaxInvoices";

	public ReceivedTaxInvoices()
	{
		DataProvider = new ReceivedTaxInvoicesDataProvider();
		AddParameters();
		FillColumnsFromDataProvider();
	}

	public override void BeforeLoadData(DataViewPanel dvp)
	{
		base.BeforeLoadData(dvp);
		dvp.Set_ColumnVisible("CreationDate", value: false);
		dvp.Set_ColumnVisible("BuyerTin", value: false);
		dvp.Set_ColumnVisible("BuyerName", value: false);
		dvp.Set_ColumnVisible("CancelledDate", value: false);
		dvp.Set_ColumnVisible("CancelResponseDate", value: false);
		dvp.Set_ColumnVisible("ReceivedOrSubmitted", value: false);
		if (!dvp.IsSorted)
		{
			dvp.Set_ColumnSortOrder("SubmitDate", ColumnSortOrder.Descending);
		}
	}
}
