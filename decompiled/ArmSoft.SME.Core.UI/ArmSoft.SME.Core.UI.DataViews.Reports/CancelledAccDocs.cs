using ArmSoft.SME.Core.UI.DataViewsData.Reports;
using ArmSoft.SME.Core.UI.Documents;
using ArmSoft.TaxService.Enums;
using DevExpress.Data;

namespace ArmSoft.SME.Core.UI.DataViews.Reports;

[DataView(37)]
public class CancelledAccDocs : TaxBase
{
	public override TaxDocType TaxDocType => TaxDocType.AccDoc;

	public override Status TaxDocStatus => Status.Cancelled;

	public override string HelpId => "e-Invoicing_CancelledAccountingDocs";

	public CancelledAccDocs()
	{
		DataProvider = new CancelledAccDocsDataProvider();
		AddParameters();
		FillColumnsFromDataProvider();
	}

	public override void BeforeLoadData(DataViewPanel dvp)
	{
		base.BeforeLoadData(dvp);
		dvp.Set_ColumnVisible("CreationDate", value: false);
		dvp.Set_ColumnVisible("SupplierTaxRegimeStart", value: false);
		dvp.Set_ColumnVisible("SupplierTaxRegimeCaption", value: false);
		if (!dvp.IsSorted)
		{
			dvp.Set_ColumnSortOrder("SubmitDate", ColumnSortOrder.Descending);
		}
	}
}
