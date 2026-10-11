using System;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Extensions;
using ArmSoft.SME.Core.UI.DataViews;
using ArmSoft.SME.Trade.Common;
using ArmSoft.SME.Trade.UI.DataViewDataProvider.Offline;

namespace ArmSoft.SME.Trade.UI.DropDown;

public class LocalDBDocuments : DropDownView<LocalDBDocumentsDataProvider.DataRow, LocalDBDocumentsDataProvider.Param>
{
	public required DateOnly StartDate { get; set; }

	public required DateOnly EndDate { get; set; }

	public string CurrCode { get; set; }

	public string CashDeskCode { get; set; }

	public string CashierShiftNumber { get; set; }

	public byte DocumentState { get; set; }

	public bool OrderByAscedning { get; set; }

	public string DocType { get; set; }

	public override string Caption => ArmSoft.SME.Common.Resources.OfflineECRChecksJournal + " " + StartDate.FormatDDMMYY() + " - " + EndDate.FormatDDMMYY();

	public override string Name => "LocalDBDocuments";

	public override ReloadMode ReloadMode => ReloadMode.Reloadable;

	public override string HelpId => "Sales_Journals_ECR_Offline";

	public override bool KeyMemberSupported => true;

	public LocalDBDocuments()
	{
		DataProvider = new LocalDBDocumentsDataProvider();
		ConfigureColumns();
	}

	private void ConfigureColumns()
	{
		Columns.Add("DocDate", ArmSoft.SME.Common.Resources.DateWord);
		Columns.Add("LastModifyDate", ArmSoft.SME.Common.Resources.Time).Format = "H:mm:ss";
		Columns.Add("DocNum", ArmSoft.SME.Common.Resources.DocNumber);
		Columns.Add("ECRCheckNum", ArmSoft.SME.Common.Resources.ECRCheckNum);
		Columns.Add("DocTypeName", ArmSoft.SME.Common.Resources.TypeWord);
		Columns.Add("Summ", ArmSoft.SME.Common.Resources.Summa).Format = DecimalExtension.GetCurrencyFormat(16, 2);
		Columns.Add("Cashdesk", ArmSoft.SME.Common.Resources.Cash);
		Columns.Add("CashierShiftNumber", ArmSoft.SME.Trade.Common.Resources.CashierShiftNum);
		Columns.Add("DocOwner", ArmSoft.SME.Common.Resources.DocOwner);
	}

	public override short ValueMemberLength()
	{
		return 255;
	}

	public override object KeyMember(int index)
	{
		return Data[index].ISN;
	}

	public override string ValueMember(int index)
	{
		return Data[index].Comment;
	}

	public override string CommentMember(int index)
	{
		return Data[index].Comment;
	}

	protected override void ApplyParametersToDataProvider()
	{
		ParametersProvider.StartDate = StartDate;
		ParametersProvider.EndDate = EndDate;
		ParametersProvider.DocState = DocumentState.ToString();
		ParametersProvider.CurrCode = CurrCode;
		ParametersProvider.CashDeskCode = CashDeskCode;
		ParametersProvider.CashierShiftNumber = CashierShiftNumber;
		ParametersProvider.OrderByAscedning = OrderByAscedning;
		ParametersProvider.DocType = DocType;
	}
}
