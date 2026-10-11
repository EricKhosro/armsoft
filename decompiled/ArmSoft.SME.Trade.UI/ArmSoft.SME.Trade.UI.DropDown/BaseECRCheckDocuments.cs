using System;
using ArmSoft.SME.Core.UI.DataViews;
using ArmSoft.SME.Trade.Client.Data.Reports;

namespace ArmSoft.SME.Trade.UI.DropDown;

public class BaseECRCheckDocuments : DropDownView<BaseECRCheckDocumentsDataProvider.DataRow, BaseECRCheckDocumentsDataProvider.Param>
{
	private readonly string _eCRCheckNum;

	private readonly DateOnly _endDate;

	private readonly string _docType;

	public BaseECRCheckDocuments(string eCRCheckNum, DateOnly endDate, string docType)
	{
		_eCRCheckNum = eCRCheckNum;
		_endDate = endDate;
		_docType = docType;
		DataProvider = new BaseECRCheckDocumentsDataProvider(TradeSettings.CreateTradeClient());
		FillColumnsFromDataProvider();
		Columns["ISN"].Visible = false;
		Columns.AddSorting("DocDate", descending: true);
		Columns.AddSorting("DocNum");
	}

	protected override void ApplyParametersToDataProvider()
	{
		ParametersProvider.EcrCheckNum = _eCRCheckNum;
		ParametersProvider.EndDate = _endDate;
		ParametersProvider.DocType = _docType;
	}
}
